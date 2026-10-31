using Microsoft.EntityFrameworkCore;
using TeamNotesApi.Data;
using TeamNotesApi.Models;

namespace TeamNotesApi.Services
{
    public class NoteService : INoteService
    {
        private readonly NotesDbContext _context;

        public NoteService(NotesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NoteResponseDto>> GetTeamNotesAsync(int teamId)
        {
            return await _context.Notes
                .Where(n => n.TeamId == teamId && !n.IsDeleted)
                .Include(n => n.CreatorUser)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => MapToDto(n))
                .ToListAsync();
        }

        public async Task<NoteResponseDto?> GetNoteByIdAsync(int teamId, int noteId)
        {
            var note = await _context.Notes
                .Include(n => n.CreatorUser)
                .FirstOrDefaultAsync(n => n.Id == noteId && n.TeamId == teamId && !n.IsDeleted);

            return note == null ? null : MapToDto(note);
        }

        public async Task<NoteResponseDto> CreateNoteAsync(int teamId, int userId, CreateNoteDto dto)
        {
            var note = new Note
            {
                TeamId = teamId,
                CreatorUserId = userId,
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(userId);
            note.CreatorUser = user;

            return MapToDto(note);
        }

        public async Task<NoteResponseDto?> UpdateNoteAsync(int teamId, int noteId, UpdateNoteDto dto)
        {
            var note = await _context.Notes
                .Include(n => n.CreatorUser)
                .FirstOrDefaultAsync(n => n.Id == noteId && n.TeamId == teamId && !n.IsDeleted);

            if (note == null) return null;

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(note);
        }

        public async Task<bool> DeleteNoteAsync(int teamId, int noteId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == noteId && n.TeamId == teamId && !n.IsDeleted);

            if (note == null) return false;

            note.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        private static NoteResponseDto MapToDto(Note note) =>
            new NoteResponseDto(
                note.Id,
                note.TeamId,
                note.Title,
                note.Content,
                note.CreatorUserId,
                note.CreatorUser?.FullName ?? "Unknown User",
                note.CreatedAt,
                note.UpdatedAt
            );
    }
}