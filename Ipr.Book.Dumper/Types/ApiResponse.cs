using System.Text.Json.Serialization;

namespace Ipr.Book.Dumper.Types {
    public class ApiResponse {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}