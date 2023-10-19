namespace DocAisle_Email.Models
{
    public class EmailDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;
    }
}
