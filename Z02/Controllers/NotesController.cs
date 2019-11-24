using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Z02.Models;
using Z02.Models.DBModel;
using Z02.Repositories;

namespace Z02.Controllers{
    public class NotesController : Controller{
        private readonly NotesFileRepository _notesFileRepository = new NotesFileRepository ();
        private readonly NotesDbRepository _notesDbRepository = new NotesDbRepository ();
        List<NoteWithoutContentModel> _notes;
        SelectList _allCategories;
        DateTime _startDate;
        DateTime _endDate;

        public IActionResult Index (DateTime startDate, DateTime endDate, String chosenCategory, int page = 1){
            UpdateNotesView ();
            ViewData["Page"] = page;

            Filter (startDate, endDate, chosenCategory);
            ViewData["AllPages"] = _notes.Count / 11 + 1;
            Pagination (page);
            
            ViewData["AllCategories"] = _allCategories;
            ViewData["ChosenCategory"] = chosenCategory;
            if ( startDate == DateTime.MinValue ) ViewData["StartDate"] = _startDate;
            else ViewData["StartDate"] = startDate;

            if ( endDate == DateTime.MinValue ) ViewData["EndDate"] = _endDate;
            else ViewData["EndDate"] = endDate;


            return View (_notes);
        }

        public IActionResult Delete (String title){
            _notesFileRepository.Delete (title);
            return RedirectToAction (nameof (Index));
        }

        private void UpdateNotesView (){
            _notes = _notesDbRepository.FindAll ();
            UpdateListOfAllCategories ();
            UpdateDates ();
        }

        private void UpdateListOfAllCategories (){
            Dictionary<string, string> categoriesDictionary = new Dictionary<string, string> ();
            _notesDbRepository.FindAllCategories ()
                              .ForEach (it => categoriesDictionary.Add (it.Title, it.Title) );

            _allCategories = new SelectList (categoriesDictionary, "Key", "Value");
        }

        private void UpdateDates (){
            if ( _notes.Count > 0 ){
                _startDate = _endDate = _notes[0].Date;
                foreach ( var note in _notes ){
                    if ( note.Date > _endDate ) _endDate = note.Date;
                    if ( note.Date < _startDate ) _startDate = note.Date;
                }
            }
        }

        private void Filter (DateTime startDate, DateTime endDate, String chosenCategory){
            if ( startDate != DateTime.MinValue )
                _notes = _notes.Where (it => it.Date >= startDate).ToList ();
            if ( endDate != DateTime.MinValue )
                _notes = _notes.Where (it => it.Date <= endDate).ToList ();
            if ( !string.IsNullOrEmpty (chosenCategory) && !chosenCategory.Equals ("All") )
                _notes = _notes.FindAll (it => it.Categories.Exists (cat => cat.ToLower ().Equals (chosenCategory.ToLower())));
        }

        private void Pagination (int page){
            int numberOfRemainingNotes = _notes.Count - 10 * (page - 1);
            int count = numberOfRemainingNotes < 10 ? numberOfRemainingNotes : 10;

            _notes = _notes.GetRange (10 * (page - 1), count);
        }

        public IActionResult Clear (){ return RedirectToAction (nameof (Index)); }

        public IActionResult NextPage (DateTime startDate, DateTime endDate, String chosenCategory, int page, int allPages){
            page = page + 1 == allPages + 1 ? allPages : page + 1;
            return RedirectToAction (nameof (Index), new{startDate, endDate, chosenCategory, page});
        }

        public IActionResult PreviousPage (DateTime startDate, DateTime endDate, String chosenCategory, int page){
            page = page - 1 == 0 ? 1 : page - 1;
            return RedirectToAction (nameof (Index), new{startDate, endDate, chosenCategory, page});
        }

        public IActionResult Error (){
            return View (new ErrorViewModel{RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}