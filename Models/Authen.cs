using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{
    public class Authen
    {
        private readonly FirebaseAuthClient _authClient;
        public Authen(FirebaseAuthClient authClient)
        {
            this._authClient = authClient;
        }
        public string GetCurrentUserEmail()
        {
            var user = _authClient.User;
            return user?.Info.Email;
        }
    }
}
