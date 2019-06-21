using System;
using System.Linq;

namespace Rhythmic.Screens.Select
{
    public class FilterCriteria
    {
        public SortMode Sort;

        public string[] SearchTerms = Array.Empty<string>();

        private string searchText;

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                SearchTerms = searchText.Split(',', ' ', '!').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            }
        }
    }
}
