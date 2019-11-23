using System;
using System.Collections.Generic;
using Z01.Models;

namespace Z01.Repositories{
    public interface INotesRepository{
        List<NoteWithoutContentModel> FindAll ();
        
        NoteViewModel FindNoteByTitle (string title);

        void Add (NoteViewModel note);
        
        void Delete (string title);
        
        void Update (String oldTitle, NoteViewModel note);
    }
}