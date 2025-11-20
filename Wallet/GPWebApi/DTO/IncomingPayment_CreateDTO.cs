namespace GPWebApi.DTO;

public class IncomingPaymentCreateRequest
{
    // Payment Information
    public string ReceivingCustomerId { get; set; } = String.Empty;
    public string CountryOfOriginCode { get; set; } = String.Empty;
    public decimal Amount  { get; set; } = Decimal.Zero;
    public string AmountCurrencyCode { get; set; } = String.Empty;
    public string ValueDate { get; set; } = String.Empty;
    public decimal FeeAmount { get; set; } = Decimal.Zero;
    public string FeeAmountCurrencyCode  { get; set; } = String.Empty;
    public string SwiftUETR { get; set; } = String.Empty;
    public string SendingBankReference { get; set; } = String.Empty;
    public string PaymentData { get; set; } = String.Empty;
    public string Memo { get; set; } = String.Empty;

    // Ordering Customer
    public string OrderingCustomerName { get; set; } = String.Empty;
    public string OrderingCustomerAccountNumber { get; set; } = String.Empty;
    public string OrderingCustomerWKYCId { get; set; } = String.Empty;
    public string OrderingCustomerStreetAddress1 { get; set; } = String.Empty;
    public string OrderingCustomerStreetAddress2 { get; set; } = String.Empty;
    public string OrderingCustomerDepartment { get; set; } = String.Empty;
    public string OrderingCustomerSubDepartment { get; set; } = String.Empty;
    public string OrderingCustomerStreetName { get; set; } = String.Empty;
    public string OrderingCustomerBuildingNumber { get; set; } = String.Empty;
    public string OrderingCustomerBuildingName { get; set; } = String.Empty;
    public string OrderingCustomerFloor { get; set; } = String.Empty;
    public string OrderingCustomerPostBox { get; set; } = String.Empty;
    public string OrderingCustomerRoom { get; set; } = String.Empty;
    public string OrderingCustomerCity { get; set; } = String.Empty;
    public string OrderingCustomerTownLocationName { get; set; } = String.Empty;
    public string OrderingCustomerDistrictName { get; set; } = String.Empty;
    public string OrderingCustomerStateOrProvince { get; set; } = String.Empty;
    public string OrderingCustomerPostalCode { get; set; } = String.Empty;
    public string OrderingCustomerCountryCode { get; set; } = String.Empty;

    // Ordering Customer Bank
    public string OrderingCustomerBankName { get; set; } = String.Empty;
    public string OrderingCustomerBankStreetAddress1 { get; set; } = String.Empty;
    public string OrderingCustomerBankStreetAddress2 { get; set; } = String.Empty;
    public string OrderingCustomerBankDepartment { get; set; } = String.Empty;
    public string OrderingCustomerBankSubDepartment { get; set; } = String.Empty;
    public string OrderingCustomerBankStreetName { get; set; } = String.Empty;
    public string OrderingCustomerBankBuildingNumber { get; set; } = String.Empty;
    public string OrderingCustomerBankBuildingName { get; set; } = String.Empty;
    public string OrderingCustomerBankFloor { get; set; } = String.Empty;
    public string OrderingCustomerBankPostBox { get; set; } = String.Empty;
    public string OrderingCustomerBankRoom { get; set; } = String.Empty;
    public string OrderingCustomerBankCity { get; set; } = String.Empty;
    public string OrderingCustomerBankTownLocationName { get; set; } = String.Empty;
    public string OrderingCustomerBankDistrictName { get; set; } = String.Empty;
    public string OrderingCustomerBankStateOrProvince { get; set; } = String.Empty;
    public string OrderingCustomerBankPostalCode { get; set; } = String.Empty;
    public string OrderingCustomerBankCountryCode { get; set; } = String.Empty;
    public string OrderingCustomerBankBIC { get; set; } = String.Empty;
    public string OrderingCustomerBankNationalCodeType { get; set; } = String.Empty;
    public string OrderingCustomerBankNationalCode { get; set; } = String.Empty;

