using System;

namespace PartyServer.Model
{
    public class Invitation
    {
        public long Id { get; set; }

        public long InitiatorId { get; set; }

        public long TargetId { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}