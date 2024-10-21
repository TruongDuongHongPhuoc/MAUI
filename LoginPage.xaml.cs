using Firebase.Auth;

namespace CourseWork;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseAuthClient _authClient;

    // Email property to bind to Entry
    public string Email { get; set; }

    // Password property to bind to Entry
    public string Password { get; set; }

    public LoginPage(FirebaseAuthClient authClient)
	{
		InitializeComponent();

        this._authClient = authClient;
        BindingContext = this;
    }
    private async Task Login()
    {
        try
        {
            Email = "215@gmail.com";
            Password = "Phuoc287287";
            // Use the Firebase Auth client to sign in
            var result = await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

            // Successful login logic
            //await DisplayAlert("Success", "Login successful!", "OK");
            // Navigate to the next page, replace with your main app page
            Application.Current.MainPage = new NavigationPage(new MainPage( _authClient));
        }
        catch (Exception ex)
        {
            // Handle login failure
            await DisplayAlert("Login Failed", "your username or password is incorrect", "OK");
        }
    }

    //Login Button
    private async void Button_Clicked(object sender, EventArgs e)
    {
       await Login();
    }
       
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage(_authClient));
    }
}