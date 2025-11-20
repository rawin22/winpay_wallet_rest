namespace GPWebApi.DTO;

public class PaymentSettlementHistoryGetRequest
{
    public Guid PaymentId { get; set; }
}

public class PaymentSettlementHistoryGetResponse : DTOResponseBase
{
    public List<PaymentSettlementHistoryGetData> Settlements { get; set; }
}

public class PaymentSettlementHistoryGetData
{
    public Guid SettlementId { get; set; } 
    public string TransmittedBy { get; set; }
    public string TransmittedByName { get; set; }
    public string SettlementMessageType { get ; set; }
    public string SettlementMessageTypeName { get; set; }
    public string TransmittedTime { get; set; }
    public string ProviderResponseTime { get; set; }
    public string ProviderResponse { get; set; }
    public string ProviderResponseText { get; set; }
    public string ProviderResponseTextFormatted { get; set; }
}