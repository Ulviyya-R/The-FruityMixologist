using System.ComponentModel.DataAnnotations;

namespace TheFruityMixologist.Areas.MixologistArea.ViewModels
{
    public class ChangeRoleUserViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
