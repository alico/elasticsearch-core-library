using ElasticSearch7x.Core.Models;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearch7x.Core.Queries
{
    public class FullTextQuery : ElasticSearchQueryBase
    {
        private readonly Fields _fields;
        private readonly string _keyword;
        private readonly FullTextQueryExcludedWordDTO _excluded;
        private readonly double _fuzziness = 0;

        public FullTextQuery(Fields fields, string keyword, FullTextQueryExcludedWordDTO excluded, bool fuzzySearchEnable)
        {
            _fields = fields;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                _keyword = keyword.ToLowerInvariant();
            }

            _excluded = excluded;

            if (fuzzySearchEnable)
            {
                _fuzziness = 1;
            }
        }

        protected override QueryContainer CreateQuery()
        {
            var should = new List<QueryContainer>();

            if (!string.IsNullOrWhiteSpace(_keyword))
            {
                var keywordLower = _keyword.ToLowerInvariant();

                should.Add(new MultiMatchQuery
                {
                    Boost = 2.0d,
                    TieBreaker = 1.7d,
                    Type = TextQueryType.PhrasePrefix,
                    Fields = _fields,
                    Query = string.Join(" ", _keyword.Split(' ').ToList()).Trim(),
                    Slop = 5
                });

                var keywordCleanedList = keywordLower.Split(' ').ToList();

                if (keywordCleanedList.Count > 1)
                {
                    if (_excluded != null)
                    {
                        keywordCleanedList.RemoveAll(a => _excluded.StartsWithKeywordList.Any(a.StartsWith));
                        keywordCleanedList.RemoveAll(a => _excluded.ExactMatchKeywordList.Any(a.Contains));
                    }
                }

                if (keywordCleanedList.Count > 0)
                {
                    var keywordList = keywordLower.Split(' ').ToList();

                    var query = string.Join(" ", keywordList).Trim();
                    var innerMust = new List<QueryContainer>
                    {
                        new MultiMatchQuery
                        {
                            Boost = 50,
                            Type = TextQueryType.BestFields,
                            Query = query,
                            Fields = _fields,
                            Fuzziness = Fuzziness.Ratio(_fuzziness)
                        }
                    };

                    var innerShould = new List<QueryContainer>();

                    var neededWordList = keywordList.Where(word => keywordCleanedList.Contains(word)).ToList();

                    foreach (var kw in neededWordList)
                    {
                        innerShould.Add(new MultiMatchQuery
                        {
                            Boost = 0.1d,
                            TieBreaker = 1.7d,
                            Type = TextQueryType.PhrasePrefix,
                            Fields = _fields,
                            Query = kw,
                            Slop = 5
                        });
                    }

                    innerMust.Add(new BoolQuery { Should = innerShould });

                    should.Add(new BoolQuery { Must = innerMust });
                }
                else
                {
                    var query = string.Join(" ", keywordLower.Split(' ').ToList()).Trim();
                    should.Add(new MultiMatchQuery
                    {
                        Boost = 5,
                        Type = TextQueryType.BestFields,
                        Query = query,
                        Fields = _fields,
                        Fuzziness = Fuzziness.Ratio(_fuzziness)
                    });
                }
            }

            var outerMust = new List<QueryContainer>
            {
                new BoolQuery {Should = should}
            };

            return new BoolQuery { Must = outerMust };
        }
    }
}