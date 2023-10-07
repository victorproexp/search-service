namespace SearchApi.Services
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic()
		{
			return new SearchLogic();
		}
	}
}
