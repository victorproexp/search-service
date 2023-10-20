namespace SearchApi
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic()
		{
			IDatabase database = new Database();
			IWordManager wordManager = new WordManager<int>(database.GetAllWords());

			ISearchLogic searchLogic = new SearchLogic(wordManager);

			return searchLogic;
		}

		public static ISearchLogic CreateNormalizedSearchLogic()
		{
			NormalizedWordDictionary dictionary = new();
			IWordManager wordManager = new WordManager<List<int>>(dictionary.GetAllWords());

			ISearchLogic searchLogic = new SearchLogic(wordManager);

			return searchLogic;
		}
	}
}
