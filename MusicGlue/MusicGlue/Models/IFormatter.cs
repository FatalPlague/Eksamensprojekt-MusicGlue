using MusicGlue.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicGlue.Models
{
    public interface IFormatter
    {
        string Format(List<Consignment> consignments);
    }
}