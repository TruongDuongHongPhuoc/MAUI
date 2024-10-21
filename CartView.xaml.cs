
using CourseWork.Models;
using Firebase.Auth;
using System.Collections.ObjectModel;


namespace CourseWork;

public partial class CartView : ContentPage
{
    private readonly CartRepo cartRepo;
    private readonly Authen authen;
    private readonly CourseRepo courseRepo;
    private readonly YogaClassRepo yogaClassRepo;
    private readonly PurchaseLogRepo purchaseLogRepo;
    //private readonly Authen authen;
    ObservableCollection<YogaClassViewModel> classes;
    public CartView(FirebaseAuthClient client)
	{
        //BUG KEMETAO
		InitializeComponent();
        cartRepo = new CartRepo(this);
        courseRepo = new CourseRepo();
        yogaClassRepo = new YogaClassRepo();
        purchaseLogRepo = new PurchaseLogRepo(this);
        authen = new Authen(client);
        start();
    }
    private async void start()
    {
        float price =0;
       classes = new ObservableCollection<YogaClassViewModel>();
        var cart = await cartRepo.GetCart(authen.GetCurrentUserEmail()); // Await the GetCart method
        if (cart != null)
        {
            List<int> classIDs = cart.yogaClassIDs;
            foreach (int classID in classIDs)
            {
                YogaClass yogaClass = await yogaClassRepo.GetYogaClassWith(classID);
                Course course = await courseRepo.GetCourseWith(yogaClass.CourseID); // Fetch the course data
                price += course.PricePerClass;
                classes.Add(new YogaClassViewModel
                {
                    Id = yogaClass.Id,
                    Name = yogaClass.Name,
                    Teacher = yogaClass.Teacher,
                    Comment = yogaClass.Comment,
                    Date = yogaClass.Date,
                    CurrentCapability = yogaClass.CurrentCapability,
                    CourseID = yogaClass.CourseID,
                    CourseName = course.Name,
                    DayOfWeek = course.DaysOfWeek,
                    PricePerClass = course.PricePerClass
                });
            }
            YogaClassList.ItemsSource = classes; // Use ObservableCollection for data binding
            btnBuy.Text = price.ToString();
        }
        else
        {
            await DisplayAlert("Info", "No items in the cart.", "OK");
        }
    }   

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            if (button != null)
            {
                var id = (int)button.CommandParameter;
                cartRepo.RemoveItemFromCart(authen.GetCurrentUserEmail(), id);
                RemoveItemFromClass(id);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("FAIL", ex.Message, "OK");
        }
    }
    private void RemoveItemFromClass(int id)
    {
        var itemToRemove = classes.FirstOrDefault(c => c.Id == id);
        if (itemToRemove != null)
        {
            classes.Remove(itemToRemove);
        }

    }

    private async void btnBuy_Clicked(object sender, EventArgs e)
    {
        try
        {
            List<int> booked = new List<int>();
            foreach (YogaClassViewModel item in classes)
            {
                await purchaseLogRepo.AddItemToLog(authen.GetCurrentUserEmail(), item.Id);
            }
            classes.Clear();
            await cartRepo.DeleteAllItemsFromCart(authen.GetCurrentUserEmail());
            await DisplayAlert("Success", "Your have booked the classes", "OK");
        }
        catch (Exception ex) {
            await DisplayAlert("Error", "Your Book have problem please try later", "OK");
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PurchaseLogView(authen));
    }
}