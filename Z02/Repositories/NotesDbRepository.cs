using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Z02.Models;
using Z02.Models.DBModel;

namespace Z02.Repositories{
    public enum UpdateState{
        Ok,
        Deleted,
        Changed
    };

    public class NotesDbRepository {
        public List<NoteWithoutContentModel> FindAll (){
            List<NoteWithoutContentModel> notes = new List<NoteWithoutContentModel> ();

            using ( var context = new NotesDbContext () ){
                foreach ( var note in context.Notes.ToList () ){
                    List<string> stringCategories = mapNoteCategoriesToStringCategories (context, note);
                    notes.Add (Mapper.NoteToNoteWithoutContentModel (note, stringCategories));
                }
            }

            return notes;
        }

        public NoteViewModel FindNoteById (int id){
            using ( var context = new NotesDbContext () ){
                Note note = context.Notes.Find (id);
                if ( note != null ){
                    List<string> stringCategories = mapNoteCategoriesToStringCategories (context, note);
                    return Mapper.NoteToNoteViewModel (note, stringCategories);
                }

                return null;
            }
        }

        public void Add (NoteViewModel model){
            using ( var context = new NotesDbContext () ){
                var note = Mapper.NoteViewModelToNote (model);
                note.NoteCategories = mapStringCategoriesToNoteCategory (context, model.Categories, note);

                context.Notes.Add (note);
                context.SaveChanges ();
            }
        }

        public void Delete (int id){
            using ( var context = new NotesDbContext () ){
                Note original = context.Notes.Find (id);
                if ( original == null ) return;

                context.Notes.Remove (original);
                context.SaveChanges ();
            }
        }

        public UpdateState Update (NoteViewModel note){
            using ( var context = new NotesDbContext () ){
                Note original = context.Notes
                                       .Include (it => it.NoteCategories)
                                       .ThenInclude (noteCategory => noteCategory.Category)
                                       .FirstOrDefault (dbNote => dbNote.NoteID == note.Id);

                if ( original == null ) 
                    return UpdateState.Deleted;

                context.Entry (original).Property ("RowVersion").OriginalValue = note.RowVersion;
                original.Title = note.Title;
                original.NoteDate = note.Date;
                original.Description = note.Content;
                original.NoteCategories = mapStringCategoriesToNoteCategory (context, note.Categories, original);

                try{
                    context.SaveChanges ();
                }
                catch ( DbUpdateConcurrencyException e ){
                    var exceptionEntry = e.Entries.Single ();
                    var databaseEntry = exceptionEntry.GetDatabaseValues ();

                    if ( databaseEntry == null ) return UpdateState.Deleted;
                    return UpdateState.Changed;
                }

                return UpdateState.Ok;
            }
        }

        public List<Category> FindAllCategories (){
            List<Category> categories;
            using ( var context = new NotesDbContext () ){
                categories = context.Categories.ToList ();
            }

            return categories;
        }

        private List<NoteCategory> mapStringCategoriesToNoteCategory (NotesDbContext context,
                                                                      List<string> stringCategories,
                                                                      Note note){
            List<NoteCategory> noteCategories = new List<NoteCategory> ();

            foreach ( string stringCategory in stringCategories ){
                var category = context.Categories.FirstOrDefault (it => it.Title == stringCategory);

                if ( category == null ){
                    category = new Category{Title = stringCategory};
                    context.Categories.Add (category);
                }

                noteCategories.Add (new NoteCategory{Note = note, Category = category});
            }

            return noteCategories;
        }

        private List<string> mapNoteCategoriesToStringCategories (NotesDbContext context, Note note){
            List<string> stringCategories = new List<string> ();
            List<NoteCategory> noteCategories = context.NoteCategory.ToList ()
                                                       .FindAll (it => it.Note == note);

            foreach ( var noteCategory in noteCategories ){
                Category category = context.Categories.Find (noteCategory.CategoryID);
                stringCategories.Add (category.Title);
            }

            return stringCategories;
        }
    }
}