using TheFruityMixologist.Entities;

namespace TheFruityMixologist.ViewModels
{
    public class SlidersVM
    {
         public List<PopularSlider> slider { get; set; }
         public List<Comment> comments { get; set; }

        public SlidersVM()
        {
            slider = new();
        }
    }
}
