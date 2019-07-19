using Newtonsoft.Json;
using System;
using System.IO;

namespace Akka.Mailboxes
{
    /// <summary>
    /// Quick file storage implementation
    /// </summary>
    public class DurableStorage
    {
        private readonly string _folder;

        public DurableStorage()
        {
            _folder = AppDomain.CurrentDomain.BaseDirectory + "\\mailbox";
            if (!Directory.Exists(_folder))
            {
                Directory.CreateDirectory(_folder);
            }
        }

        public void Persist(string mailboxId, DurableMessage durableMessage)
        {
            // serialize to json, write to file.
            string serialized = JsonConvert.SerializeObject(durableMessage);
            var filename = $"{_folder}/{mailboxId}_{DateTime.UtcNow.Ticks}";
            File.WriteAllText(filename, serialized);
        }
    }
}
