namespace GPWebApi.DTO;

public class PaymentCreateRequest
{
    // Payment Information
    public Guid CustomerId { get; set; }
    public Guid FXDealId { get; set; }
    public decimal Amount { get; set; }
    public string AmountCurrencyCode { get; set; } = string.Empty;
    public string DestinationCountryCode { get; set; } = string.Empty;
    public string ValueDate { get; set; } = string.Empty;
    public decimal FeeAmount { get; set; }
    public string FeeAmountCurrencyCode { get; set; } = string.Empty;
    public string SwiftUETR { get; set; } = string.Empty;
    public string PaymentValueType { get; set; } = "LOW";
    public string OtherReference { get; set; } = string.Empty;
    public bool IsThirdPartyPayment { get; set; } = false;

    // Ordering Customer
    public int OrderingCustomerTypeId { get; set; } 
    public string OrderingCustomerName { get; set; } = string.Empty;
    public string OrderingCustomerFirstName { get; set; } = string.Empty;
    public string OrderingCustomerMiddleName { get; set; } = string.Empty;
    public string OrderingCustomerLastName { get; set; } = string.Empty;
    public string OrderingCustomerStreetAddress1 { get; set; } = string.Empty;
    public string OrderingCustomerStreetAddress2 { get; set; } = string.Empty;
    public string OrderingCustomerDepartment { get; set; } = string.Empty;
    public string OrderingCustomerSubDepartment { get; set; } = string.Empty;
    public string OrderingCustomerStreetName { get; set; } = string.Empty;
    public string OrderingCustomerBuildingNumber { get; set; } = string.Empty;
    public string OrderingCustomerBuildingName { get; set; } = string.Empty;
    public string OrderingCustomerFloor { get; set; } = string.Empty;
    public string OrderingCustomerPostBox { get; set; } = string.Empty;
    public string OrderingCustomerRoom { get; set; } = string.Empty;
    public string OrderingCustomerCity { get; set; } = string.Empty;
    public string OrderingCustomerTownLocationName { get; set; } = string.Empty;
    public string OrderingCustomerDistrictName { get; set; } = string.Empty;
    public string OrderingCustomerStateOrProvince { get; set; } = string.Empty;
    public string OrderingCustomerPostalCode { get; set; } = string.Empty;
    public string OrderingCustomerCountryCode { get; set; } = string.Empty;
    public string OrderingCustomerAddress1 { get; set; } = string.Empty;
    public string OrderingCustomerAddress2 { get; set; } = string.Empty;
    public string OrderingCustomerAddress3 { get; set; } = string.Empty;
    public string OrderingCustomerPhone { get; set; } = string.Empty;
    public string OrderingCustomerEmail { get; set; } = string.Empty;
    public string OrderingCustomerCompanyRegistrationNumber { get; set; } = string.Empty;
    public string OrderingCustomerCompanyRegistrationCountryCode { get; set; } = string.Empty;
    public string OrderingCustomerCompanyRegistrationDate { get; set; } = string.Empty;
    public string OrderingCustomerDateOfBirth { get; set; } = string.Empty;
    public string OrderingCustomerCountryOfBirthCode { get; set; } = string.Empty;
    public int OrderingCustomerIdentificationTypeId { get; set; } = 0;
    public string OrderingCustomerIdentificationNumber { get; set; } = string.Empty;
    public string OrderingCustomerIdentificationCountryCode { get; set; } = string.Empty;
    public string OrderingCustomerAccountNumber { get; set; } = string.Empty;
    public string OrderingCustomerWKYCID { get; set; } = string.Empty;

