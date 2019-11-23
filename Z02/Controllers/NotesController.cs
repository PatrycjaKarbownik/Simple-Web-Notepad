using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Z01.Models;
using Z01.Repositories;

namespace Z01.Controllers{
    public class NotesController : Controller{
        readonly NotesFileRepository _notesFileRepository = new NotesFileRepository ();
        List<NoteWithoutContentModel> _notes;
        SelectList _allCategories;
        DateTime _startDate;
        DateTime _endDate;

        public IActionResult Index (DateTime startDate, DateTime endDate, String chosenCategory, int page = 1){
            Update ();
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

        private void Update (){
            _notes = _notesFileRepository.FindAll ();
            UpdateListOfAllCategories ();
            UpdateDates ();
        }

        private void UpdateListOfAllCategories (){
            List<String> categoryStrings = new List<String> ();
            foreach ( var note in _notes ){
                foreach ( var category in note.Categories ){
                    if ( categoryStrings.Find (it => it.ToLower().Equals (category.ToLower())) == null )
                        categoryStrings.Add (category.ToLower());
                }
            }

            Dictionary<string, string> categoriesDictionary = new Dictionary<string, string> ();
            categoryStrings.ForEach (it => { categoriesDictionary.Add (it, it); });

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