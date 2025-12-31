using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GPWebApi.DTO;

public class Problem
{
    public int ProblemCode { get; set; } = 0;
    public ProblemType ProblemType { get; set; }
    public string Message { get; set; } = string.Empty;
    public string MessageDetails { get; set; } = string.Empty;
    public string FieldName { get; set; } = string.Empty;
    public string FieldValue { get; set; } = string.Empty;

    [JsonConstructor]
    public Problem()
    {
    }

    public Problem(int problemCode, ProblemType problemType, string message, string messageDetails)
    {
        ProblemType = problemType;
        ProblemCode = problemCode;
        Message = message;
        MessageDetails = messageDetails;
        FieldName = string.Empty;
        FieldValue = string.Empty;
    }

    public Problem(int problemCode,
                   ProblemType problemType,
                   string message,
                   string messageDetails,
                   string fieldName,
                   string fieldValue)
    {
        ProblemType = problemType;
        ProblemCode = problemCode;
        Message = message;
        MessageDetails = messageDetails;
        FieldName = fieldName;
        FieldValue = fieldValue;
    }
}

public enum ProblemType
{
    [EnumMember]
    Information = 0,
    [EnumMember]
    Error = 1,
    [EnumMember]
    Warning = 2,
}



