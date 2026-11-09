using Microsoft.EntityFrameworkCore;
using SmartFlow.Domain.Entities;
using System.Data;
using System.Text.RegularExpressions;

namespace SmartFlow.Infrastructure.Data
{
    public class SmartFlowDbContext : DbContext
    {
        public SmartFlowDbContext(DbContextOptions<SmartFlowDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Vente> Ventes { get; set; }
        public DbSet<LigneVente> LignesVente { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produit>()
                .Property(p => p.PrixAchat).HasPrecision(18, 2);

            modelBuilder.Entity<Produit>()
                .Property(p => p.PrixVente).HasPrecision(18, 2);

            modelBuilder.Entity<Vente>()
                .Property(v => v.MontantTotal).HasPrecision(18, 2);

            modelBuilder.Entity<LigneVente>()
                .Property(l => l.PrixUnitaire).HasPrecision(18, 2);

            modelBuilder.Entity<Facture>()
                .Property(f => f.MontantTotal).HasPrecision(18, 2);

            modelBuilder.Entity<Paiement>()
                .Property(p => p.Montant).HasPrecision(18, 2);

            modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Nom = "Admin" },
            new Role { Id = 2, Nom = "Vendeur" },
            new Role { Id = 3, Nom = "Magasinier" }
            );
        }
    }
}