namespace Cor.Apt.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Salt { get; set; }
        public string ApiId { get; set; }
        public string ApiKey { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public string MessageContentType { get; set; }
    }
}