    // Initiating Institution
    public bool InitiatingInstitutionSameAsOrderingInstitution { get; set; } = false;
    public string InitiatingInstitutionName { get; set; } = String.Empty;
    public string InitiatingInstitutionStreetAddress1 { get; set; } = String.Empty;
    public string InitiatingInstitutionStreetAddress2 { get; set; } = String.Empty;
    public string InitiatingInstitutionDepartment { get; set; } = String.Empty;
    public string InitiatingInstitutionSubDepartment { get; set; } = String.Empty;
    public string InitiatingInstitutionStreetName { get; set; } = String.Empty;
    public string InitiatingInstitutionBuildingNumber { get; set; } = String.Empty;
    public string InitiatingInstitutionBuildingName { get; set; } = String.Empty;
    public string InitiatingInstitutionFloor { get; set; } = String.Empty;
    public string InitiatingInstitutionPostBox { get; set; } = String.Empty;
    public string InitiatingInstitutionRoom { get; set; } = String.Empty;
    public string InitiatingInstitutionCity { get; set; } = String.Empty;
    public string InitiatingInstitutionTownLocationName { get; set; } = String.Empty;
    public string InitiatingInstitutionDistrictName { get; set; } = String.Empty;
    public string InitiatingInstitutionStateOrProvince { get; set; } = String.Empty;
    public string InitiatingInstitutionPostalCode { get; set; } = String.Empty;
    public string InitiatingInstitutionCountryCode { get; set; } = String.Empty;
    public string InitiatingInstitutionBIC { get; set; } = String.Empty;
    public string InitiatingInstitutionNationalCodeType { get; set; } = String.Empty;
    public string InitiatingInstitutionNationalCode { get; set; } = String.Empty;

    // Sending Institution
    public bool SendingInstitutionSameAsOrderingInstitution { get; set; } = false;
    public string SendingInstitutionName { get; set; } = String.Empty;
    public string SendingInstitutionStreetAddress1 { get; set; } = String.Empty;
    public string SendingInstitutionStreetAddress2 { get; set; } = String.Empty;
    public string SendingInstitutionDepartment { get; set; } = String.Empty;
    public string SendingInstitutionSubDepartment { get; set; } = String.Empty;
    public string SendingInstitutionStreetName { get; set; } = String.Empty;
    public string SendingInstitutionBuildingNumber { get; set; } = String.Empty;
    public string SendingInstitutionBuildingName { get; set; } = String.Empty;
    public string SendingInstitutionFloor { get; set; } = String.Empty;
    public string SendingInstitutionPostBox { get; set; } = String.Empty;
    public string SendingInstitutionRoom { get; set; } = String.Empty;
    public string SendingInstitutionCity { get; set; } = String.Empty;
    public string SendingInstitutionTownLocationName { get; set; } = String.Empty;
    public string SendingInstitutionDistrictName { get; set; } = String.Empty;
    public string SendingInstitutionStateOrProvince { get; set; } = String.Empty;
    public string SendingInstitutionPostalCode { get; set; } = String.Empty;
    public string SendingInstitutionCountryCode { get; set; } = String.Empty;
    public string SendingInstitutionBIC { get; set; } = String.Empty;
    public string SendingInstitutionNationalCodeType { get; set; } = String.Empty;
    public string SendingInstitutionNationalCode { get; set; } = String.Empty;

    // Beneficiary
    public string BeneficiaryName { get; set; } = String.Empty;
    public string BeneficiaryAccountNumber { get; set; } = String.Empty;
    public string BeneficiaryStreetAddress1 { get; set; } = String.Empty;
    public string BeneficiaryStreetAddress2 { get; set; } = String.Empty;
    public string BeneficiaryDepartment { get; set; } = String.Empty;
    public string BeneficiarySubDepartment { get; set; } = String.Empty;
    public string BeneficiaryStreetName { get; set; } = String.Empty;
    public string BeneficiaryBuildingNumber { get; set; } = String.Empty;
    public string BeneficiaryBuildingName { get; set; } = String.Empty;
    public string BeneficiaryFloor { get; set; } = String.Empty;
    public string BeneficiaryPostBox { get; set; } = String.Empty;
    public string BeneficiaryRoom { get; set; } = String.Empty;
    public string BeneficiaryCity { get; set; } = String.Empty;
    public string BeneficiaryTownLocationName { get; set; } = String.Empty;
    public string BeneficiaryDistrictName { get; set; } = String.Empty;
    public string BeneficiaryStateOrProvince { get; set; } = String.Empty;
    public string BeneficiaryPostalCode { get; set; } = String.Empty;
    public string BeneficiaryCountryCode { get; set; } = String.Empty;
    public string BeneficiaryInfoLine1 { get; set; } = String.Empty;
    public string BeneficiaryInfoLine2 { get; set; } = String.Empty;
    public string BeneficiaryInfoLine3 { get; set; } = String.Empty;
    public string BeneficiaryInfoLine4 { get; set; } = String.Empty;

