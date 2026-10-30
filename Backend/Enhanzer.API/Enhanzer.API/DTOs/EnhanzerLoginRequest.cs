
using System.Text.Json.Serialization;

namespace Enhanzer.API.DTOs
{
    public class EnhanzerLoginRequest
    {
        [JsonPropertyName("API_Action")]
        public string API_Action { get; set; } = "GetLoginData";

        [JsonPropertyName("Device_Id")]
        public string Device_Id { get; set; } = "D001";

        [JsonPropertyName("Sync_Time")]
        public string Sync_Time { get; set; } = "";

        [JsonPropertyName("Company_Code")]
        public string Company_Code { get; set; } = string.Empty;

        [JsonPropertyName("API_Body")]
        public EnhanzerLoginBody API_Body { get; set; } = new();
    }
    public class EnhanzerLoginBody
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("Pw")]
        public string Pw { get; set; } = string.Empty;
    }
}

