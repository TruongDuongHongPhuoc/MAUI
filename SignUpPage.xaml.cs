using CourseWork.Models;
using Firebase.Auth;

namespace CourseWork;

public partial class SignUpPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    // Email property to bind to Entry
    public string Email { get; set; }

    // Password property to bind to Entry
    public string Password { get; set; }
    // Password property to bind to Entry
    public string RePassword { get; set; }
    public SignUpPage(FirebaseAuthClient authClient)
	{
		InitializeComponent();
        _authClient = authClient;
        BindingContext = this;
    }
    private async Task SignUp()
    {
        // Confirm password
        //if (!Password.Equals(RePassword)){
        //    DisplayAlert("Alert", "your confirm password and password is not the same try again", "Cancel");
        //    return;
        //}

        try
        {
            // Use the Firebase Auth client to create a new user
            var result = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password);
           
            // Successful sign-up logic
            await DisplayAlert("Success", "Account created successfully!", "OK");
            // Navigate to login or main app page
            await Navigation.PushAsync(new LoginPage(_authClient));
        }
        catch (Exception ex)
        {
            // Handle sign-up failure
            await DisplayAlert("Sign-Up Failed", ex.Message, "OK");
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
       await SignUp();
    }
}