using System;
using System.Collections.Generic;
using Z02.Models;
using Z02.Models.DBModel;

namespace Z02.Repositories{
    public class Mapper{
        public static NoteWithoutContentModel NoteToNoteWithoutContentModel (Note note, List<string> categories){
            return new NoteWithoutContentModel{
                Id = note.NoteID,
                Date = note.NoteDate,
                Title = note.Title,
                Categories = categories
            };
        }
        
        public static NoteViewModel NoteToNoteViewModel (Note note, List<string> categories){
            return new NoteViewModel{
                Id = note.NoteID,
                Date = note.NoteDate,
                Title = note.Title,
                Content = note.Description,
                Categories = categories
            };
        }
        
        public static Note NoteViewModelToNote (NoteViewModel model){
            return new Note{
                NoteDate = model.Date,
                Title = model.Title,
                Description = model.Content
            };
        }
    }
}