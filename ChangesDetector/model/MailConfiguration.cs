namespace ChangesDetector.model
{
    public class MailConfiguration
    {
        public string HostSmtp { get; set; }
        public string MailFromSmtp { get; set; }
        public string LoginSmtp { get; set; }
        public string PasswordSmtp { get; set; }
        public int PortSmtp { get; set; }
        public bool SslSmtp { get; set; }
    }
}
