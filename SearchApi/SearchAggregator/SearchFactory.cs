namespace SearchApi
{
	public class SearchFactory
	{
		public static List<ISearchLogic> CreateSearchLogics(IDatabase database)
		{
			var searchLogics = new List<ISearchLogic>();

			var dictionary = database.GetAllWords();
            var normalizedDictionary = NormalizeDictionary(dictionary);

			var wordManager = new WordManager<int>(dictionary);
            var normalizedWordManager = new WordManager<List<int>>(normalizedDictionary);

			var searchLogic = CreateSearchLogic(database, wordManager);
			var normalizedSearchLogic = CreateSearchLogic(database, normalizedWordManager);

			searchLogics.AddRange(new[] { searchLogic, normalizedSearchLogic });

			return searchLogics;
		}

		private static ISearchLogic CreateSearchLogic(IDatabase database, IWordManager wordManager)
		{
			return new SearchLogic(database, wordManager);
		}

		private static Dictionary<string, List<int>> NormalizeDictionary(Dictionary<string, int> originalDict)
		{
			var newDict = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

			foreach (var kvp in originalDict)
			{
				string lowerKey = kvp.Key.ToLower();

				if (newDict.ContainsKey(lowerKey))
				{
					newDict[lowerKey].Add(kvp.Value);
				}
				else
				{
					newDict.Add(lowerKey, new List<int> { kvp.Value });
				}
			}

			return newDict;
		}
	}
}