    // Ordering Customer Bank
    public string OrderingCustomerBankName { get; set; } = string.Empty;
    public string OrderingCustomerBankStreetAddress1 { get; set; } = string.Empty;
    public string OrderingCustomerBankStreetAddress2 { get; set; } = string.Empty;
    public string OrderingCustomerBankDepartment { get; set; } = string.Empty;
    public string OrderingCustomerBankSubDepartment { get; set; } = string.Empty;
    public string OrderingCustomerBankStreetName { get; set; } = string.Empty;
    public string OrderingCustomerBankBuildingNumber { get; set; } = string.Empty;
    public string OrderingCustomerBankBuildingName { get; set; } = string.Empty;
    public string OrderingCustomerBankFloor { get; set; } = string.Empty;   
    public string OrderingCustomerBankPostBox { get; set; } = string.Empty;
    public string OrderingCustomerBankRoom { get; set; } = string.Empty;
    public string OrderingCustomerBankCity { get; set; } = string.Empty;
    public string OrderingCustomerBankTownLocationName { get; set; } = string.Empty;
    public string OrderingCustomerBankDistrictName { get; set; } = string.Empty;
    public string OrderingCustomerBankStateOrProvince { get; set; } = string.Empty;
    public string OrderingCustomerBankPostalCode { get; set; } = string.Empty;
    public string OrderingCustomerBankCountryCode { get; set; } = string.Empty;
    public string OrderingCustomerBankBIC { get; set; } = string.Empty;
    public string OrderingCustomerBankNationalCodeType { get; set; } = string.Empty;
    public string OrderingCustomerBankNationalCode { get; set; } = string.Empty;


    // Initiating Institution
    public bool InitiatingInstitutionSameAsOrderingInstitution { get; set; } = false;
    public string InitiatingInstitutionName { get; set; } = string.Empty;
    public string InitiatingInstitutionStreetAddress1 { get; set; } = string.Empty;
    public string InitiatingInstitutionStreetAddress2 { get; set; } = string.Empty;
    public string InitiatingInstitutionDepartment { get; set; } = string.Empty;
    public string InitiatingInstitutionSubDepartment { get; set; } = string.Empty;
    public string InitiatingInstitutionStreetName { get; set; } = string.Empty;
    public string InitiatingInstitutionBuildingNumber { get; set; } = string.Empty;
    public string InitiatingInstitutionBuildingName { get; set; } = string.Empty;
    public string InitiatingInstitutionFloor { get; set; } = string.Empty;
    public string InitiatingInstitutionPostBox { get; set; } = string.Empty;
    public string InitiatingInstitutionRoom { get; set; } = string.Empty;
    public string InitiatingInstitutionCity { get; set; } = string.Empty;
    public string InitiatingInstitutionTownLocationName { get; set; } = string.Empty;
    public string InitiatingInstitutionDistrictName { get; set; } = string.Empty;
    public string InitiatingInstitutionStateOrProvince { get; set; } = string.Empty;
    public string InitiatingInstitutionPostalCode { get; set; } = string.Empty;
    public string InitiatingInstitutionCountryCode { get; set; } = string.Empty;
    public string InitiatingInstitutionBIC { get; set; } = string.Empty;
    public string InitiatingInstitutionNationalCodeType { get; set; } = string.Empty;
    public string InitiatingInstitutionNationalCode { get; set; } = string.Empty;
    public string InitiatingInstitutionABN { get; set; } = string.Empty;
    public string InitiatingInstitutionACN { get; set; } = string.Empty;
    public string InitiatingInstitutionARBN { get; set; } = string.Empty;

    // Sending Institution
    public bool SendingInstitutionSameAsOrderingInstitution { get; set; } = false;
    public string SendingInstitutionName { get; set; } = string.Empty;
    public string SendingInstitutionStreetAddress1 { get; set; } = string.Empty;
    public string SendingInstitutionStreetAddress2 { get; set; } = string.Empty;
    public string SendingInstitutionDepartment { get; set; } = string.Empty;
    public string SendingInstitutionSubDepartment { get; set; } = string.Empty;
    public string SendingInstitutionStreetName { get; set; } = string.Empty;
    public string SendingInstitutionBuildingNumber { get; set; } = string.Empty;
    public string SendingInstitutionBuildingName { get; set; } = string.Empty;
    public string SendingInstitutionFloor { get; set; } = string.Empty;
    public string SendingInstitutionPostBox { get; set; } = string.Empty;
    public string SendingInstitutionRoom { get; set; } = string.Empty;
    public string SendingInstitutionCity { get; set; } = string.Empty;
    public string SendingInstitutionTownLocationName { get; set; } = string.Empty;      
    public string SendingInstitutionDistrictName { get; set; } = string.Empty;
    public string SendingInstitutionStateOrProvince { get; set; } = string.Empty;
    public string SendingInstitutionPostalCode { get; set; } = string.Empty;
    public string SendingInstitutionCountryCode { get; set; } = string.Empty;
    public string SendingInstitutionBIC { get; set; } = string.Empty;
    public string SendingInstitutionNationalCodeType { get; set; } = string.Empty;
    public string SendingInstitutionNationalCode { get; set; } = string.Empty;
    public string SendingInstitutionPhone { get; set; } = string.Empty;
    public string SendingInstitutionEmail { get; set; } = string.Empty;
    public string SendingInstitutionABN { get; set; } = string.Empty;
    public string SendingInstitutionACN { get; set; } = string.Empty;
    public string SendingInstitutionARBN { get; set; } = string.Empty;
    public string SendingInstitutionBusinessStructureTypeCode { get; set; } = string.Empty;
    public int SendingInstitutionOccupationTypeId { get; set; } = 0;    
    public string SendingInstitutionOccupationCode { get; set; } = string.Empty;
    public string SendingInstitutionOccupationDescription { get; set; } = string.Empty;

