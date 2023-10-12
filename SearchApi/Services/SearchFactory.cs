using SearchApi.Database;

namespace SearchApi.Services
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic()
		{
			return new SearchLogic(new PostgresDatabase());
		}

		public static ISearchLogic CreateCaseInsensitiveSearchLogic()
		{
			return new CaseInsensitiveSearchLogic(new CaseInsensitivePostgresDatabase());
		}
	}
}
