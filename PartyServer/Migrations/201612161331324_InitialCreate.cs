using System;
using System.Data.Entity.Migrations;

namespace PartyServer.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        initiator_id = c.Long(nullable: false),
                        target_id = c.Long(nullable: false),
                        expiration_time = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.id);
        }
        
        public override void Down()
        {
            DropTable("dbo.Invitations");
        }
    }
}