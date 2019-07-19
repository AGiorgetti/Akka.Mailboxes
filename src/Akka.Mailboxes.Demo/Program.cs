using Akka.Actor;
using Akka.Configuration;
using Akka.Mailboxes.Demo.Messages;
using EasyConsoleApplication;
using EasyConsoleApplication.Menus;
using EasyConsoleApplication.Pages;
using System;
using System.Reflection;

namespace Akka.Mailboxes.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string hocon = System.IO.File.ReadAllText(@".\hocon.cfg");
            var config = ConfigurationFactory.ParseString(hocon); // .WithFallback(...);
            var actorSystem = ActorSystem.Create("ConsoleApp", config);
            ConsoleHelpers.WriteGreen($"[{Assembly.GetExecutingAssembly().FullName}]");
            ConsoleHelpers.WriteGreen($"'{actorSystem.Name}' Actor System Created.");

            // start the Application Actors
            var testActorRef = actorSystem.ActorOf(
                Props.Create<TestActor>()
                     .WithMailbox("durable-unbounded-mailbox")
                , "TestActor");

            // display the application
            Application.GoTo<ApplicationPage>(testActorRef);

            // wait for the actor system to terminate
            ConsoleHelpers.WriteGreen("Awaiting for ActorSystem Termination.");
            actorSystem.Terminate();
            actorSystem.WhenTerminated.Wait();
            ConsoleHelpers.WriteGreen("ActorSystem Terminated.");
            ConsoleHelpers.HitEnterToContinue();
        }
    }

    public class ApplicationPage : Page
    {
        private readonly IActorRef _testActorRef;

        public ApplicationPage(IActorRef testActorRef)
        {
            Title = "Mailbox Test Application";
            TitleColor = ConsoleColor.Green;
            Body = $"----\n";
            BodyColor = ConsoleColor.Green;
            MenuItems.Add(new MenuItem("Send a message that will block the actor, then queue some more", () =>
            {
                _testActorRef.Tell(new TestMessage("Message 1"));
                _testActorRef.Tell(BlockingMessage.Instance);
                _testActorRef.Tell(new TestMessage("Message 2"));
                _testActorRef.Tell(new TestMessage("Message 3"));
            }));
            MenuItems.Add(Separator.Instance);
            MenuItems.Add(new MenuItem("q", "Quit", () => Application.Exit()));
            _testActorRef = testActorRef;
        }
    }
}
