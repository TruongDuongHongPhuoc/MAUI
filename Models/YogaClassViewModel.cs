using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    class YogaClassViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Teacher { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string DayOfWeek { get; set; }
        public int CurrentCapability { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public float PricePerClass { get; set; }

    }
}
