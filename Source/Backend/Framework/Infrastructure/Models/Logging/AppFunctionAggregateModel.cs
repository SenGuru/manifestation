namespace Framework.Infrastructure.Models
{
    public class AppFunctionAggregateModel
    {
        public string AppFunction { get; set; }

        public float? MedianElapsedTimeSec { get; set; }

        public float? MeanElapsedTimeSec { get; set; }

        public int AppFunctionCount { get; set; }
    }
}