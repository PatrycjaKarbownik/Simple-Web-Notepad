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
            modelBuilder.Entity<NoteCategory> ().HasKey (i => new{i.CategoryID, i.NoteID});
            modelBuilder.Entity<NoteCategory> ().HasOne<Note> (i => i.Note)
                        .WithMany (i => i.NoteCategories)
                        .HasForeignKey (i => i.NoteID);
            modelBuilder.Entity<NoteCategory> ().HasOne<Category> (i => i.Category)
                        .WithMany (i => i.NotesCategory)
                        .HasForeignKey (i => i.CategoryID);
        }
    }
}