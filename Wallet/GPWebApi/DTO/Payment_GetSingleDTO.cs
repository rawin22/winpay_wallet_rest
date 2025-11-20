namespace GPWebApi.DTO;


public class PaymentGetRequest
{
    public Guid PaymentId { get; set; } 
}

public class PaymentGetResponse : DTOResponseBase
{
    public PaymentGetData Payment { get; set; }
}

public class PaymentGetData
{
    // Payment Information
    public Guid PaymentId { get; set; } 
    public string PaymentReference { get; set; } 
    public int PaymentSequenceNumber { get; set; } 
    public int PaymentSourceId { get; set; }
    public string PaymentSource { get; set; } 
    public Guid BankId { get; set; }
    public string SwiftUETR { get; set; }
    public int PaymentStatusTypeId { get; set; } = 0;
    public string PaymentStatusTypeName { get; set; }
    public int ApplicationId { get; set; } = 0;
    public string DestinationCountryCode { get; set; }
    public string DestinationCountryName { get; set; }
    public decimal Amount { get; set; } = Decimal.Zero;
    public string AmountCurrencyCode { get; set; }
    public string AmountTextBare { get; set; }
    public string AmountTextWithCurrencyCode { get; set; }
    public string AmountTextWithCommasAndCurrencyCode { get; set; }
    public string AmountTextBareSWIFT { get; set; }
    public int AmountCurrencyScale { get; set; } = 0;
    public decimal FeeAmount { get; set; } = Decimal.Zero;
    public string FeeAmountCurrencyCode { get; set; }
    public string FeeAmountTextBare { get; set; }
    public string FeeAmountTextWithCurrencyCode { get; set; }
    public string FeeAmountTextWithCommasAndCurrencyCode { get; set; }
    public int FeeAmountCurrencyScale { get; set; } = 0;
    public string ValueDate { get; set; }
    public int WKYCStatusTypeId { get; set; } = 0;
    public string WKYCStatusTypeName { get; set; }
    public string WKYCStatusTypeDescription { get; set; }
    public Guid ProcessingBranchId { get; set; }
    public string ProcessingBranchName { get; set; }
    public string ProcessingBranchCode { get; set; }
    public string ProcessingBranchPhone { get; set; }
    public int SettlementMessageTypeId { get; set; } = 0;
    public string SettlementMessageTypeName { get; set; }
    public string PaymentValueType { get; set; }
    public string OtherReference { get; set; }
    public bool IsThirdPartyPayment { get; set; } = false;
    public int NumberOfPriorCustomerPaymentsToSameBeneficiaryAccount { get; set; } = 0;
    public int NumberOfPriorCustomerPaymentsToSameBeneficiaryAccountPastYear { get; set; } = 0;
    public int NumberOfPriorCustomerPaymentsToSameBeneficiaryAccountAnyCustomer { get; set; } = 0;
    public string ChargeDetail { get; set; }
    public string BankOperationCode { get; set; }
    public bool IsTransmitted { get; set; } = false;
    public string ReceiverBIC { get; set; }
    public string CreatedTime { get; set; }
    public string CreatedBy { get; set; }
    public string CreatedByFullName { get; set; }
    public string SubmittedTime { get; set; }
    public string SubmittedBy { get; set; }
    public string SubmittedByFullName { get; set; }
    public string ApprovedTime { get; set; }
    public string ApprovedBy { get; set; }
    public string ApprovedByFullName { get; set; }
    public string VerifiedTime { get; set; }
    public string VerifiedBy { get; set; }
    public string VerifiedByFullName { get; set; }
    public string ReleasedTime { get; set; }
    public string ReleasedBy { get; set; }
    public string ReleasedByFullName { get; set; }
    public string DeletedTime { get; set; }
    public string DeletedBy { get; set; }
    public string DeletedByFullName { get; set; }
    public bool IsDownloaded { get; set; } = false;
    public bool IsReportable { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public string Timestamp { get; set; }

    // FX Deal
    public Guid? FXDealId { get; set; }
    public string? FXDealReference { get; set; }
    public string? FXDealSymbol { get; set; }
    public string? FXDealBookedRateText { get; set; }
    public string? FXDealBookedRateTextWithSymbol { get; set; }
    public string? FXDealAllFeesText { get; set; }
    public string? FXDealBookedByUserName { get; set; }
    public string? FXCoverDealExecutionId { get; set; }
    public string? FXCoverDealTradeId { get; set; }
    public string? FXDealBaseEquivalentTextWithCommasAndCurrencyCode { get; set; }
    public decimal? FXSellAmountEquivalent { get; set; }
    public string? FXSellAmountEquivalentTextBare { get; set; }
    public string? FXSellAmountEquivalentTextWithCurrencyCode { get; set; }
    public string? FXSellAmountEquivalentTextBareSWIFT { get; set; }
    public string? FXSellAmountEquivalentCurrencyCode { get; set; }
    public int? FXSellAmountEquivalentCurrencyScale { get; set; } 


    // Ordering Customer
    public int OrderingCustomerTypeId { get; set; } = 0;
    public string OrderingCustomerTypeName { get; set; }
    public Guid? OrderingCustomerId { get; set; }
    public string OrderingCustomerName { get; set; }
    public string OrderingCustomerFirstName { get; set; }
    public string OrderingCustomerMiddleName { get; set; }
    public string OrderingCustomerLastName { get; set; }
    public string OrderingCustomerAccountNumber { get; set; }
    public string OrderingCustomerStreetAddress1 { get; set; }
    public string OrderingCustomerStreetAddress2 { get; set; }
    public string OrderingCustomerDepartment { get; set; }
    public string OrderingCustomerSubDepartment { get; set; }
    public string OrderingCustomerStreetName { get; set; }
    public string OrderingCustomerBuildingNumber { get; set; }
    public string OrderingCustomerBuildingName { get; set; }
    public string OrderingCustomerFloor { get; set; }
    public string OrderingCustomerPostBox { get; set; }
    public string OrderingCustomerRoom { get; set; }
    public string OrderingCustomerCity { get; set; }
    public string OrderingCustomerTownLocationName { get; set; }
    public string OrderingCustomerDistrictName { get; set; }
    public string OrderingCustomerStateOrProvince { get; set; }
    public string OrderingCustomerStateOrProvinceText { get; set; }
    public string OrderingCustomerPostalCode { get; set; }
    public string OrderingCustomerCountryCode { get; set; }
    public string OrderingCustomerCountryName { get; set; }
    public string OrderingCustomerAddress1 { get; set; }
    public string OrderingCustomerAddress2 { get; set; }
    public string OrderingCustomerAddress3 { get; set; }
    public string OrderingCustomerPhone { get; set; }
    public string OrderingCustomerEmail { get; set; }
    public string OrderingCustomerBranchCode { get; set; }
    public string OrderingCustomerCompanyRegistrationNumber { get; set; }
    public string OrderingCustomerCompanyRegistrationCountryCode { get; set; }
    public string OrderingCustomerCompanyRegistrationCountryName { get; set; }
    public string OrderingCustomerDateOfBirth { get; set; }
    public string OrderingCustomerCountryOfBirthCode { get; set; }
    public string OrderingCustomerCountryOfBirthName { get; set; }
    public int OrderingCustomerIdentificationTypeId { get; set; } = 0;
    public string OrderingCustomerIdentificationTypeName { get; set; }
    public string OrderingCustomerIdentificationNumber { get; set; }
    public string OrderingCustomerIdentificationCountryCode { get; set; }
    public string OrderingCustomerIdentificationCountryName { get; set; }


    // Ordering Customer Bank
    public string OrderingCustomerBankName { get; set; }
    public string OrderingCustomerBankStreetAddress1 { get; set; }
    public string OrderingCustomerBankStreetAddress2 { get; set; }
    public string OrderingCustomerBankDepartment { get; set; }
    public string OrderingCustomerBankSubDepartment { get; set; }
    public string OrderingCustomerBankStreetName { get; set; }
    public string OrderingCustomerBankBuildingNumber { get; set; }
    public string OrderingCustomerBankBuildingName { get; set; }
    public string OrderingCustomerBankFloor { get; set; }
    public string OrderingCustomerBankPostBox { get; set; }
    public string OrderingCustomerBankRoom { get; set; }
    public string OrderingCustomerBankCity { get; set; }
    public string OrderingCustomerBankTownLocationName { get; set; }
    public string OrderingCustomerBankDistrictName { get; set; }
    public string OrderingCustomerBankStateOrProvince { get; set; }
    public string OrderingCustomerBankStateOrProvinceText { get; set; }
    public string OrderingCustomerBankPostalCode { get; set; }
    public string OrderingCustomerBankCountryCode { get; set; }
    public string OrderingCustomerBankCountryName { get; set; }
    public string OrderingCustomerBankAddress1 { get; set; }
    public string OrderingCustomerBankAddress2 { get; set; }
    public string OrderingCustomerBankAddress3 { get; set; }
    public string OrderingCustomerBankBIC { get; set; }
    public string OrderingCustomerBankNationalCodeType { get; set; }
    public string OrderingCustomerBankNationalCodeTypeName { get; set; }
    public string OrderingCustomerBankNationalCode { get; set; }


    // Initiating Institution
    public bool InitiatingInstitutionSameAsOrderingInstitution { get; set; } = false;
    public string InitiatingInstitutionName { get; set; }
    public string InitiatingInstitutionStreetAddress1 { get; set; }
    public string InitiatingInstitutionStreetAddress2 { get; set; }
    public string InitiatingInstitutionDepartment { get; set; }
    public string InitiatingInstitutionSubDepartment { get; set; }
    public string InitiatingInstitutionStreetName { get; set; }
    public string InitiatingInstitutionBuildingNumber { get; set; }
    public string InitiatingInstitutionBuildingName { get; set; }
    public string InitiatingInstitutionFloor { get; set; }
    public string InitiatingInstitutionPostBox { get; set; }
    public string InitiatingInstitutionRoom { get; set; }
    public string InitiatingInstitutionCity { get; set; }
    public string InitiatingInstitutionTownLocationName { get; set; }
    public string InitiatingInstitutionDistrictName { get; set; }
    public string InitiatingInstitutionStateOrProvince { get; set; }
    public string InitiatingInstitutionStateOrProvinceText { get; set; }
    public string InitiatingInstitutionPostalCode { get; set; }
    public string InitiatingInstitutionCountryCode { get; set; }
    public string InitiatingInstitutionCountryName { get; set; }
    public string InitiatingInstitutionBIC { get; set; }
    public string InitiatingInstitutionNationalCodeType { get; set; }
    public string InitiatingInstitutionNationalCodeTypeName { get; set; }
    public string InitiatingInstitutionNationalCode { get; set; }
    public string InitiatingInstitutionABN { get; set; }
    public string InitiatingInstitutionACN { get; set; }
    public string InitiatingInstitutionARBN { get; set; }


    // Sending Institution
    public bool SendingInstitutionSameAsOrderingInstitution { get; set; } = false;
    public string SendingInstitutionName { get; set; }
    public string SendingInstitutionStreetAddress1 { get; set; }
    public string SendingInstitutionStreetAddress2 { get; set; }
    public string SendingInstitutionDepartment { get; set; }
    public string SendingInstitutionSubDepartment { get; set; }
    public string SendingInstitutionStreetName { get; set; }
    public string SendingInstitutionBuildingNumber { get; set; }
    public string SendingInstitutionBuildingName { get; set; }
    public string SendingInstitutionFloor { get; set; }
    public string SendingInstitutionPostBox { get; set; }
    public string SendingInstitutionRoom { get; set; }
    public string SendingInstitutionCity { get; set; }
    public string SendingInstitutionTownLocationName { get; set; }
    public string SendingInstitutionDistrictName { get; set; }
    public string SendingInstitutionStateOrProvince { get; set; }
    public string SendingInstitutionStateOrProvinceText { get; set; }
    public string SendingInstitutionPostalCode { get; set; }
    public string SendingInstitutionCountryCode { get; set; }
    public string SendingInstitutionCountryName { get; set; }
    public string SendingInstitutionBIC { get; set; }
    public string SendingInstitutionNationalCodeType { get; set; }
    public string SendingInstitutionNationalCodeTypeName { get; set; }
    public string SendingInstitutionNationalCode { get; set; }
    public string SendingInstitutionPhone { get; set; }
    public string SendingInstitutionEmail { get; set; }
    public string SendingInstitutionABN { get; set; }
    public string SendingInstitutionACN { get; set; }
    public string SendingInstitutionARBN { get; set; }
    //public string SendingInstitutionBusinessStructureTypeCode { get; set; }
    //public string SendingInstitutionBusinessStructureTypeName { get; set; }
    public int SendingInstitutionOccupationTypeId { get; set; } = 0;
    public string SendingInstitutionOccupationTypeName { get; set; }
    public string SendingInstitutionOccupationCode { get; set; }
    public string SendingInstitutionOccupationDescription { get; set; }


    // Beneficiary
    public string BeneficiaryName { get; set; }
    public string BeneficiaryFirstName { get; set; }
    public string BeneficiaryMiddleName { get; set; }
    public string BeneficiaryLastName { get; set; }
    public string BeneficiaryStreetAddress1 { get; set; }
    public string BeneficiaryStreetAddress2 { get; set; }
    public string BeneficiaryDepartment { get; set; }
    public string BeneficiarySubDepartment { get; set; }
    public string BeneficiaryStreetName { get; set; }
    public string BeneficiaryBuildingNumber { get; set; }
    public string BeneficiaryBuildingName { get; set; }
    public string BeneficiaryFloor { get; set; }
    public string BeneficiaryPostBox { get; set; }
    public string BeneficiaryRoom { get; set; }
    public string BeneficiaryCity { get; set; }
    public string BeneficiaryTownLocationName { get; set; }
    public string BeneficiaryDistrictName { get; set; }
    public string BeneficiaryStateOrProvince { get; set; }
    public string BeneficiaryStateOrProvinceText { get; set; }
    public string BeneficiaryPostalCode { get; set; }
    public string BeneficiaryCountryCode { get; set; }
    public string BeneficiaryCountryName { get; set; }
    public string BeneficiaryAddress1 { get; set; }
    public string BeneficiaryAddress2 { get; set; }
    public string BeneficiaryAddress3 { get; set; }
    public string BeneficiaryInfoLine1 { get; set; }
    public string BeneficiaryInfoLine2 { get; set; }
    public string BeneficiaryInfoLine3 { get; set; }
    public string BeneficiaryInfoLine4 { get; set; }
    public string BeneficiaryExternalReference { get; set; }
    public string BeneficiaryWKYCId { get; set; }
    public string BeneficiaryCellPhone { get; set; }
    public string BeneficiaryEmail { get; set; }
    public string BeneficiaryABN { get; set; }
    public string BeneficiaryACN { get; set; }
    public string BeneficiaryARBN { get; set; }
    public int BeneficiaryTypeId { get; set; } = 0;
    public string BeneficiaryTypeName { get; set; }
    public string BeneficiaryAccountNumberPrefix { get; set; }
    public string BeneficiaryAccountNumber { get; set; }
    public string BeneficiaryAccountNumberSuffix { get; set; }
    public string BeneficiaryCompanyRegistrationNumber { get; set; }
    public string BeneficiaryCompanyRegistrationCountryCode { get; set; }
    public string BeneficiaryCompanyRegistrationCountryName { get; set; }

    public string BeneficiaryBusinessStructureTypeCode { get; set; }
    public string BeneficiaryBusinessStructureTypeName { get; set; }
    public int BeneficiaryOccupationTypeId { get; set; } = 0;
    public string BeneficiaryOccupationTypeName { get; set; }
    public string BeneficiaryOccupationDescription { get; set; }
    public string BeneficiaryOccupationCode { get; set; }
    public int BeneficiaryIdentificationTypeId { get; set; } = 0;
    public string BeneficiaryIdentificationTypeName { get; set; }
    public string BeneficiaryIdentificationNumber { get; set; }
    public string BeneficiaryIdentificationCountryCode { get; set; }
    public string BeneficiaryIdentificationCountryName { get; set; }
    public string BeneficiaryDateOfBirth { get; set; }
    public string BeneficiaryCountryOfBirthCode { get; set; }
    public string BeneficiaryCountryOfBirthName { get; set; }
    public string BeneficiaryAccountTypeCode { get; set; }
    public string BeneficiaryAccountTypeName { get; set; }
    public string BeneficiaryTaxId { get; set; }


    // Beneficiary Bank
    public string BeneficiaryBankName { get; set; }
    public string BeneficiaryBankStreetAddress1 { get; set; }
    public string BeneficiaryBankStreetAddress2 { get; set; }
    public string BeneficiaryBankDepartment { get; set; }
    public string BeneficiaryBankSubDepartment { get; set; }
    public string BeneficiaryBankStreetName { get; set; }
    public string BeneficiaryBankBuildingNumber { get; set; }
    public string BeneficiaryBankBuildingName { get; set; }
    public string BeneficiaryBankFloor { get; set; }
    public string BeneficiaryBankPostBox { get; set; }
    public string BeneficiaryBankRoom { get; set; }
    public string BeneficiaryBankCity { get; set; }
    public string BeneficiaryBankTownLocationName { get; set; }
    public string BeneficiaryBankDistrictName { get; set; }
    public string BeneficiaryBankStateOrProvince { get; set; }
    public string BeneficiaryBankStateOrProvinceText { get; set; }
    public string BeneficiaryBankPostalCode { get; set; }
    public string BeneficiaryBankCountryCode { get; set; }
    public string BeneficiaryBankCountryName { get; set; }
    public string BeneficiaryBankAddress1 { get; set; }
    public string BeneficiaryBankAddress2 { get; set; }
    public string BeneficiaryBankAddress3 { get; set; }
    public string BeneficiaryBankBIC { get; set; }
    public string BeneficiaryBankNationalCodeType { get; set; }
    public string BeneficiaryBankNationalCodeTypeName { get; set; }
    public string BeneficiaryBankNationalCode { get; set; }
    public string BeneficiaryBankCode { get; set; }
    public string BeneficiaryBranchCode { get; set; }

    // Receiving Institution
    public string ReceivingInstitutionName { get; set; }
    public string ReceivingInstitutionStreetAddress1 { get; set; }
    public string ReceivingInstitutionStreetAddress2 { get; set; }
    public string ReceivingInstitutionDepartment { get; set; }
    public string ReceivingInstitutionSubDepartment { get; set; }
    public string ReceivingInstitutionStreetName { get; set; }
    public string ReceivingInstitutionBuildingNumber { get; set; }
    public string ReceivingInstitutionBuildingName { get; set; }
    public string ReceivingInstitutionFloor { get; set; }
    public string ReceivingInstitutionPostBox { get; set; }
    public string ReceivingInstitutionRoom { get; set; }
    public string ReceivingInstitutionCity { get; set; }
    public string ReceivingInstitutionTownLocationName { get; set; }
    public string ReceivingInstitutionDistrictName { get; set; }
    public string ReceivingInstitutionStateOrProvince { get; set; }
    public string ReceivingInstitutionStateOrProvinceText { get; set; }
    public string ReceivingInstitutionPostalCode { get; set; }
    public string ReceivingInstitutionCountryCode { get; set; }
    public string ReceivingInstitutionCountryName { get; set; }
    public string ReceivingInstitutionBIC { get; set; }
    public string ReceivingInstitutionNationalCodeType { get; set; }
    public string ReceivingInstitutionNationalCodeTypeName { get; set; }
    public string ReceivingInstitutionNationalCode { get; set; }

    // Beneficiary Branch
    public string BeneficiaryBranchName { get; set; }
    public string BeneficiaryBranchId { get; set; }
    public string BeneficiaryBranchStreetAddress1 { get; set; }
    public string BeneficiaryBranchStreetAddress2 { get; set; }
    public string BeneficiaryBranchDepartment { get; set; }
    public string BeneficiaryBranchSubDepartment { get; set; }
    public string BeneficiaryBranchStreetName { get; set; }
    public string BeneficiaryBranchBuildingNumber { get; set; }
    public string BeneficiaryBranchBuildingName { get; set; }
    public string BeneficiaryBranchFloor { get; set; }
    public string BeneficiaryBranchPostBox { get; set; }
    public string BeneficiaryBranchRoom { get; set; }
    public string BeneficiaryBranchCity { get; set; }
    public string BeneficiaryBranchTownLocationName { get; set; }
    public string BeneficiaryBranchDistrictName { get; set; }
    public string BeneficiaryBranchStateOrProvince { get; set; }
    public string BeneficiaryBranchStateOrProvinceText { get; set; }
    public string BeneficiaryBranchPostalCode { get; set; }
    public string BeneficiaryBranchCountryCode { get; set; }
    public string BeneficiaryBranchCountryName { get; set; }
    public string BeneficiaryBranchBIC { get; set; }
    public string BeneficiaryBranchNationalCodeType { get; set; }
    public string BeneficiaryBranchNationalCodeTypeName { get; set; }
    public string BeneficiaryBranchNationalCode { get; set; }

    // Intermediary Bank
    public string IntermediaryBankName { get; set; }
    public string IntermediaryBankStreetAddress1 { get; set; }
    public string IntermediaryBankStreetAddress2 { get; set; }
    public string IntermediaryBankDepartment { get; set; }
    public string IntermediaryBankSubDepartment { get; set; }
    public string IntermediaryBankStreetName { get; set; }
    public string IntermediaryBankBuildingNumber { get; set; }
    public string IntermediaryBankBuildingName { get; set; }
    public string IntermediaryBankFloor { get; set; }
    public string IntermediaryBankPostBox { get; set; }
    public string IntermediaryBankRoom { get; set; }
    public string IntermediaryBankCity { get; set; }
    public string IntermediaryBankTownLocationName { get; set; }
    public string IntermediaryBankDistrictName { get; set; }
    public string IntermediaryBankStateOrProvince { get; set; }
    public string IntermediaryBankStateOrProvinceText { get; set; }
    public string IntermediaryBankPostalCode { get; set; }
    public string IntermediaryBankCountryCode { get; set; }
    public string IntermediaryBankCountryName { get; set; }
    public string IntermediaryBankAddress1 { get; set; }
    public string IntermediaryBankAddress2 { get; set; }
    public string IntermediaryBankAddress3 { get; set; }
    public string IntermediaryBankBIC { get; set; }
    public string IntermediaryBankNationalCodeType { get; set; }
    public string IntermediaryBankNationalCodeTypeName { get; set; }
    public string IntermediaryBankNationalCode { get; set; }

    public string ReasonForPaymentCode { get; set; }
    public string ReasonForPaymentCodeName { get; set; }
    public string ReasonForPayment { get; set; }
    public string SenderToReceiverInfo1 { get; set; }
    public string SenderToReceiverInfo2 { get; set; }
    public string SenderToReceiverInfo3 { get; set; }
    public string SenderToReceiverInfo4 { get; set; }
    public string SenderToReceiverInfo5 { get; set; }
    public string SenderToReceiverInfo6 { get; set; }
}