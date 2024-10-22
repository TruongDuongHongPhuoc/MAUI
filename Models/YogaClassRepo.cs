
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CourseWork.Models
{
    internal class YogaClassRepo
    {
        List<YogaClass> listYogaClass;
        FirebaseClient firebaseClient;
        private readonly string TABLE_NAME = "yogaclass";
        private readonly string DATABASE_LINK = "";
        public YogaClassRepo()
        {
            firebaseClient = new FirebaseClient(DATABASE_LINK);
        }

        public async Task<List<YogaClass>> GetYogaClassesAsync()
        {
            var yogaClasses = await firebaseClient
                .Child(TABLE_NAME)
                .OnceAsync<YogaClass>();

            var result = new List<YogaClass>();
            foreach (var item in yogaClasses)
            {
                var x = item.Object;
                x.Id = int.Parse(item.Key);
                result.Add(item.Object);
                
            }

            return result;
        }
        public async Task<YogaClass> GetYogaClassWith(int id)
        {
            var yogaClass = await firebaseClient
                .Child(TABLE_NAME)
                .Child(id.ToString())
                .OnceSingleAsync<YogaClass>();

            if (yogaClass != null)
            {
                yogaClass.Id = id; // Set the Id to the provided id
            }

            return yogaClass;
        }
    }

}
