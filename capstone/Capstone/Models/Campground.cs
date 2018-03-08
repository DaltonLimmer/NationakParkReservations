using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    class Campground
    {
        public int Campground_Id { get; set; }
        public int Park_Id { get; set; }
        public string Name { get; set; }
        public int Open_From_MM { get; set; }
        public int Open_To_MM { get; set; }
        public double Daily_Fee { get; set; }

    }
}
