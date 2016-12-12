using System.Data.Entity;

using PartyServer.DomainModel;

namespace PartyServer.DAL
{
    public class PartyContext : DbContext
    {
        public PartyContext()
        {
            // Turn off the Migrations, (NOT a code first Db)
            Database.SetInitializer<PartyContext>(null);
        }

        public DbSet<Invitation> Invitations { get; set; }
    }
}