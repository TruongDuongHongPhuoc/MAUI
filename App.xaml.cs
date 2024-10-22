using Firebase.Auth.Providers;
using Firebase.Auth;

namespace CourseWork
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var authClient = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "API KEY",
                AuthDomain = "AUTH DOMAIN FIREBASE",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });

            MainPage = new NavigationPage(new LoginPage(authClient));
            //MainPage = new NavigationPage(new MainPage(authClient));
        }
    }
}
