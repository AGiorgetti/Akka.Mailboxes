using Akka.Actor;
using Akka.Dispatch.MessageQueues;

namespace Akka.Mailboxes
{
    /// <summary>
    /// first approach at providing a durable queue wrapper to implement
    /// durable mailboxes
    /// </summary>
    public class DurableWrapperMessageQueue : IMessageQueue
    {
        /// <summary>
        /// The underlying <see cref="IMessageQueue"/>.
        /// </summary>
        protected readonly IMessageQueue MessageQueue;

        private readonly string DurableQueueId;

        private DurableMessageSerialization _serialization;
        private DurableStorage _durableStorage = new DurableStorage();

        /// <summary>
        /// Takes another <see cref="IMessageQueue"/> as an argument - wraps <paramref name="messageQueue"/>
        /// in order to provide it durability semantics.
        /// </summary>
        /// <param name="messageQueue">The underlying message queue wrapped by this one.</param>
        public DurableWrapperMessageQueue(
            IActorRef owner,
            ActorSystem system,
            IMessageQueue messageQueue)
        {
            _serialization = new DurableMessageSerialization(owner, system);
            DurableQueueId = "MailBox_" + owner.Path.Name;
            // todo: reload all the messages from the stoare and enqueue them
            MessageQueue = messageQueue;
        }

        public bool HasMessages => MessageQueue.HasMessages;

        public int Count => MessageQueue.Count;

        public void CleanUp(IActorRef owner, IMessageQueue deadletters)
        {
            while (TryDequeue(out Envelope msg))
            {
                deadletters.Enqueue(owner, msg);
            }
        }

        public void Enqueue(IActorRef receiver, Envelope envelope)
        {
            // todo: persist to a storage
            var durableMessage = _serialization.Serialize(envelope);
            _durableStorage.Persist(DurableQueueId, durableMessage);
            MessageQueue.Enqueue(receiver, envelope);
        }

        public bool TryDequeue(out Envelope envelope)
        {
            var msg = MessageQueue.TryDequeue(out envelope);
            // todo: remove from the durable storage
            return msg;
        }
    }
}
