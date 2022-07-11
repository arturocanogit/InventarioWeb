namespace InventarioWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Negocio");
            AlterColumn("dbo.Negocio", "NegocioId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Negocio", "Nombre", c => c.String(nullable: false, maxLength: 64));
            AddPrimaryKey("dbo.Negocio", "NegocioId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Negocio");
            AlterColumn("dbo.Negocio", "Nombre", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Negocio", "NegocioId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Negocio", new[] { "NegocioId", "Nombre" });
        }
    }
}
