using System.Text.Json.Serialization;
using TODOAPI.Models;

namespace TODOAPI.Dtos
{
    public class UpdateTodoStatusDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoStatus Status { get; set; }
    }
}