using TeamNotesApi.Models;

namespace TeamNotesApi.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteResponseDto>> GetTeamNotesAsync(int teamId);
        Task<NoteResponseDto?> GetNoteByIdAsync(int teamId, int noteId);
        Task<NoteResponseDto> CreateNoteAsync(int teamId, int userId, CreateNoteDto dto);
        Task<NoteResponseDto?> UpdateNoteAsync(int teamId, int noteId, UpdateNoteDto dto);
        Task<bool> DeleteNoteAsync(int teamId, int noteId);
    }
}