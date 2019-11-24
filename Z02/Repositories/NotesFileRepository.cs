using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Z02.Models;

namespace Z02.Repositories{
    public class NotesFileRepository : INotesRepository{
        private String directory = "./Resources";

        public List<NoteWithoutContentModel> FindAll (){
            List<String> fileNames = Directory.GetFiles (directory).ToList ();
            List<NoteWithoutContentModel> notes = new List<NoteWithoutContentModel> ();

            foreach ( String fileName in fileNames ){
                using ( StreamReader file = new StreamReader (fileName) ){
                    notes.Add (new NoteWithoutContentModel{
                        Title = extractTitle (fileName),
                        Categories = extractCategories (file.ReadLine ()),
                        Date = extractDate (file.ReadLine ())
                    });
                }
            }

            return notes;
        }

        public NoteViewModel FindNoteByTitle (string title){
            List<String> fileNames = Directory.GetFiles (directory).ToList ();
            String foundFile = fileNames.Find (it => extractTitle (it).Equals (title));

            NoteViewModel note;

            using ( StreamReader file = new StreamReader (foundFile) ){
                note = new NoteViewModel{
                    Title = title,
                    Categories = extractCategories (file.ReadLine ()),
                    Date = extractDate (file.ReadLine ()),
                    IsMarkdown = extractExtension (foundFile),
                    Content = file.ReadToEnd ()
                };
            }

            return note;
        }

        public void Add (NoteViewModel model){
            StringBuilder stringBuilder = new StringBuilder ("");

            stringBuilder.Append ("category:");
            int numberOfCategories = model.Categories.Count;
            for ( int i = 0; i < numberOfCategories - 1; ++i )
                stringBuilder.Append (" " + model.Categories[i] + ",");
            if ( model.Categories.Count != 0 ) stringBuilder.Append (model.Categories.Last ());

            IFormatProvider culture = new CultureInfo ("en-US", true);
            stringBuilder.Append ("\ndate: " + model.Date.ToString ("yyyy/MM/dd", culture) + "\n");
            
            stringBuilder.Append (model.Content);
            string path = directory + "/" + model.Title + "." + (model.IsMarkdown ? "md" : "txt");
            
            File.WriteAllText (path, stringBuilder.ToString ());
        }

        public void Delete (string title){
            List<String> fileNames = Directory.GetFiles (directory).ToList ();
            String fileToDelete = fileNames.Find (it => extractTitle (it).Equals (title));
            File.Delete (fileToDelete);
        }

        public void Update (String oldTitle, NoteViewModel note){
            Delete (oldTitle);
            Add (note);
        }

        private List<String> extractCategories (String lineWithCategories){
            List<String> result = lineWithCategories.Split (":")[1].Split (",").Select (it => it.Trim ()).ToList ();
            if(result.First().Equals ("")) return new List<String>();
            return result;
        }

        private DateTime extractDate (String lineWithDate){
            String date = lineWithDate.Split (":")[1].Trim ();
            return DateTime.Parse (date);
        }

        private String extractTitle (String fileName){ return fileName.Split ("/").Last ().Split (".")[0]; }

        private Boolean extractExtension (String fileName){
            String ext = fileName.Split ("/").Last ().Split (".")[1];
            if ( ext.ToLower () == "txt" ) return false;

            return true;
        }
    }
}