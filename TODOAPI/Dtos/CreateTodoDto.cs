using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TODOAPI.Models;

namespace TODOAPI.Dtos
{
    public class CreateTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; }  // Enum olarak tanımlandı
        public Category Category { get; set; }  // Enum olarak tanımlandı
    }
}