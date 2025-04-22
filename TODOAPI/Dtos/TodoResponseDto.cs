using System;

namespace TODOAPI.Dtos
{
    public class TodoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
    }
}