    // Beneficiary
    public int BeneficiaryTypeId { get; set; } = 0;
    public string BeneficiaryCompanyRegistrationNumber { get; set; } = string.Empty;
    public string BeneficiaryCompanyRegistrationCountryCode { get; set; } = string.Empty;
    public string BeneficiaryName { get; set; } = string.Empty;
    public string BeneficiaryFirstName { get; set; } = string.Empty;
    public string BeneficiaryMiddleName { get; set; } = string.Empty;
    public string BeneficiaryLastName { get; set; } = string.Empty;
    public string BeneficiaryAddress1 { get; set; } = string.Empty;
    public string BeneficiaryAddress2 { get; set; } = string.Empty;
    public string BeneficiaryAddress3 { get; set; } = string.Empty;
    public string BeneficiaryStreetAddress1 { get; set; } = string.Empty;
    public string BeneficiaryStreetAddress2 { get; set; } = string.Empty;
    public string BeneficiaryDepartment { get; set; } = string.Empty;
    public string BeneficiarySubDepartment { get; set; } = string.Empty;
    public string BeneficiaryStreetName { get; set; } = string.Empty;
    public string BeneficiaryBuildingNumber { get; set; } = string.Empty;
    public string BeneficiaryBuildingName { get; set; } = string.Empty;
    public string BeneficiaryFloor { get; set; } = string.Empty;
    public string BeneficiaryPostBox { get; set; } = string.Empty;
    public string BeneficiaryRoom { get; set; } = string.Empty;
    public string BeneficiaryCity { get; set; } = string.Empty;
    public string BeneficiaryTownLocationName { get; set; } = string.Empty;
    public string BeneficiaryDistrictName { get; set; } = string.Empty;
    public string BeneficiaryStateOrProvince { get; set; } = string.Empty;
    public string BeneficiaryPostalCode { get; set; } = string.Empty;
    public string BeneficiaryCountryCode { get; set; } = string.Empty;
    public string BeneficiaryCellPhone { get; set; } = string.Empty;
    public string BeneficiaryEmail { get; set; } = string.Empty;
    public string BeneficiaryABN { get; set; } = string.Empty;
    public string BeneficiaryACN { get; set; } = string.Empty;
    public string BeneficiaryARBN { get; set; } = string.Empty;
    public string BeneficiaryBusinessStructureTypeCode { get; set; } = string.Empty;
    public int BeneficiaryOccupationTypeId { get; set; }    
    public string BeneficiaryOccupationDescription { get; set; } = string.Empty;
    public string BeneficiaryOccupationCode { get; set; } = string.Empty;
    public int BeneficiaryIdentificationTypeId { get; set; }
    public string BeneficiaryIdentificationNumber { get; set; } = string.Empty;
    public string BeneficiaryIdentificationCountryCode { get; set; } = string.Empty;
    public string BeneficiaryDateOfBirth { get; set; } = string.Empty;
    public string BeneficiaryCountryOfBirthCode { get; set; } = string.Empty;
    public string BeneficiaryAccountTypeCode { get; set; } = string.Empty;
    public string BeneficiaryAccountNumberPrefix { get; set; } = string.Empty;
    public string BeneficiaryAccountNumber { get; set; } = string.Empty;
    public string BeneficiaryAccountNumberSuffix { get; set; } = string.Empty;
    public string BeneficiaryTaxId { get; set; } = string.Empty;
    public string BeneficiaryInfoLine1 { get; set; } = string.Empty;
    public string BeneficiaryInfoLine2 { get; set; } = string.Empty;
    public string BeneficiaryInfoLine3 { get; set; } = string.Empty;
    public string BeneficiaryInfoLine4 { get; set; } = string.Empty;
    public string BeneficiaryExternalReference { get; set; } = string.Empty;
    public string BeneficiaryWKYCId { get; set; } = string.Empty;

