internal interface ILanguageService
{
	Task LoadLanguagesAsync();
	Task<Dictionary<string, string>> LoadTranslationsAsync(string locale);
}