using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces;

public interface IVerifyMapperService
{
	VerifyGetRequest MapVerifyViewModelToGetRequest(VerifyModel model);
}
