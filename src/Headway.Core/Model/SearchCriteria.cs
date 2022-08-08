using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            Clauses = new List<SearchCriterion>();
        }

        public string SourceConfig { get; set; }
        public List<SearchCriterion> Clauses { get; set; }
    }
}
