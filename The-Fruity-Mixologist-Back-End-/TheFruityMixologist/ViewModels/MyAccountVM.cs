using System.ComponentModel.DataAnnotations;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels
{
    public class MyAccountVM
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
    }
}
