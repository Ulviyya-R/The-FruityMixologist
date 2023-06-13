using System.ComponentModel.DataAnnotations.Schema;

namespace TheFruityMixologist.Entities
{
    public class PopularSlider:BaseEntity
    {
        public string? ImagePath { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
