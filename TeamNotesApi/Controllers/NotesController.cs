using Microsoft.AspNetCore.Mvc;
using TeamNotesApi.Models;
using TeamNotesApi.Services;

namespace TeamNotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteResponseDto>>> GetNotes([FromHeader(Name = "X-Team-Id")] int teamId)
        {
            if (teamId <= 0) return BadRequest("Valid X-Team-Id header is required.");
            var notes = await _noteService.GetTeamNotesAsync(teamId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteResponseDto>> GetNoteById(int id, [FromHeader(Name = "X-Team-Id")] int teamId)
        {
            if (teamId <= 0) return BadRequest("Valid X-Team-Id header is required.");
            var note = await _noteService.GetNoteByIdAsync(teamId, id);
            if (note == null) return NotFound($"Note with ID {id} not found within your team context.");
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<NoteResponseDto>> CreateNote(
            [FromHeader(Name = "X-Team-Id")] int teamId,
            [FromHeader(Name = "X-User-Id")] int userId,
            [FromBody] CreateNoteDto dto)
        {
            if (teamId <= 0 || userId <= 0) return BadRequest("Valid X-Team-Id and X-User-Id headers are required.");
            if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Note title cannot be blank.");

            var createdNote = await _noteService.CreateNoteAsync(teamId, userId, dto);
            return CreatedAtAction(nameof(GetNoteById), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NoteResponseDto>> UpdateNote(
            int id,
            [FromHeader(Name = "X-Team-Id")] int teamId,
            [FromBody] UpdateNoteDto dto)
        {
            if (teamId <= 0) return BadRequest("Valid X-Team-Id header is required.");
            if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Note title cannot be blank.");

            var updatedNote = await _noteService.UpdateNoteAsync(teamId, id, dto);
            if (updatedNote == null) return NotFound($"Note with ID {id} not found within your team context.");

            return Ok(updatedNote);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id, [FromHeader(Name = "X-Team-Id")] int teamId)
        {
            if (teamId <= 0) return BadRequest("Valid X-Team-Id header is required.");
            var success = await _noteService.DeleteNoteAsync(teamId, id);
            if (!success) return NotFound($"Note with ID {id} not found within your team context.");

            return NoContent();
        }
    }
}