using System.ComponentModel.DataAnnotations.Schema;

namespace TheFruityMixologist.Entities
{
    public class EventSlider:BaseEntity
    {
        public string EventName { get; set; }
        public DateTime EventData { get; set; }
        public List<EventSliderImages> eventSliderImages { get; set; }

    }
}
