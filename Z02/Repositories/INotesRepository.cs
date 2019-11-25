using System;
using System.Collections.Generic;
using Z02.Models;

namespace Z02.Repositories{
    public interface INotesRepository{
        List<NoteWithoutContentModel> FindAll ();
        
        NoteViewModel FindNoteById (int id);

        void Add (NoteViewModel model);
        
        void Delete (int id);
        
        void Update (String oldTitle, NoteViewModel note);
    }
}