    // Beneficiary Bank
    public string BeneficiaryBankName { get; set; } = string.Empty;
    public string BeneficiaryBankStreetAddress1 { get; set; } = string.Empty;
    public string BeneficiaryBankStreetAddress2 { get; set; } = string.Empty;
    public string BeneficiaryBankDepartment { get; set; } = string.Empty;
    public string BeneficiaryBankSubDepartment { get; set; } = string.Empty;
    public string BeneficiaryBankStreetName { get; set; } = string.Empty;
    public string BeneficiaryBankBuildingNumber { get; set; } = string.Empty;
    public string BeneficiaryBankBuildingName { get; set; } = string.Empty;
    public string BeneficiaryBankFloor { get; set; } = string.Empty;
    public string BeneficiaryBankPostBox { get; set; } = string.Empty;
    public string BeneficiaryBankRoom { get; set; } = string.Empty;
    public string BeneficiaryBankCity { get; set; } = string.Empty;
    public string BeneficiaryBankTownLocationName { get; set; } = string.Empty;
    public string BeneficiaryBankDistrictName { get; set; } = string.Empty;
    public string BeneficiaryBankStateOrProvince { get; set; } = string.Empty;
    public string BeneficiaryBankPostalCode { get; set; } = string.Empty;
    public string BeneficiaryBankCountryCode { get; set; } = string.Empty;
    public string BeneficiaryBankAddress1 { get; set; } = string.Empty;
    public string BeneficiaryBankAddress2 { get; set; } = string.Empty;
    public string BeneficiaryBankAddress3 { get; set; } = string.Empty;
    public string BeneficiaryBankBIC { get; set; } = string.Empty;
    public string BeneficiaryBankNationalCodeType { get; set; } = string.Empty;
    public string BeneficiaryBankNationalCode { get; set; } = string.Empty;
    public string BeneficiaryBankCode { get; set; } = string.Empty;
    public string BeneficiaryBranchCode { get; set; } = string.Empty;

    // Receiving Institution
    public string ReceivingInstitutionName { get; set; } = string.Empty;
    public string ReceivingInstitutionStreetAddress1 { get; set; } = string.Empty;
    public string ReceivingInstitutionStreetAddress2 { get; set; } = string.Empty;
    public string ReceivingInstitutionDepartment { get; set; } = string.Empty;
    public string ReceivingInstitutionSubDepartment { get; set; } = string.Empty;
    public string ReceivingInstitutionStreetName { get; set; } = string.Empty;
    public string ReceivingInstitutionBuildingNumber { get; set; } = string.Empty;
    public string ReceivingInstitutionBuildingName { get; set; } = string.Empty;
    public string ReceivingInstitutionFloor { get; set; } = string.Empty;
    public string ReceivingInstitutionPostBox { get; set; } = string.Empty;
    public string ReceivingInstitutionRoom { get; set; } = string.Empty;
    public string ReceivingInstitutionCity { get; set; } = string.Empty;
    public string ReceivingInstitutionTownLocationName { get; set; } = string.Empty;
    public string ReceivingInstitutionDistrictName { get; set; } = string.Empty;
    public string ReceivingInstitutionStateOrProvince { get; set; } = string.Empty;
    public string ReceivingInstitutionPostalCode { get; set; } = string.Empty;
    public string ReceivingInstitutionCountryCode { get; set; } = string.Empty;
    public string ReceivingInstitutionBIC { get; set; } = string.Empty;
    public string ReceivingInstitutionNationalCodeType { get; set; } = string.Empty;
    public string ReceivingInstitutionNationalCode { get; set; } = string.Empty;

