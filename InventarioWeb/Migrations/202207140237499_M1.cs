namespace InventarioWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proveedores", "Direccion", c => c.String(maxLength: 256));
            AddColumn("dbo.Proveedores", "Email", c => c.String(maxLength: 32));
            AddColumn("dbo.Proveedores", "Telefono", c => c.String(maxLength: 10));
            AddColumn("dbo.Proveedores", "Rfc", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proveedores", "Rfc");
            DropColumn("dbo.Proveedores", "Telefono");
            DropColumn("dbo.Proveedores", "Email");
            DropColumn("dbo.Proveedores", "Direccion");
        }
    }
}
