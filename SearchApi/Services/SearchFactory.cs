namespace SearchApi.Services
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic()
		{
			return new SearchLogic();
		}

		public static ISearchLogic CreateCaseInsensitiveSearchLogic()
		{
			return new CaseInsensitiveSearchLogic();
		}
	}
}
