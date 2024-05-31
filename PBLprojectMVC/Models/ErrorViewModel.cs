namespace PBLprojectMVC.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel(string message)
        {
            Error = message;
        }

        public ErrorViewModel() { }

        public string Error { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
