using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Z02.Models;
using Z02.Models.DBModel;

namespace Z02.Repositories{
    public class NotesDbRepository : INotesRepository{
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
        
        public NoteViewModel FindNoteByTitle (string id){
            return new NoteViewModel ();
        }

        public void Add (NoteViewModel model){
            using ( var context = new NotesDbContext () ){
                var note = Mapper.NoteViewModelToNote (model);
                note.NoteCategories = mapStringCategoriesToNoteCategory (context, model.Categories, note);

                context.Notes.Add (note);
                context.SaveChanges ();
            }
        }

        public void Delete (string title){ throw new NotImplementedException (); }
        public void Update (string oldTitle, NoteViewModel note){ throw new NotImplementedException (); }

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
                var category = context.Categories.FirstOrDefault(it => it.Title == stringCategory);

                if ( category == null ){
                    category = new Category{Title = stringCategory};
                    context.Categories.Add (category);
                }

                noteCategories.Add (new NoteCategory{Note = note, Category = category});
            }

            return noteCategories;
        }
    }
}