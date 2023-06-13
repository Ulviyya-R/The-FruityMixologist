using Microsoft.Build.Framework;

namespace TheFruityMixologist.Entities
{
    public class Message:BaseEntity
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Text { get; set; }
        public string Image { get; set; }   
        public DateTime When { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
