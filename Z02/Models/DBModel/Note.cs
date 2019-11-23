using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z02.Models.DBModel{
    
    [Table("Note", Schema = "karbownik")]
    public class Note{
        
        [DatabaseGenerated (DatabaseGeneratedOption.Identity)]
        public int NoteID { get; set; }

        public DateTime NoteDate { get; set; }

        [StringLength (64)] public string Title { get; set; }

        public string Description { get; set; }

        public IList<NoteCategory> NoteCategories { get; set; }
    }
}