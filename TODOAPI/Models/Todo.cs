using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace TODOAPI.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? Deadline { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoStatus Status { get; set; } = TodoStatus.ToDo;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; set; } = Priority.Medium;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Priority
    {
        Low,
        Medium,
        High
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TodoStatus
    {
        ToDo,
        InProgress,
        Completed,
        OnHold
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        EvIsleri,
        Is,
        Arkadas,
        Alisveris,
        Saglik,
        Diger
    }
}