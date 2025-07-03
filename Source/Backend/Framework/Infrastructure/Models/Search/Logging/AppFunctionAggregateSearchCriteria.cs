using Framework.Infrastructure.Models.Search;

namespace Framework.Infrastructure.Models.Search.LogSearch
{
    public class AppFunctionAggregateSearchCriteria : BaseSearchCriteria
    {
        public string AppFunction { get; set; }

        public float? LesserThanMedianTimeElapsed { get; set; }

        public float? GreaterThanMedianTimeElapsed { get; set; }

        public float? LesserThanMeanTimeElapsed { get; set; }

        public float? GreaterThanMeanTimeElapsed { get; set; }

        public float? LesserThanAppFunctionCount { get; set; }

        public float? GreaterThanAppFunctionCount { get; set; }
    }
}