using System;
using System.Collections.Generic;

namespace Framework.Infrastructure.Models.Search.LogSearch
{
    public class LogSearchCriteria : DateIntervalSearchCriteria
    {
        public List<string> ActivityIds { get; set; }

        public string MachineName { get; set; }

        public string UserIdentity { get; set; }

        public string AppName { get; set; }

        public List<string> SeverityLevels { get; set; }

        public List<string> ProcessIds { get; set; }

        public List<string> ThreadIds { get; set; }

        public List<string> CurrentFunctions { get; set; }

        public string CurrentSourceLine { get; set; }

        public string Result { get; set; }

        public string Message { get; set; }

        public string IPAddress { get; set; }

        public float? ElaspedTime { get; set; }

        public DateTime? LogDate { get; set; }
    }
}