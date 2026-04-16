using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models
{
    public class ReportingOrganisation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Format { get; set; }
        public string IdentificationType { get; set; }
    }
}
