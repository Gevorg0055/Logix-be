using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogixTask.Models
{
    public class UserClass
    {
        [Key]
        public int Id { get; set; }
        public string WebUserId { get; set; }
        public int CourseId { get; set; }  

        [ForeignKey("WebUserId")]
        public virtual WebUser WebUser { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

    }
}
