namespace PBLprojectMVC.Models
{
    public class UserViewModel : StandardViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool IsAdmin { get; set; }
    }
}
