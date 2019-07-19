namespace Akka.Mailboxes.Demo.Messages
{
    public class TestMessage
    {
        public string Message { get; set; }

        public TestMessage(string message)
        {
            Message = message;
        }
    }
}
