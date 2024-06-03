using System.ComponentModel.DataAnnotations;

namespace PBLprojectMVC.Models
{
    public class TemperatureViewModel : StandardViewModel
    {
        public DateTime Time { get; set; }
        public float Value { get; set; }
        public int DeviceId { get; set; }
    }
}
