using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Z02.Models;
using Z02.Repositories;

namespace Z02.Controllers{
    public class NewOrEditNoteController : Controller{
        private readonly NotesDbRepository _notesDbRepository = new NotesDbRepository ();

        public IActionResult Index (int id){
            ViewData["TitleExists"] = false;
            NoteViewModel note = null;
            if ( id != -1 )
                note = _notesDbRepository.FindNoteById (id);

            if ( note == null ) note = new NoteViewModel ();

            return View (note);
        }

        [HttpPost]
        public IActionResult Save (NoteViewModel note){
            if ( ModelState.IsValid ){
                if ( IfTitleExists(note) ){
                    ViewData["TitleExists"] = true;
                    return View("Index", note);
                }
                    
                if ( note.Id == -1 ) _notesDbRepository.Add (note);
                else{
                    if ( _notesDbRepository.FindNoteById (note.Id) == null ) return NotFound ();
                    Console.WriteLine("update");
//                    _notesFileRepository.Update (oldTitle, note);
                }
                return Cancel ();
            }

            ViewData["TitleExists"] = false;
            return View("Index", note);
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