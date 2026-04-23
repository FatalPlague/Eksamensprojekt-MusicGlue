using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models.Formatters
{
    public class OCCFormatter : IFormatter
    {
        public string Format(List<Consignment> consignments)
        {
            string occ = "0" + consignments[0].ZipCode

            return string.Empty;
        }
    }
}
