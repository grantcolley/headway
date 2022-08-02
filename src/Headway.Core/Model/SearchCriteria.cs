using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class SearchCriteria
    {
        public SearchCriteria(List<SearchCriterion> criteria)
        {
            Clauses = new List<SearchCriterion>(criteria);
        }

        public List<SearchCriterion> Clauses { get; }
    }
}
