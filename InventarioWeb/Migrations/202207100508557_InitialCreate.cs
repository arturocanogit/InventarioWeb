namespace InventarioWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Almacen",
                c => new
                    {
                        NegocioId = c.Int(nullable: false),
                        AlmacenId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 64),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaMod = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.NegocioId, t.AlmacenId });
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        NegocioId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        AlmacenId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 64),
                        Costo = c.Double(nullable: false),
                        Precio = c.Double(nullable: false),
                        Contenido = c.Int(nullable: false),
                        Unidad = c.String(nullable: false, maxLength: 8),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaMod = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.NegocioId, t.ProductoId })
                .ForeignKey("dbo.Almacen", t => new { t.NegocioId, t.AlmacenId }, cascadeDelete: true)
                .ForeignKey("dbo.Proveedores", t => new { t.NegocioId, t.ProveedorId }, cascadeDelete: true)
                .Index(t => new { t.NegocioId, t.AlmacenId })
                .Index(t => new { t.NegocioId, t.ProveedorId });
            
            CreateTable(
                "dbo.Proveedores",
                c => new
                    {
                        NegocioId = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 64),
                        Activo = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaMod = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.NegocioId, t.ProveedorId });
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Productos", new[] { "NegocioId", "ProveedorId" }, "dbo.Proveedores");
            DropForeignKey("dbo.Productos", new[] { "NegocioId", "AlmacenId" }, "dbo.Almacen");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Productos", new[] { "NegocioId", "ProveedorId" });
            DropIndex("dbo.Productos", new[] { "NegocioId", "AlmacenId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Proveedores");
            DropTable("dbo.Productos");
            DropTable("dbo.Almacen");
        }
    }
}
