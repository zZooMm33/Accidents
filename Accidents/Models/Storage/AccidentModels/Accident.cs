using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accidents.Models.Storage
{
    public class Accident
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }


        public int IdDanger { get; set; }
        public Danger Danger { get; set; }
        public int IdProfession { get; set; }
        public Profession Profession { get; set; }
        public int IdSourceDanger { get; set; }
        public SourceDanger SourceDanger { get; set; }
    }
}