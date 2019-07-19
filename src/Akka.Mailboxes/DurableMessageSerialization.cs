using Akka.Actor;
using System;

namespace Akka.Mailboxes
{
    public class DurableMessageSerialization
    {
        private readonly ActorSystem _actorSystem;
        private readonly IActorRef _actor;

        public DurableMessageSerialization(IActorRef actor, ActorSystem actorSystem)
        {
            _actorSystem = actorSystem;
            _actor = actor;
        }

        public DurableMessage Serialize(Envelope envelope)
        {
            return new DurableMessage
            {
                Message = envelope.Message,
                Recipient = _actor.Path.ToString(),
                Sender = envelope.Sender.Path.ToString()
            };
        }

        public Envelope Deserialize(DurableMessage data)
        {
            var sender = _actorSystem.ActorSelection(data.Sender).ResolveOne(TimeSpan.Zero).Result;
            return new Envelope(data.Message, sender, _actorSystem);
        }
    }

    public class DurableMessage
    {
        public object Message { get; set; }
        public string Recipient { get; set; }
        public string Sender { get; set; }
    }
}
