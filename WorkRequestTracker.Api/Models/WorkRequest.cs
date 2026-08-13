using System;
using System.Text.Json.Serialization;

namespace WorkRequestTracker.Api.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequestStatus
    {
        New,
        InProgress,
        Blocked,
        Completed
    }

    public class WorkRequest
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Priority Priority { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string? Notes { get; set; }
    }
}