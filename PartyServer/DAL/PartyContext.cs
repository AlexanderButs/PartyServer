using System.Data.Entity;

using PartyServer.DomainModel;

namespace PartyServer.DAL
{
    internal class PartyContext : DbContext
    {
        public PartyContext()
        {
        }

        public DbSet<Invitation> Invitations { get; set; }
    }
}