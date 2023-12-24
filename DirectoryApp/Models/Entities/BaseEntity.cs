using System.ComponentModel.DataAnnotations;

namespace DirectoryApp.Models.Entities
{
    public class BaseEntity
    {
            [Key]
            public int Id { get; set; }
            public DateTime CreatedDate { get; set; } = DateTime.Now;

            [StringLength(128)]
            public string CreatedUser { get; set; }
        }
}
