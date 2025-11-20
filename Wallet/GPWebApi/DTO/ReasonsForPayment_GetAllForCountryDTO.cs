using System.Runtime.Serialization;

namespace GPWebApi.DTO;

public class ReasonsForPaymentGetRequest
{
    public string CountryCode { get; set; }
    public ReasonsForPaymentEntityType EntityType { get; set; } = ReasonsForPaymentEntityType.Business;
}


public enum ReasonsForPaymentEntityType
{
    [EnumMember]
    None = 0,
    [EnumMember]
    Individual = 1,
    [EnumMember]
    Business = 2
}

public class ReasonsForPaymentGetResponse : DTOResponseBase
{
    public List<ReasonForPaymentData> ReasonsForPayment { get; set; } = new List<ReasonForPaymentData>();
}

public class ReasonForPaymentData
{
    public int SettlementMessageTypeId { get; set; } 
    public string SettlementMessageTypeName { get; set; }
    public string ReasonCode { get; set; } 
    public string ReasonDescription { get; set; } 
    public bool IsAdditionalInformationRequired { get; set; }
    public int SortIndex {get; set; }
}