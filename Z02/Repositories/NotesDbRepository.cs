using System.Collections.Generic;
using System.Linq;
using Z02.Models;
using Z02.Models.DBModel;

namespace Z02.Repositories{
    public class NotesDbRepository : INotesRepository{

        public List<NoteWithoutContentModel> FindAll (){
            List<NoteWithoutContentModel> noteData = new List<NoteWithoutContentModel>();

            using (var context = new NotesDbContext()) {
            
                foreach (var note in context.Notes.ToList()) {
                    //var categories = context.NoteCategory.ToList().FindAll(i => i.Note == note);
                    List<string> cat = new List<string>();

                    /*foreach (var category in categories) {
                        var catt = context.Categories.Find(category.CategoryID);
                        cat.Add(catt.Title);
                    }*/

                    noteData.Add(new NoteWithoutContentModel{
                        Date = note.NoteDate,
                        Title = note.Title,
                        Categories = cat
                    });
                }
            
            }
            return noteData;
        }
        public NoteViewModel FindNoteByTitle (string title){ throw new System.NotImplementedException (); }
        public void Add (NoteViewModel note){ throw new System.NotImplementedException (); }
        public void Delete (string title){ throw new System.NotImplementedException (); }
        public void Update (string oldTitle, NoteViewModel note){ throw new System.NotImplementedException (); }
    }
}