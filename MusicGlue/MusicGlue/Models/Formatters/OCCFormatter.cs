using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models.Formatters
{
    public class OCCFormatter : IFormatter
    {
        public string Format(List<Consignment> consignments)
        {
            //Header line
            string zipCode = "0" + consignments[0].ZipCode;
            string date = DateTime.Now.ToString("YYMMDD");
            
            //
            foreach (MusicProduct musicProduct in consignments[0].MusicProducts)
            {

            }
            

            //footer

            return string.Empty;
        }
    }
}
