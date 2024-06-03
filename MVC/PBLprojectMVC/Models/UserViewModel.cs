namespace PBLprojectMVC.Models
{
    public class UserViewModel : StandardViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
