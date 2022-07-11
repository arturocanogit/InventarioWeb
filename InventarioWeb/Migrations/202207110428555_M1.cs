namespace InventarioWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventario",
                c => new
                    {
                        NegocioId = c.Int(nullable: false),
                        InventarioId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaMod = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.NegocioId, t.InventarioId })
                .ForeignKey("dbo.Productos", t => new { t.NegocioId, t.ProductoId }, cascadeDelete: true)
                .Index(t => new { t.NegocioId, t.ProductoId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventario", new[] { "NegocioId", "ProductoId" }, "dbo.Productos");
            DropIndex("dbo.Inventario", new[] { "NegocioId", "ProductoId" });
            DropTable("dbo.Inventario");
        }
    }
}
