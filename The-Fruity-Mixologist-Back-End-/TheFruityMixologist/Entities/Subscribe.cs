using System.ComponentModel.DataAnnotations;

namespace TheFruityMixologist.Entities
{
    public class Subscribe:BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
