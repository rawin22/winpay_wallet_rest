using System.Text.RegularExpressions;
using Wallet.Interfaces;


namespace Wallet.Helper;

public class TextHelper : ITextHelper
{
	private readonly ILogger<VLinkServiceHelper> _logger;

	public TextHelper(
		ILogger<VLinkServiceHelper> logger
		)
	{
		_logger = logger;
	}

	public string ConvertUrlsToLinks(string text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return text;
		}

		// Regular expression to match URLs with or without the scheme
		string urlPattern = @"((http[s]?://)|(www\.))([^\s]+)";
		// MatchEvaluator to process each match
		MatchEvaluator evaluator = new MatchEvaluator((match) =>
		{
			string url = match.Value;
			if (!url.StartsWith("http"))
			{
				url = "https://" + url;
			}
			return $"<a href=\"{url}\" target=\"_blank\">{url}</a>";
		});

		return Regex.Replace(text, urlPattern, evaluator);
	}

	public string GetTextWithLineBreaks(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}

		// Encode the text to avoid XSS and then replace newlines with <br> tags
		//var encodedText = HtmlEncoder.Default.Encode(text);
		return text.Replace("\n", "<br>").Replace("\r", "");
	}
}