    // Beneficiary Bank
    public string BeneficiaryBankName { get; set; } = String.Empty;
    public string BeneficiaryBankStreetAddress1 { get; set; } = String.Empty;
    public string BeneficiaryBankStreetAddress2 { get; set; } = String.Empty;
    public string BeneficiaryBankDepartment { get; set; } = String.Empty;
    public string BeneficiaryBankSubDepartment { get; set; } = String.Empty;
    public string BeneficiaryBankStreetName { get; set; } = String.Empty;
    public string BeneficiaryBankBuildingNumber { get; set; } = String.Empty;
    public string BeneficiaryBankBuildingName { get; set; } = String.Empty;
    public string BeneficiaryBankFloor { get; set; } = String.Empty;
    public string BeneficiaryBankPostBox { get; set; } = String.Empty;
    public string BeneficiaryBankRoom { get; set; } = String.Empty;
    public string BeneficiaryBankCity { get; set; } = String.Empty;
    public string BeneficiaryBankTownLocationName { get; set; } = String.Empty;
    public string BeneficiaryBankDistrictName { get; set; } = String.Empty;
    public string BeneficiaryBankStateOrProvince { get; set; } = String.Empty;
    public string BeneficiaryBankPostalCode { get; set; } = String.Empty;
    public string BeneficiaryBankCountryCode { get; set; } = String.Empty;
    public string BeneficiaryBankBIC { get; set; } = String.Empty;
    public string BeneficiaryBankNationalCodeType { get; set; } = String.Empty;
    public string BeneficiaryBankNationalCode { get; set; } = String.Empty;

    // Receiving Institution
    public string ReceivingInstitutionName { get; set; } = String.Empty;
    public string ReceivingInstitutionStreetAddress1 { get; set; } = String.Empty;
    public string ReceivingInstitutionStreetAddress2 { get; set; } = String.Empty;
    public string ReceivingInstitutionDepartment { get; set; } = String.Empty;
    public string ReceivingInstitutionSubDepartment { get; set; } = String.Empty;
    public string ReceivingInstitutionStreetName { get; set; } = String.Empty;
    public string ReceivingInstitutionBuildingNumber { get; set; } = String.Empty;
    public string ReceivingInstitutionBuildingName { get; set; } = String.Empty;
    public string ReceivingInstitutionFloor { get; set; } = String.Empty;
    public string ReceivingInstitutionPostBox { get; set; } = String.Empty;
    public string ReceivingInstitutionRoom { get; set; } = String.Empty;
    public string ReceivingInstitutionCity { get; set; } = String.Empty;
    public string ReceivingInstitutionTownLocationName { get; set; } = String.Empty;
    public string ReceivingInstitutionDistrictName { get; set; } = String.Empty;
    public string ReceivingInstitutionStateOrProvince { get; set; } = String.Empty;
    public string ReceivingInstitutionPostalCode { get; set; } = String.Empty;
    public string ReceivingInstitutionCountryCode { get; set; } = String.Empty;
    public string ReceivingInstitutionBIC { get; set; } = String.Empty;
    public string ReceivingInstitutionNationalCodeType { get; set; } = String.Empty;
    public string ReceivingInstitutionNationalCode { get; set; } = String.Empty;

