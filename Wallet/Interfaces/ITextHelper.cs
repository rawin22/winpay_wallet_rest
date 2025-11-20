namespace Wallet.Interfaces;

public interface ITextHelper
{
	string ConvertUrlsToLinks(string text);
	string GetTextWithLineBreaks(string text);
}