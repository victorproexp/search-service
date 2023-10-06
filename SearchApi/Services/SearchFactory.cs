using System;
namespace ConsoleSearch
{
	public class SearchFactory
	{
		public static ISearchLogic CreateSearchLogic()
		{
			return new SearchLogic();
		}
	}
}
