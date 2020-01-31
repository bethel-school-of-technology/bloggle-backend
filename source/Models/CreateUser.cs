namespace collaby_backend.Models
{
    public class CreateUser{
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Img { get; set; }
    }
}