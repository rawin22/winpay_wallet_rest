using GPWebApi.DTO;
using Wallet.Models;

namespace Wallet.Interfaces;

public interface IItemNoteMapperService
{
	ItemNoteCreateRequest MapNewNoteDataToCreateRequest(Guid itemId, int itemType, string noteText, bool isViewableByCustomer);
	ItemNoteDeleteRequest MapNoteDataToDeleteRequest(Guid itemId, Guid itemNoteId);
}
