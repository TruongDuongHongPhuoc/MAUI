using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    internal class YogaClass
    {

            public int Id { get; set; }
            public string Name { get; set; }
            public string Teacher { get; set; }
            public string Comment { get; set; }
            public string Date { get; set; }
            public int CurrentCapability { get; set; }
            public int CourseID { get; set; }

            public YogaClass() { }
        }

    }

