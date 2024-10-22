
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    internal class TypeRepo
    {
        private readonly FirebaseClient firebaseClient;
        private readonly string TABLE_NAME = "type";
        private readonly string DATABASE_LINK = "/";

        public TypeRepo()
        {
            firebaseClient = new FirebaseClient(DATABASE_LINK);
        }
        public async Task<List<Type>> GetTypesAsync()
        {
            var types = await firebaseClient
                .Child(TABLE_NAME)
                .OnceAsync<Type>();

            var result = new List<Type>();
            foreach (var item in types)
            {
                var type = item.Object;
                type.Id = int.Parse(item.Key); // Set the Id to the Firebase key
                result.Add(type);
            }

            return result;
        }
        public async Task<Type> GetTypeWith(int id)
        {
            var type = await firebaseClient
                .Child(TABLE_NAME)
                .Child(id.ToString())
                .OnceSingleAsync<Type>();

            if (type != null)
            {
                type.Id = id; // Set the Id to the provided id
            }

            return type;
        }
    }
}
