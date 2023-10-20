namespace SearchApi
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic()
		{
			return new SearchLogic(new Database());
		}

		public static ISearchLogic CreateNormalizedSearchLogic()
		{
			return new NormalizedSearchLogic(new NormalizedWordDictionary());
		}
	}
}
