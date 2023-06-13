using System.ComponentModel.DataAnnotations.Schema;

namespace TheFruityMixologist.Entities
{
    public class EventSliderImages:BaseEntity
    {
        public string Path { get; set; }
        [NotMapped]
        public List<IFormFile?> Images { get; set; }
        public int EventSliderId { get; set; }

        public EventSlider EventSlider { get; set; }

    }
}
