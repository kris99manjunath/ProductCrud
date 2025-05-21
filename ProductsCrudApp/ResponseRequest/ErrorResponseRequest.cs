
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ProductsCrudApp.ResponseRequest
{
    public class ErrorResponseRequest
    {
        public ErrorCode ErrorCode { get; protected set; }
        public string Message { get; protected set; }
        public Dictionary<string, string[]> Errors { get; protected set; }

        public ErrorResponseRequest(ErrorCode errorCode, string message, Dictionary<string, string[]> errors = null)
        {
            ErrorCode = errorCode;
            Message = message;
            Errors = errors;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorCode
    {
        [EnumMember(Value = "VALIDATION_ERROR")]
        VALIDATION_ERROR,

        [EnumMember(Value = "NOT_FOUND")]
        NOT_FOUND,

        [EnumMember(Value = "INVALID_OPERATION")]
        INVALID_OPERATION,

        [EnumMember(Value = "INTERNAL_ERROR")]
        INTERNAL_ERROR,
    }
}

