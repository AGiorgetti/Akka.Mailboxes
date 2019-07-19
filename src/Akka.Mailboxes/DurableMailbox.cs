using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;

namespace Akka.Mailboxes
{
    public sealed class DurableUnboundedMailbox : MailboxType, IProducesMessageQueue<UnboundedMessageQueue>
    {
        /// <inheritdoc cref="MailboxType"/>
        public override IMessageQueue Create(IActorRef owner, ActorSystem system)
        {
            return new DurableWrapperMessageQueue(owner, system, new UnboundedMessageQueue());
        }


        /// <summary>
        /// Default constructor for an unbounded mailbox.
        /// </summary>
        public DurableUnboundedMailbox() : this(null, null)
        {
        }

        /// <inheritdoc cref="MailboxType"/>
        public DurableUnboundedMailbox(Settings settings, Config config) : base(settings, config)
        {
        }
    }
}
