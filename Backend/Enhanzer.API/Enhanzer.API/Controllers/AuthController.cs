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
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            AuthService authService,
            ApplicationDbContext context,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("Login validation rejected: Email or password was not provided.");
                return BadRequest(new
                {
                    message = "Email and password are required."
                });
            }

            try
            {
                _logger.LogInformation("Starting login process for email: {Email}", request.Email);

                using var response = await _authService.LoginAsync(
                    request.Email,
                    request.Password
                );

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("External login API returned non-success status code {StatusCode} for email: {Email}", response.StatusCode, request.Email);
                    return StatusCode(502, new
                    {
                        message = "External login API request failed."
                    });
                }

                var responseBody =
                    await response.Content.ReadAsStringAsync();

                _logger.LogDebug("Received raw response from external login API for email: {Email}", request.Email);

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
                    _logger.LogWarning("Enhanzer login rejected for email: {Email}. Reason: {Reason}", request.Email, loginResponse?.Message ?? "Invalid login details.");
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
                    _logger.LogWarning("No locations returned for user: {Email}", request.Email);
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

                _logger.LogInformation("Login successful and {Count} locations saved for email: {Email}", loginData.User_Locations.Count, request.Email);

                return Ok(new
                {
                    message = "Login successful. Locations saved successfully.",
                    email = loginData.Email,
                    userName = loginData.User_Display_Name,
                    locationsSaved = loginData.User_Locations.Count,
                    locations = loginData.User_Locations
                });
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "External Enhanzer login API request timed out for email: {Email}", request.Email);
                return StatusCode(StatusCodes.Status504GatewayTimeout, new
                {
                    message = "External login API request timed out. Please try again."
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request communication failure with Enhanzer login API for email: {Email}", request.Email);
                return StatusCode(502, new
                {
                    message = "Could not communicate with the Enhanzer login API."
                });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization failure for Enhanzer login response for email: {Email}", request.Email);
                return StatusCode(502, new
                {
                    message = "Could not process the external login response."
                });
            }
        }
    }
}