    // Beneficiary Branch
    public string BeneficiaryBranchName { get; set; } = string.Empty;
    public string BeneficiaryBranchId { get; set; } = string.Empty;
    public string BeneficiaryBranchStreetAddress1 { get; set; } = string.Empty;
    public string BeneficiaryBranchStreetAddress2 { get; set; } = string.Empty;
    public string BeneficiaryBranchDepartment { get; set; } = string.Empty;
    public string BeneficiaryBranchSubDepartment { get; set; } = string.Empty;
    public string BeneficiaryBranchStreetName { get; set; } = string.Empty;
    public string BeneficiaryBranchBuildingNumber { get; set; } = string.Empty;
    public string BeneficiaryBranchBuildingName { get; set; } = string.Empty;
    public string BeneficiaryBranchFloor { get; set; } = string.Empty;
    public string BeneficiaryBranchPostBox { get; set; } = string.Empty;
    public string BeneficiaryBranchRoom { get; set; } = string.Empty;
    public string BeneficiaryBranchCity { get; set; } = string.Empty;
    public string BeneficiaryBranchTownLocationName { get; set; } = string.Empty;
    public string BeneficiaryBranchDistrictName { get; set; } = string.Empty;
    public string BeneficiaryBranchStateOrProvince { get; set; } = string.Empty;
    public string BeneficiaryBranchPostalCode { get; set; } = string.Empty;
    public string BeneficiaryBranchCountryCode { get; set; } = string.Empty;
    public string BeneficiaryBranchBIC { get; set; } = string.Empty;
    public string BeneficiaryBranchNationalCodeType { get; set; } = string.Empty;
    public string BeneficiaryBranchNationalCode { get; set; } = string.Empty;

    // Intermediary Bank
    public string IntermediaryBankName { get; set; } = string.Empty;
    public string IntermediaryBankStreetAddress1 { get; set; } = string.Empty;
    public string IntermediaryBankStreetAddress2 { get; set; } = string.Empty;
    public string IntermediaryBankDepartment { get; set; } = string.Empty;
    public string IntermediaryBankSubDepartment { get; set; } = string.Empty;
    public string IntermediaryBankStreetName { get; set; } = string.Empty;
    public string IntermediaryBankBuildingNumber { get; set; } = string.Empty;
    public string IntermediaryBankBuildingName { get; set; } = string.Empty;
    public string IntermediaryBankFloor { get; set; } = string.Empty;
    public string IntermediaryBankPostBox { get; set; } = string.Empty;
    public string IntermediaryBankRoom { get; set; } = string.Empty;
    public string IntermediaryBankCity { get; set; } = string.Empty;
    public string IntermediaryBankTownLocationName { get; set; } = string.Empty;
    public string IntermediaryBankDistrictName { get; set; } = string.Empty;
    public string IntermediaryBankStateOrProvince { get; set; } = string.Empty;
    public string IntermediaryBankPostalCode { get; set; } = string.Empty;
    public string IntermediaryBankCountryCode { get; set; } = string.Empty;
    public string IntermediaryBankAddress1 { get; set; } = string.Empty;
    public string IntermediaryBankAddress2 { get; set; } = string.Empty;
    public string IntermediaryBankAddress3 { get; set; } = string.Empty;
    public string IntermediaryBankBIC { get; set; } = string.Empty;
    public string IntermediaryBankNationalCodeType { get; set; } = string.Empty;
    public string IntermediaryBankNationalCode { get; set; } = string.Empty;

    public string ReasonForPaymentCode { get; set; } = string.Empty;
    public string ReasonForPayment { get; set; } = string.Empty;
    public string ChargeDetail { get; set; } = string.Empty;
    public string SenderToReceiverInfo1 { get; set; } = string.Empty;
    public string SenderToReceiverInfo2 { get; set; } = string.Empty;
    public string SenderToReceiverInfo3 { get; set; } = string.Empty;
    public string SenderToReceiverInfo4 { get; set; } = string.Empty;
    public string SenderToReceiverInfo5 { get; set; } = string.Empty;
    public string SenderToReceiverInfo6 { get; set; } = string.Empty;
    public string BankOperationCode { get; set; } = string.Empty;
    public string ReceiverBIC { get; set; } = string.Empty;
}

public class PaymentCreateResponse : DTOResponseBase
{
    public PaymentCreateData Payment { get; set; }
}

public class PaymentCreateData
{
    public string PaymentId { get; set; }
    public string PaymentReference { get; set; }
    public string Timestamp { get; set; }
}


