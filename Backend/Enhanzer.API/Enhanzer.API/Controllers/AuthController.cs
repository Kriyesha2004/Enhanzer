using System.Text.Json;
using Enhanzer.API.Data;
using Enhanzer.API.DTOs;
using Enhanzer.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Enhanzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(
            AuthService authService,
            ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Email and password are required."
                });
            }

            try
            {
                using var response = await _authService.LoginAsync(
                    request.Email,
                    request.Password
                );

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(502, new
                    {
                        message = "External login API request failed."
                    });
                }

                var responseBody =
                    await response.Content.ReadAsStringAsync();

                var loginResponse =
                    JsonSerializer.Deserialize<EnhanzerLoginResponse>(
                        responseBody,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );

                if (loginResponse == null ||
                    loginResponse.Status_Code != 200 ||
                    loginResponse.Response_Body == null ||
                    loginResponse.Response_Body.Count == 0)
                {
                    return Unauthorized(new
                    {
                        message = loginResponse?.Message ??
                                  "Invalid login details."
                    });
                }

                var loginData = loginResponse.Response_Body[0];

                if (loginData.User_Locations == null ||
                    loginData.User_Locations.Count == 0)
                {
                    return BadRequest(new
                    {
                        message = "No locations were returned for this user."
                    });
                }

                foreach (var location in loginData.User_Locations)
                {
                    var existingLocation =
                        await _context.LocationDetails.FirstOrDefaultAsync(
                            x => x.Location_Code == location.Location_Code
                        );

                    if (existingLocation == null)
                    {
                        _context.LocationDetails.Add(
                            new Enhanzer.API.Models.LocationDetail
                            {
                                Location_Code = location.Location_Code,
                                Location_Name = location.Location_Name
                            }
                        );
                    }
                    else
                    {
                        existingLocation.Location_Name =
                            location.Location_Name;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Login successful. Locations saved successfully.",
                    email = loginData.Email,
                    userName = loginData.User_Display_Name,
                    locationsSaved = loginData.User_Locations.Count,
                    locations = loginData.User_Locations
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    message = "Could not communicate with the Enhanzer login API."
                });
            }
            catch (JsonException)
            {
                return StatusCode(502, new
                {
                    message = "Could not process the external login response."
                });
            }
        }
    }
}