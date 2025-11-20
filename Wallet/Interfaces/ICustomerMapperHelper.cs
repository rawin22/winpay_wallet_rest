using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces
{
	public interface ICustomerMapperHelper
	{
		CustomerUpdateRequest MapCustomerDataToUpdateRequest(CustomerGetData data);
		CustomerCreateFromTemplateRequest MapSignUpDataToCustomerCreateFromTemplateRequest(string firstName, string lastName, string referredByName, string branchId, string referredByPlatform);
		CustomerCreateFromTemplateRequest MapSignUpDataToCustomerCreateFromTemplateRequest(string firstName, string lastName, string email, string cellphone, string referredByName, string branchId, string referredByPlatform);
		CustomerCreateFromTemplateRequest MapSignUpDataToCustomerCreateFromTemplateRequest(string firstName, string lastName, string email, string cellphone, string referredByName, string branchId, string notaryNodeCountryCode, string referredByPlatform);
		CustomerCreateFromTemplateRequest MapUserSignUpModelToCustomerCreateFromTemplateRequest(UserSignUpModel model);
		CustomerAccountAliasCreateRequest MapNewCustomerAccountAliasToCustomerAccountAliasCreateRequest(NewCustomerAccountAlias model);
	}
}
