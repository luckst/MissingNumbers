namespace PuntosColombia.MissingNumbers.Models.Security
{
    public class UserViewModel : BaseViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
    }
}
