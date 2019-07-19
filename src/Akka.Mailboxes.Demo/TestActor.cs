using Akka.Actor;
using Akka.Mailboxes.Demo.Messages;
using System;
using System.Threading;

namespace Akka.Mailboxes.Demo
{
    public class TestActor : ReceiveActor
    {
        public TestActor()
        {
            Receive<TestMessage>(msg => Console.WriteLine(msg.Message));
            Receive<BlockingMessage>(_ => Thread.Sleep(-1));
        }
    }
}
