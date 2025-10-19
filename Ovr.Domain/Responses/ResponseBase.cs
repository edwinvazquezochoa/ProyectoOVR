using System.Text.Json.Serialization;

namespace Ovr.Domain.Responses
{
    public class ResponseBase<T>
    {
        public ResponseBase() { }

        public ResponseBase(int code, string message, T? data = default)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => Code >= 200 && Code < 300;
    }
}
