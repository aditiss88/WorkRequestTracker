using System;
using System.ComponentModel.DataAnnotations;
using WorkRequestTracker.Api.Models;

namespace WorkRequestTracker.Api.Dtos
{
    // Used when creating a new work request
    public class CreateWorkRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Client name is required.")]
        public string ClientName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public Priority Priority { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public RequestStatus Status { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }
    }

    // Used for the PATCH status-update endpoint
    public class UpdateStatusDto
    {
        [Required(ErrorMessage = "Status is required.")]
        public RequestStatus Status { get; set; }
    }

    // Used for the add-note endpoint
    public class AddNoteDto
    {
        [Required(ErrorMessage = "Note text is required.")]
        public string Note { get; set; } = string.Empty;
    }

    // Used when returning a work request to the client
    public class WorkRequestResponseDto
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