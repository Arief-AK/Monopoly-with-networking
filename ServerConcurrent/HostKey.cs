using System.Text.Json.Serialization;

namespace ServerConcurrent
{
    public class HostKey
    {
        [JsonPropertyName("_id")]
        public string ID { get; set; }
        public string InGameKey { get; set; }
        public int CurrentPlayer { get; set; }
    }
}