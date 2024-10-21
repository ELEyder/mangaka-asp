namespace Mangaka.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreate { get; set; }
        public bool Status { get; set; }
    }
}
