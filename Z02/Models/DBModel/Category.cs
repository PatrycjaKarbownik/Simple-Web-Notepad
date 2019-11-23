using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z02.Models.DBModel{
    
    [Table("Category", Schema = "karbownik")]
    public class Category{
        
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [StringLength (64)] public string Title { get; set; }

        public IList<NoteCategory> NotesCategory { get; set; }
    }
}