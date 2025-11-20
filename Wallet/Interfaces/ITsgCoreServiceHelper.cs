using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces
{
    public interface ITsgCoreServiceHelper
    {
        Task<AuthenticateResponse> AuthenticateAsync(string username, string password);
        Task<bool> RefreshAccessTokenAndRebuildCookie(Boolean doPageRefresh);
        Task SetAuthenticationCookie(Tokens tokens, UserSettingsData settings);
        Task<WkycFileAttachemenmtListResponse> FileAttachmentGetFileListForObjectAsync(string customerId);
        Task<WkycFileAttachmentGetDataResponse> FileAttachmentGetDataAsync(string fileAttachmentId);
        Task<WkycFileAttachmentGetDataResponse> FileAttachmentAddAsync(FileAttachmentAddFileRequest request);
        Task<WkycFileAttachmentUpdateFileInfoResponse> FileAttachmentUpdateAsync(FileAttachmentUpdateFileInfoRequest request);
        Task<WkycFileAttachmentDeleteFileResponse> FileAttachmentDeleteAsync(Guid fileAttachmentId);
        Task<VerifiedLinkSearchResponse> VerifiedLinkSearchAsync(string verifiedLinkTypeId, string reference, int pageIndex, int pageSize, int amountMin);
        Task<VerifiedLinkData> GetVerifiedLinkDetailsAsync(string verifiedLinkId, string token);
        Task<VerifiedLinkData> GetVerifiedLinkDetailsAsync(string verifiedLinkId);
        Task<WkycVerifiedLinkUpdateResponse> UpdateVerifiedLinkAsync(VerifiedLinkUpdateRequest request);
        Task<WkycVerifiedLinkCreateResponse> CreateVerifiedLinkAsync(VerifiedLinkCreateRequest request);
        Task<WkycVerifiedLinkDeleteResponse> DeleteVerifiedLinkAsync(VerifiedLinkDeleteRequest request);

        #region Customer
        Task<WkycGetCustomerResponse> CustomerGetSingleAsync(string? customerId, string token);
        Task<WkycGetCustomerResponse> CustomerGetSingleAsync(string? customerId);
        Task<WkycUpdateCustomerResponse> CustomerUpdateAsync(CustomerUpdateRequest request, string token);
        Task<WkycUpdateCustomerResponse> CustomerUpdateAsync(CustomerUpdateRequest request);
        Task<WkycCustomerCreateResponse> CreateCustomerAsync(CustomerCreateRequest request, string token);
        Task<WkycCustomerCreateResponse> CreateCustomerAsync(CustomerCreateRequest request);
        Task<WkycCustomerCreateFromTemplateResponse> CreateCustomerFromTemplateAsync(CustomerCreateFromTemplateRequest request, string token);
        Task<WkycCustomerCreateFromTemplateResponse> CreateCustomerFromTemplateAsync(CustomerCreateFromTemplateRequest request);
        Task<WkycCustomerAccountAliasListResponse> GetCustomerAccountAliasListAsync(string customerId, string token);
        Task<WkycCustomerAccountAliasListResponse> GetCustomerAccountAliasListAsync(string customerId);
        Task<WkycCustomerAccountAliasCreateResponse> CreateCustomerAccountAliasAsync(CustomerAccountAliasCreateRequest request, string token);
        Task<WkycCustomerAccountAliasCreateResponse> CreateCustomerAccountAliasAsync(CustomerAccountAliasCreateRequest request);
        Task<WkycCustomerAccountAliasDeleteResponse> DeleteCustomerAccountAliasAsync(string customerId, string alias, string token);
        Task<WkycCustomerAccountAliasDeleteResponse> DeleteCustomerAccountAliasAsync(string customerId, string alias);
        Task<WkycCustomerAccountAliasSetDefaultResponse> SetDefaultCustomerAccountAliasAsync(string customerId, string alias, string token);
        Task<WkycCustomerAccountAliasSetDefaultResponse> SetDefaultCustomerAccountAliasAsync(string customerId, string alias);
        #endregion

        #region User
        Task<WkycUserPasswordChangeResponse> ChangePasswordAsync(UserPasswordChangeRequest request);
        Task<WkycUserDoesUsernameExistResponse> IsUsernameExistAsync(string username, string token);
        Task<WkycUserDoesUsernameExistResponse> IsUsernameExistAsync(string username);
        Task<WkycCustomerUserCreateResponse> CreateCustomerUserAsync(CustomerUserCreateRequest request, string token);
        Task<WkycCustomerUserCreateResponse> CreateCustomerUserAsync(CustomerUserCreateRequest request);
        Task<WkycUserAccessRightTemplateLinkResponse> LinkUserAccessRightTemplateAsync(UserAccessRightTemplateLinkRequest request, string token);
        Task<WkycUserAccessRightTemplateLinkResponse> LinkUserAccessRightTemplateAsync(UserAccessRightTemplateLinkRequest request);
        Task<WkycUserAccessRightTemplateApplyResponse> ApplyUserAccessRightTemplateAsync(UserAccessRightTemplateApplyRequest request, string token);
        Task<WkycUserAccessRightTemplateApplyResponse> ApplyUserAccessRightTemplateAsync(UserAccessRightTemplateApplyRequest request);
        #endregion

        Task<WkycCountryListResponse> GetCountryListAsync(string token);
        Task<WkycCountryListResponse> GetCountryListAsync();
        Task<WkycPaymentCurrencyListResponse> GetPaymentCurrencyListAsync();
        Task<WkycCountryIdentificationTypeListResponse> GetCountryIdentificationTypeListAsync(string token, string countryCode);
        Task<WkycCountryIdentificationTypeListResponse> GetCountryIdentificationTypeListAsync(string countryCode);
        Task<WkycUserAccountAliasListResponse> GetUserAccountAliasListAsync(string userId, string token);
        Task<WkycUserAccountAliasListResponse> GetUserAccountAliasListAsync(string userId);

        #region Verify
        Task<WkycVerifyResponse> VerifyAsync(VerifyGetRequest request, string token);
        Task<WkycVerifyResponse> VerifyAsync(VerifyGetRequest request);
        Task<WkycVerifyResponse> VerifyAsync(Guid VLinkId, string token);
        Task<WkycVerifyResponse> VerifyAsync(Guid VLinkId);
        Task<WkycVerifyMultipleResponse> VerifyAsync(VerifyGetMultipleRequest request);
        Task<WkycVerifyResponse> PublicVerifyAsync(VerifyGetRequest request);
        Task<WkycVerifyResponse> PublicVerifyAsync(Guid VLinkId);
        #endregion

        #region Item Note
        Task<WkycItemNoteGetAllForItemResponse> GetAllItemNotesForItemAsync(Guid itemId, string token);
        Task<WkycItemNoteGetAllForItemResponse> GetAllItemNotesForItemAsync(Guid itemId);
        Task<WkycItemNoteGetResponse> GetItemNoteAsync(Guid itemId, Guid itemNoteId, string token);
        Task<WkycItemNoteGetResponse> GetItemNoteAsync(Guid itemId, Guid itemNoteId);
        Task<WkycItemNoteUpdateResponse> UpdateItemNoteAsync(ItemNoteUpdateRequest request, string token);
        Task<WkycItemNoteUpdateResponse> UpdateItemNoteAsync(ItemNoteUpdateRequest request);
        Task<WkycItemNoteCreateResponse> CreateItemNoteAsync(ItemNoteCreateRequest request, string token);
        Task<WkycItemNoteCreateResponse> CreateItemNoteAsync(ItemNoteCreateRequest request);
        Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(ItemNoteDeleteRequest request, string token);
        Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(ItemNoteDeleteRequest request);
        Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(Guid itemId, Guid itemNoteId, string token);
        Task<WkycItemNoteDeleteResponse> DeleteItemNoteAsync(Guid itemId, Guid itemNoteId);
        #endregion
    }
}

