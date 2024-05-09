namespace HangFire.Data.Models
{
    public class MailRequest
    {
        public Recipient Recipient { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public Dictionary<string , string>? Placeholders { get; set; }
    }

    public class Recipient
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
