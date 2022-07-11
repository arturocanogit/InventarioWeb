namespace InventarioWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Negocio",
                c => new
                    {
                        NegocioId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 128),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaMod = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.NegocioId, t.Nombre });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Negocio");
        }
    }
}
