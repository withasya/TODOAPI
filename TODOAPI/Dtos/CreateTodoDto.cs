using System;

namespace TODOAPI.Dtos
{
    public class CreateTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
    }
}