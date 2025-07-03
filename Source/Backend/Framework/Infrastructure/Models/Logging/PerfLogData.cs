#nullable enable

using System;
using CsvHelper.Configuration.Attributes;

namespace Framework.Infrastructure.Models
{
    public class PerfLogData
    {
        [Index(0)]
        public DateTime? LogDate { get; set; }

        [Index(1)]
        public string? ActivityId { get; set; }

        [Index(2)]
        public string? MachineName { get; set; }

        [Index(3)]
        public string? UserIdentity { get; set; }

        [Index(4)]
        public string? AppName { get; set; }

        [Index(5)]
        public string? AppModule { get; set; }

        [Index(6)]
        public string? AppFunction { get; set; }

        [Index(7)]
        public string? HttpVerb { get; set; }

        [Index(8)]
        public string? StatusCode { get; set; }

        [Index(9)]
        public string? StartTime { get; set; }

        [Index(10)]
        public string? EndTime { get; set; }

        [Index(11)]
        public float? ElapsedTimeInMs { get; set; }

        [Index(12)]
        public string? IPAddress { get; set; }

        public float? ElapsedTimeInSeconds { get; set; }
    }
}
#nullable disable