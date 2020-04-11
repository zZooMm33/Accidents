using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accidents.Models.Storage
{
    public class Accident
    {
        [Required]
        [DataType(DataType.Text)]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Link { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }


        public int? DangerId { get; set; }
        public Danger Danger { get; set; }
        public int? ProfessionId { get; set; }
        public Profession Profession { get; set; }
        public int? SourceDangerId { get; set; }
        public SourceDanger SourceDanger { get; set; }
    }
}