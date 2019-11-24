using System;
using System.Collections.Generic;
using Z02.Models;

namespace Z02.Repositories{
    public interface INotesRepository{
        List<NoteWithoutContentModel> FindAll ();
        
        NoteViewModel FindNoteByTitle (string title);

        void Add (NoteViewModel model);
        
        void Delete (string title);
        
        void Update (String oldTitle, NoteViewModel note);
    }
}