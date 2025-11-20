
using GPWebApi.DTO;
using Newtonsoft.Json;
using Wallet.Interfaces;
using Wallet.Models;

namespace Wallet.Services;

public class ItemNoteMapperService : IItemNoteMapperService
{
	public ItemNoteCreateRequest MapNewNoteDataToCreateRequest(Guid itemId, int itemType, string noteText, bool isViewableByCustomer)
	{
		return new ItemNoteCreateRequest
		{
			ItemId = itemId,
			ItemType = (ItemType)itemType,
			NoteText = noteText,
			ViewableByCustomer = isViewableByCustomer,
		};
	}

	public ItemNoteDeleteRequest MapNoteDataToDeleteRequest(Guid itemId, Guid itemNoteId)
	{
		return new ItemNoteDeleteRequest
		{
			ItemId = itemId,
			ItemNoteId = itemNoteId,
		};
	}
}
