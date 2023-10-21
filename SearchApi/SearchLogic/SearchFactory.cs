namespace SearchApi
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic(IDatabase database)
		{
			IWordManager wordManager = new WordManager<int>(database.GetAllWords());
			ISearchLogic searchLogic = new SearchLogic(database, wordManager);
			return searchLogic;
		}

		public static ISearchLogic CreateNormalizedSearchLogic(IDatabase database)
		{
			IWordManager wordManager = new WordManager<List<int>>(database.GetAllWordsNormalized());
			ISearchLogic searchLogic = new SearchLogic(database, wordManager);
			return searchLogic;
		}
	}
}
