using System.ComponentModel.DataAnnotations.Schema;
using TheFruityMixologist.Utilities.Enum;

namespace TheFruityMixologist.Entities
{
    public class Order:BaseEntity
    {
        public string Country { get; set; }

        public string Adress { get; set; }


        public string Note { get; set; }


        public double TotalPrice { get; set; }

        public bool? Status { get; set; }
        public DateTime Date { get; set; }

        public User User { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
