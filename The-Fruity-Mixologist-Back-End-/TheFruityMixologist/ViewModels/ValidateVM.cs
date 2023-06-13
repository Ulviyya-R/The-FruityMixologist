using Microsoft.Build.Framework;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels
{
    public class ValidateVM
    {
        public string Email { get; set; }
        [Required]
        public string Opt { get; set; }
        public User  user { get; set; }


        public ValidateVM()
        {
            user = new();
        }
    }
}
