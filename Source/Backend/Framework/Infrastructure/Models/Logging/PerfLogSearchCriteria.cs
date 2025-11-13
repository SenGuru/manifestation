using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.Models.Search.LogSearch
{
    public class PerfLogSearchCriteria : DateIntervalSearchCriteria
    {
        public DateTime? LogDate { get; set; }

        public List<string> ActivityIds { get; set; }

        public string MachineName { get; set; }

        public string UserIdentity { get; set; }

        public string AppName { get; set; }

        public string AppModule { get; set; }

        public List<string> AppFunctions { get; set; }

        public string HttpVerb { get; set; }

        public string StatusCode { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public float? ElapsedTimeMs { get; set; }

        public float? LesserThanMS { get; set; }

        public float? GreaterThanMS { get; set; }

        public float? EqualToMs { get; set; }

        public string IPAddress { get; set; }
    }
}