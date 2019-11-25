using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Z02.Models;
using Z02.Repositories;

namespace Z02.Controllers{
    public class NewOrEditNoteController : Controller{
        private readonly NotesDbRepository _notesDbRepository = new NotesDbRepository ();

        public IActionResult Index (int id){
            NoteViewModel note = null;
            if ( id != -1 )
                note = _notesDbRepository.FindNoteById (id);

            if ( note == null ) note = new NoteViewModel ();

            return View (note);
        }

        [HttpPost]
        public IActionResult Save (NoteViewModel note){
            if ( ModelState.IsValid ){
                if ( IfTitleExists (note) ){
                    ModelState.AddModelError ("Title", "Title already exists");
                    return View ("Index", note);
                }

                if ( note.Id == -1 ) _notesDbRepository.Add (note);
                else{
                    UpdateState updateResult = _notesDbRepository.Update (note);

                    if ( updateResult == UpdateState.Deleted ){
                        ModelState.AddModelError ("Title",
                                                  "Update not available. The note was deleted by another user.");
                        return View ("Index", note);
                    }

                    if ( updateResult == UpdateState.Changed ){
                        ModelState.Clear ();
                        ModelState.AddModelError ("Title",
                                                  "Update not available. The note was edited by another user "
                                                  + "after you got the original value.");
                        return View ("Index", note);
                    }
                }
                return Cancel ();
            }

            return View ("Index", note);
        }

        public IActionResult Cancel (){ return RedirectToAction ("Index", "Notes"); }

        private Boolean IfTitleExists (NoteViewModel note){
            List<String> noteTitles = _notesDbRepository.FindAll ()
                                                        .FindAll (noteFromDb => noteFromDb.Id != note.Id)
                                                        .ConvertAll (it => it.Title);
            if ( noteTitles.Contains (note.Title) ) return true;
            return false;
        }
    }
}