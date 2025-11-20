using GPWebApi.DTO;
using Wallet.Interfaces;
using Wallet.Models;

namespace Wallet.Helper
{
	public class UserMapperHelper : IUserMapperHelper
	{
		private readonly IConfiguration _configuration;

		public UserMapperHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public CustomerUserCreateRequest MapSignUpToCustomerUserCreateRequest(Guid customerId, string userName, string password, string emailAddress, string firstName, string lastName, string wkycId,
			bool isApproved, bool userMustChangePassword, bool emailPasswordToUser)
		{
			return new CustomerUserCreateRequest
			{
				CustomerId = customerId,
				UserName = userName,
				Password = password,
				EmailAddress = emailAddress,
				FirstName = firstName,
				LastName = lastName,
				IsApproved = isApproved,
				UserMustChangePassword = userMustChangePassword,
				EmailPasswordToUser = emailPasswordToUser,
				WKYCId = wkycId
			};
		}

		public CustomerUserCreateRequest MapSignUpToCustomerUserCreateRequest(Guid customerId, string userName, string password, string emailAddress, string firstName, string lastName, string wkycId)
		{
			return MapSignUpToCustomerUserCreateRequest(customerId, userName, password, emailAddress, firstName, lastName, wkycId, true, false, false);
		}

		public UserAccessRightTemplateApplyRequest MapUserAccessRightTemplateApplyRequest(Guid userId, Guid accessRightTemplateId)
		{
			return new UserAccessRightTemplateApplyRequest
			{
				UserId = userId,
				AccessRightTemplateId = accessRightTemplateId
			};
		}

		public UserAccessRightTemplateApplyRequest MapUserAccessRightTemplateApplyRequest(Guid userId)
		{
			var accessRightTemplateId = new Guid(_configuration.GetValue<string>("Win:Beta:AccessRightTemplateId") ?? string.Empty);
			return MapUserAccessRightTemplateApplyRequest(userId, accessRightTemplateId);
		}

		public UserAccessRightTemplateLinkRequest MapUserAccessRightTemplateLinkRequest(Guid userId, Guid accessRightTemplateId)
		{
			return new UserAccessRightTemplateLinkRequest
			{
				UserId = userId,
				AccessRightTemplateId = accessRightTemplateId
			};
		}

		public UserAccessRightTemplateLinkRequest MapUserAccessRightTemplateLinkRequest(Guid userId)
		{
			var accessRightTemplateId = new Guid(_configuration.GetValue<string>("Win:Beta:AccessRightTemplateId") ?? string.Empty);
			return MapUserAccessRightTemplateLinkRequest(userId, accessRightTemplateId);
		}

		public CustomerUserCreateRequest MapUserSignUpModelToCustomerUserCreateRequest(UserSignUpModel model, Guid customerId, string wkycId, bool isApproved, bool userMustChangePassword, bool emailPasswordToUser)
		{
			return new CustomerUserCreateRequest
			{
				CustomerId = customerId,
				UserName = model.UserName,
				Password = model.Password,
				EmailAddress = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				IsApproved = isApproved,
				UserMustChangePassword = userMustChangePassword,
				EmailPasswordToUser = emailPasswordToUser,
				WKYCId = wkycId
			};
		}

		public CustomerUserCreateRequest MapUserSignUpModelToCustomerUserCreateRequest(UserSignUpModel model, Guid customerId)
		{
			return MapUserSignUpModelToCustomerUserCreateRequest(model, customerId, string.Empty, true, false, false);
		}
	}
}
