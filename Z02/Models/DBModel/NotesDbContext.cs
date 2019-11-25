using Microsoft.EntityFrameworkCore;

namespace Z02.Models.DBModel{
    public class NotesDbContext : DbContext{
        
        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<NoteCategory> NoteCategory { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlServer (
                @"Server=localhost,8200;Database=NTR2019Z;User Id=karbownik;Password=283674;");
            //base.ii.pw.edu.pl,1433
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder){
            modelBuilder.Entity<Note>()
                        .HasIndex(n => n.Title)
                        .IsUnique();
            
            modelBuilder.Entity<Note>()
                        .Property(it => it.RowVersion).IsConcurrencyToken();
            
            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.Title)
                        .IsUnique();
            
            modelBuilder.Entity<NoteCategory> ()
                        .HasKey (it => new{it.CategoryID, it.NoteID});
            
            modelBuilder.Entity<NoteCategory> ()
                        .HasOne<Note> (it => it.Note)
                        .WithMany (it => it.NoteCategories)
                        .HasForeignKey (it => it.NoteID);
            
            modelBuilder.Entity<NoteCategory> ()
                        .HasOne<Category> (it => it.Category)
                        .WithMany (it => it.NotesCategory)
                        .HasForeignKey (it => it.CategoryID);
        }
    }
}