    // Beneficiary Branch
    public string BeneficiaryBranchName { get; set; } = String.Empty;
    public string BeneficiaryBranchId { get; set; } = String.Empty;
    public string BeneficiaryBranchStreetAddress1 { get; set; } = String.Empty;
    public string BeneficiaryBranchStreetAddress2 { get; set; } = String.Empty;
    public string BeneficiaryBranchDepartment { get; set; } = String.Empty;
    public string BeneficiaryBranchSubDepartment { get; set; } = String.Empty;
    public string BeneficiaryBranchStreetName { get; set; } = String.Empty;
    public string BeneficiaryBranchBuildingNumber { get; set; } = String.Empty;
    public string BeneficiaryBranchBuildingName { get; set; } = String.Empty;
    public string BeneficiaryBranchFloor { get; set; } = String.Empty;
    public string BeneficiaryBranchPostBox { get; set; } = String.Empty;
    public string BeneficiaryBranchRoom { get; set; } = String.Empty;
    public string BeneficiaryBranchCity { get; set; } = String.Empty;
    public string BeneficiaryBranchTownLocationName { get; set; } = String.Empty;
    public string BeneficiaryBranchDistrictName { get; set; } = String.Empty;
    public string BeneficiaryBranchStateOrProvince { get; set; } = String.Empty;
    public string BeneficiaryBranchPostalCode { get; set; } = String.Empty;
    public string BeneficiaryBranchCountryCode { get; set; } = String.Empty;
    public string BeneficiaryBranchBIC { get; set; } = String.Empty;
    public string BeneficiaryBranchNationalCodeType { get; set; } = String.Empty;
    public string BeneficiaryBranchNationalCode { get; set; } = String.Empty;

    // Intermediary Bank
    public string IntermediaryBankName { get; set; } = String.Empty;
    public string IntermediaryBankStreetAddress1 { get; set; } = String.Empty;
    public string IntermediaryBankStreetAddress2 { get; set; } = String.Empty;
    public string IntermediaryBankDepartment { get; set; } = String.Empty;
    public string IntermediaryBankSubDepartment { get; set; } = String.Empty;
    public string IntermediaryBankStreetName { get; set; } = String.Empty;
    public string IntermediaryBankBuildingNumber { get; set; } = String.Empty;
    public string IntermediaryBankBuildingName { get; set; } = String.Empty;
    public string IntermediaryBankFloor { get; set; } = String.Empty;
    public string IntermediaryBankPostBox { get; set; } = String.Empty;
    public string IntermediaryBankRoom { get; set; } = String.Empty;
    public string IntermediaryBankCity { get; set; } = String.Empty;
    public string IntermediaryBankTownLocationName { get; set; } = String.Empty;
    public string IntermediaryBankDistrictName { get; set; } = String.Empty;
    public string IntermediaryBankStateOrProvince { get; set; } = String.Empty;
    public string IntermediaryBankPostalCode { get; set; } = String.Empty;
    public string IntermediaryBankCountryCode { get; set; } = String.Empty;
    public string IntermediaryBankBIC { get; set; } = String.Empty;
    public string IntermediaryBankNationalCodeType { get; set; } = String.Empty;
    public string IntermediaryBankNationalCode { get; set; } = String.Empty;

    public string ReasonForPaymentCode { get; set; } = String.Empty;
    public string ReasonForPayment { get; set; } = String.Empty;
    public string ChargeDetail { get; set; } = String.Empty;
    public string SenderToReceiverInfo1 { get; set; } = String.Empty;
    public string SenderToReceiverInfo2 { get; set; } = String.Empty;
    public string SenderToReceiverInfo3 { get; set; } = String.Empty;
    public string SenderToReceiverInfo4 { get; set; } = String.Empty;
    public string SenderToReceiverInfo5 { get; set; } = String.Empty;
    public string SenderToReceiverInfo6 { get; set; } = String.Empty;
    public string SenderExternalReference { get; set; } = String.Empty;
    public string BankOperationCode { get; set; } = String.Empty;
}

public class IncomingPaymentCreateResponse : DTOResponseBase
{
    public IncomingPaymentCreateData Payment { get; set; }
}

public class IncomingPaymentCreateData
{
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; }
    public string Timestamp { get; set; }
}

