using System.Text.Json.Serialization;

namespace Enhanzer.API.DTOs
{
    public class EnhanzerLoginResponse
    {
        [JsonPropertyName("Status_Code")]
        public int Status_Code { get; set; }

        [JsonPropertyName("Sync_Time")]
        public string? Sync_Time { get; set; }

        [JsonPropertyName("Message")]
        public string? Message { get; set; }

        [JsonPropertyName("Response_Body")]
        public List<EnhanzerLoginData>? Response_Body { get; set; }
    }

    public class EnhanzerLoginData
    {
        [JsonPropertyName("User_Code")]
        public string? User_Code { get; set; }

        [JsonPropertyName("User_Display_Name")]
        public string? User_Display_Name { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("User_Locations")]
        public List<EnhanzerLocation>? User_Locations { get; set; }
    }

    public class EnhanzerLocation
    {
        [JsonPropertyName("Location_Code")]
        public string Location_Code { get; set; } = string.Empty;

        [JsonPropertyName("Location_Name")]
        public string Location_Name { get; set; } = string.Empty;
    }
}

