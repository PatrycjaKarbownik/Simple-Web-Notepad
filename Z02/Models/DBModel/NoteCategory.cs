using System.ComponentModel.DataAnnotations.Schema;

namespace Z02.Models.DBModel{
    
    [Table("NoteCategory", Schema = "karbownik")]
    public class NoteCategory{
        
        public int NoteID {get; set;}
        public Note Note {get; set;}
        
        public int CategoryID {get; set;}
        public Category Category {get; set;}
    }
}