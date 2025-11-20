using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces
{
	public interface IUserMapperHelper
	{
		CustomerUserCreateRequest MapSignUpToCustomerUserCreateRequest(Guid customerId, string userName, string password, string emailAddress, string firstName, string lastName, string wkycId,
			bool isApproved, bool userMustChangePassword, bool emailPasswordToUser);
		CustomerUserCreateRequest MapSignUpToCustomerUserCreateRequest(Guid customerId, string userName, string password, string emailAddress, string firstName, string lastName, string wkycId);
		CustomerUserCreateRequest MapUserSignUpModelToCustomerUserCreateRequest(UserSignUpModel model, Guid customerId, string wkycId, bool isApproved, bool userMustChangePassword, bool emailPasswordToUser);
		CustomerUserCreateRequest MapUserSignUpModelToCustomerUserCreateRequest(UserSignUpModel model, Guid customerId);
		UserAccessRightTemplateApplyRequest MapUserAccessRightTemplateApplyRequest(Guid userId, Guid accessRightTemplateId);
		UserAccessRightTemplateApplyRequest MapUserAccessRightTemplateApplyRequest(Guid userId);
		UserAccessRightTemplateLinkRequest MapUserAccessRightTemplateLinkRequest(Guid userId, Guid accessRightTemplateId);
		UserAccessRightTemplateLinkRequest MapUserAccessRightTemplateLinkRequest(Guid userId);
	}
}
