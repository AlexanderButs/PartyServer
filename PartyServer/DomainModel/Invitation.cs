using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PartyServer.DomainModel
{
    internal class Invitation
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("initiator_id")]
        public long InitiatorId { get; set; }

        [Column("target_id")]
        public long TargetId { get; set; }

        [Column("expiration_time")]
        public DateTime ExpirationTime { get; set; }
    }
}