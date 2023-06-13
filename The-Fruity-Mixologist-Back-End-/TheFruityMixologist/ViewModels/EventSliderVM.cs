using System.ComponentModel.DataAnnotations.Schema;
using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels
{
    public class EventSliderVM
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public List<EventSlider> eventSliders { get; set; }
        public DateTime EventData { get; set; }
        [NotMapped]
        public ICollection<IFormFile>? Images { get; set; }
        [NotMapped]

        public ICollection<EventSliderImages>? AllImages { get; set; }
        [NotMapped]

        public ICollection<int>? ImagesId { get; set; }
    }
}
