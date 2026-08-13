using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkRequestTracker.Api.Data;
using WorkRequestTracker.Api.Dtos;
using WorkRequestTracker.Api.Models;

namespace WorkRequestTracker.Api.Controllers
{
    // Marks this class as an API controller: automatic model validation, JSON binding, etc.
    [ApiController]

    // Base route for all endpoints in this controller: /api/work-requests
    [Route("api/work-requests")]
    public class WorkRequestsController : ControllerBase
    {
        // Instead of an in-memory store, we now inject EF Core's DbContext.
        private readonly AppDbContext _db;

        public WorkRequestsController(AppDbContext db)
        {
            _db = db;
        }

        // GET /api/work-requests?status=New&search=acme&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<object>> GetAll(string? status, string? search, int page = 1, int pageSize = 10)
        {
            // Validate status filter if provided
            RequestStatus? statusFilter = null;
            if (!string.IsNullOrWhiteSpace(status))
            {
                bool parsed = Enum.TryParse<RequestStatus>(status, true, out RequestStatus parsedStatus);
                if (!parsed)
                {
                    return BadRequest(new { error = "Invalid status filter value." });
                }
                statusFilter = parsedStatus;
            }

            // Start building a query against the WorkRequests table
            IQueryable<WorkRequest> query = _db.WorkRequests;

            // Apply status filter if present
            if (statusFilter != null)
            {
                query = query.Where(w => w.Status == statusFilter);
            }

            // Apply search filter if present
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(w =>
                    w.Title.ToLower().Contains(search.ToLower()) ||
                    w.ClientName.ToLower().Contains(search.ToLower()));
            }

            // Count total matching rows (for pagination metadata)
            int totalCount = await query.CountAsync();

            // Calculate how many rows to skip based on page number
            int skip = (page - 1) * pageSize;

            // Fetch one page of results, ordered by CreatedDate descending
            List<WorkRequest> pageItems = await query
                .OrderByDescending(w => w.CreatedDate)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Convert to DTOs for safe API response
            List<WorkRequestResponseDto> dtos = pageItems.Select(MapToDto).ToList();

            // Return paginated response
            return Ok(new
            {
                items = dtos,
                totalCount = totalCount,
                page = page,
                pageSize = pageSize
            });
        }

        // GET /api/work-requests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkRequestResponseDto>> GetById(int id)
        {
            // Query the database for a single record by Id
            WorkRequest? found = await _db.WorkRequests.FirstOrDefaultAsync(w => w.Id == id);

            if (found == null)
            {
                return NotFound(new { error = $"Work request with id {id} was not found." });
            }

            return Ok(MapToDto(found));
        }

        // POST /api/work-requests
        [HttpPost]
        public async Task<ActionResult<WorkRequestResponseDto>> Create([FromBody] CreateWorkRequestDto dto)
        {
            DateTime now = DateTime.UtcNow;

            // Build new entity from DTO
            WorkRequest newRequest = new WorkRequest
            {
                Title = dto.Title,
                ClientName = dto.ClientName,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = dto.Status,
                DueDate = dto.DueDate,
                CreatedDate = now,
                UpdatedDate = now,
                Notes = null
            };

            // Add to EF Core change tracker
            _db.WorkRequests.Add(newRequest);

            // Commit INSERT to SQL Server
            await _db.SaveChangesAsync();

            // Return 201 Created with Location header
            return CreatedAtAction(nameof(GetById), new { id = newRequest.Id }, MapToDto(newRequest));
        }

        // PATCH /api/work-requests/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<WorkRequestResponseDto>> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
        {
            // Find record by Id
            WorkRequest? found = await _db.WorkRequests.FirstOrDefaultAsync(w => w.Id == id);

            if (found == null)
            {
                return NotFound(new { error = $"Work request with id {id} was not found." });
            }

            // Update fields
            found.Status = dto.Status;
            found.UpdatedDate = DateTime.UtcNow;

            // Commit UPDATE to SQL Server
            await _db.SaveChangesAsync();

            return Ok(MapToDto(found));
        }

        // POST /api/work-requests/{id}/notes
        [HttpPost("{id}/notes")]
        public async Task<ActionResult<WorkRequestResponseDto>> AddNote(int id, [FromBody] AddNoteDto dto)
        {
            // Find record by Id
            WorkRequest? found = await _db.WorkRequests.FirstOrDefaultAsync(w => w.Id == id);

            if (found == null)
            {
                return NotFound(new { error = $"Work request with id {id} was not found." });
            }

            // Update notes
            found.Notes = dto.Note;
            found.UpdatedDate = DateTime.UtcNow;

            // Commit UPDATE to SQL Server
            await _db.SaveChangesAsync();

            return Ok(MapToDto(found));
        }

        // Helper: convert entity to DTO for API response
        private WorkRequestResponseDto MapToDto(WorkRequest w)
        {
            return new WorkRequestResponseDto
            {
                Id = w.Id,
                Title = w.Title,
                ClientName = w.ClientName,
                Description = w.Description,
                Priority = w.Priority,
                Status = w.Status,
                DueDate = w.DueDate,
                CreatedDate = w.CreatedDate,
                UpdatedDate = w.UpdatedDate,
                Notes = w.Notes
            };
        }
    }
}
