namespace PBLprojectMVC.Models
{
    public class DeviceViewModel : StandardViewModel
    {
        public string Name { get; set; }
        public string Type{ get; set; }
        public IFormFile Image { get; set; }
        public byte[] ImageByte { get; set; }
        public string ImageBase64
        {
            get
            {
                if (ImageByte != null)
                    return Convert.ToBase64String(ImageByte);
                else
                    return string.Empty;
            }
        }
        public string Entity { get; set; }
        public string Transport { get; set; }
    }
}
