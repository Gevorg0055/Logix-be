using System.ComponentModel.DataAnnotations;

namespace LogixTask.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
