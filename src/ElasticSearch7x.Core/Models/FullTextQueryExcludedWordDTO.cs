using System.Collections.Generic;

namespace ElasticSearch7x.Core.Models
{
    public class FullTextQueryExcludedWordDTO
    {
        public FullTextQueryExcludedWordDTO()
        {
            StartsWithKeywordList = new List<string>();
            ExactMatchKeywordList = new List<string>();
        }

        public List<string> StartsWithKeywordList { get; set; }
        public List<string> ExactMatchKeywordList { get; set; }
    }
}