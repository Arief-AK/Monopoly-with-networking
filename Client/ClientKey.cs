using System.Text.Json.Serialization;

namespace Client
{
    public class ClientKey
    {
        [JsonPropertyName("_id")]
        public string ID { get; set; }
        public string InGameKey { get; set; }
        public int CurrentPlayer { get; set; }
    }
}