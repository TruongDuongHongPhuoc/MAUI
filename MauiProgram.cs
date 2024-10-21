using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Logging;

namespace CourseWork
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyAbRrvf1Zgjos0SnUiZflVXgfwX55wX2_0",
                AuthDomain = "yogaapplicationapp.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            }));
            return builder.Build();
        }
    }
}
