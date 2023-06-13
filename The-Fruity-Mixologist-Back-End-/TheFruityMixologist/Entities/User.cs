using Microsoft.AspNetCore.Identity;

namespace TheFruityMixologist.Entities
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public string OTP { get; set; }
        public  ICollection<Message> Messages { get; set; }
        public string Path { get; set; }
        public List<Comment> Comments { get; set; }
        public List<gifCart> GifCarts { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
        public List<Order>? Orders { get; set; }


        public User()
        {
            Messages = new List<Message>();
        }
    }
}
