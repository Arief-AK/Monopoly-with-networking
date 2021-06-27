using System.Text.Json.Serialization;

namespace ServerConcurrent
{
    public class Message
    {
        [JsonPropertyName("_id")]
        public string ID { get; set; }
        public int StatusCode { get; set; }
        public int PlayerNumber { get; set; }
        public string Text { get; set; }
        public bool Transaction { get; set; }
        public int Value { get; set; }

        public Message(int playerNumber)
        {
            StatusCode = 0;
            PlayerNumber = playerNumber;
            Text = "";
            Transaction = false;
            Value = 0;
        }
    }
}