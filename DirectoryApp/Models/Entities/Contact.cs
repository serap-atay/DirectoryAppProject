using DirectoryApp.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace DirectoryApp.Models.Entities
{
    public class Contact :BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string SurName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
        public bool isDeleted { get; set; }
    }
}
