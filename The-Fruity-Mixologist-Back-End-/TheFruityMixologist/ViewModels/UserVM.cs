using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheFruityMixologist.ViewModels
{
    public class UserVM
    {
        public string? Username { get; set; }

        public string? Fullname { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [NotMapped]
        public IFormFile ProfilePicture { get; set; }

        public string Path { get; set; }
    }
}
