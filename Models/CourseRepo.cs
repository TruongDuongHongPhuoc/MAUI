using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    internal class CourseRepo
    {
        private readonly FirebaseClient firebaseClient;
        private readonly string TABLE_NAME = "course";
        private readonly string DATABASE_LINK = "https://yogaapplicationapp-default-rtdb.asia-southeast1.firebasedatabase.app/";

        public CourseRepo()
        {
            firebaseClient = new FirebaseClient(DATABASE_LINK);
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            var courses = await firebaseClient
                .Child(TABLE_NAME)
                .OnceAsync<Course>();

            var result = new List<Course>();
            foreach (var item in courses)
            {
                var x = item.Object;
                x.Id = int.Parse(item.Key);
                result.Add(item.Object);
            }

            return result;
        }
        public async Task<Course> GetCourseWith(int id)
        {
            var course = await firebaseClient
                .Child(TABLE_NAME)
                .Child(id.ToString())
                .OnceSingleAsync<Course>();

            if (course != null)
            {
                course.Id = id; // Set the Id to the provided id
            }

            return course;
        }
    }
}
