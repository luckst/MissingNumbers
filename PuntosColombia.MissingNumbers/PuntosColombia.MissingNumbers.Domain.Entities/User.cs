namespace PuntosColombia.MissingNumbers.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }
    }
}
