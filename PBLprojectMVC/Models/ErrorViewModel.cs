namespace PBLprojectMVC.Models
{
    public class ErrorViewModel
    {
      
        public string? Error;
        public string? RequestId { get; set; }
      
        public ErrorViewModel(){
        }

        public ErrorViewModel(string error){
            this.Error = error;
        }
      
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}