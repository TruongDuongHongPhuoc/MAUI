using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    internal class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartTime { get; set; } // hours: int
        public int Duration { get; set; }
        public int Capability { get; set; }
        public float PricePerClass { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public int TypeID { get; set; }
        public string DaysOfWeek { get; set; }

        public Course() { }
    }
}
