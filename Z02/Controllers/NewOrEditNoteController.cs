using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Z02.Models;
using Z02.Repositories;

namespace Z02.Controllers{
    public class NewOrEditNoteController : Controller{
        private readonly NotesFileRepository _notesFileRepository = new NotesFileRepository ();

        public IActionResult Index (String title){
            ViewData["TitleExists"] = false;
            NoteViewModel note = null;
            if ( title != null )
                note = _notesFileRepository.FindNoteByTitle (title);

            if ( note == null ) note = new NoteViewModel ();
            else note.IsEdit = true;

            return View (note);
        }

        [HttpPost]
        public IActionResult Save (String oldTitle, NoteViewModel note){
            if ( ModelState.IsValid ){
                if ( IfTitleExists(oldTitle, note.Title) ){
                    ViewData["TitleExists"] = true;
                    return View("Index", note);
                }
                    
                if ( !note.IsEdit ) _notesFileRepository.Add (note);
                else{
                    if ( _notesFileRepository.FindNoteByTitle (oldTitle) == null ) return NotFound ();
                    _notesFileRepository.Update (oldTitle, note);
                }
                return Cancel ();
            }

            ViewData["TitleExists"] = false;
            return View("Index", note);
        }

        public IActionResult Cancel (){ return RedirectToAction ("Index", "Notes"); }

        private Boolean IfTitleExists (String oldTitle, String newTitle){
            List<String> noteTitles = _notesFileRepository.FindAll ().ConvertAll (it => it.Title);
            if ( string.IsNullOrEmpty (oldTitle) ) oldTitle = newTitle;
            if ( !oldTitle.Equals (newTitle) && noteTitles.Contains (newTitle) ) return true;
            return false;
        }
    }
}