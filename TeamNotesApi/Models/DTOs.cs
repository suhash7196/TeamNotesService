namespace TeamNotesApi.Models
{
    public record CreateNoteDto(string Title, string? Content);
    public record UpdateNoteDto(string Title, string? Content);
    
    public record NoteResponseDto(
        int Id, 
        int TeamId, 
        string Title, 
        string? Content, 
        int CreatorUserId, 
        string CreatorName, 
        DateTime CreatedAt, 
        DateTime? UpdatedAt
    );
}