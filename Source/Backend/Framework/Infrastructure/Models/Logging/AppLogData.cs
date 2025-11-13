#nullable enable

using System;
using CsvHelper.Configuration.Attributes;

namespace Framework.Infrastructure.Models
{
    public class AppLogData
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
        public string? Severity { get; set; }

        [Index(6)]
        public string? ProcessId { get; set; }

        [Index(7)]
        public string? ThreadId { get; set; }

        [Index(8)]
        public string? CurrentFunction { get; set; }

        [Index(9)]
        public string? CurrentSourceFileName { get; set; }

        [Index(10)]
        public string? CurrentSourceLineNumber { get; set; }

        [Index(11)]
        public float? ElaspedTime { get; set; }

        [Index(12)]
        public string? Result { get; set; }

        [Index(13)]
        public string? Message { get; set; }

        [Index(12)]
        public string? IPAddress { get; set; }
    }
}
#nullable disable