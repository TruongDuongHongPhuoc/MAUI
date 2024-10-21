using CourseWork.Models;

using Type = CourseWork.Models.Type;

namespace CourseWork;

public partial class ViewDetail : ContentPage
{
    YogaClassRepo yogaClassRepo;
    CourseRepo courseRepo;
    TypeRepo typeRepo;
    YogaClass yoga;
    Course course;
    Type type;
    CartRepo cartRepo;
    // 
    Authen authent;
    // 
    int itemID;

    public ViewDetail(int id, Authen authent)
    {
        InitializeComponent();
        courseRepo = new CourseRepo();
        yogaClassRepo = new YogaClassRepo();
        typeRepo = new TypeRepo();
        cartRepo = new CartRepo(this);
        LoadData(id);
        this.authent = authent;
        this.itemID = id;
    }

    private async void LoadData(int id)
    {
        try
        {
            yoga = await yogaClassRepo.GetYogaClassWith(id);
            if (yoga != null)
            {
                course = await courseRepo.GetCourseWith(yoga.CourseID);
                if (course != null)
                {
                    type = await typeRepo.GetTypeWith(course.TypeID);
                    if (type != null)
                    {
                        // Set the BindingContext to a new object containing all the data
                        BindingContext = new
                        {
                            Yoga = yoga,
                            Course = course,
                            Type = type
                        };
                    }
                    else
                    {
                        // Handle case where type is not found
                        DisplayAlert("Error", $"Type with ID {course.TypeID} not found.", "OK");
                    }
                }
                else
                {
                    // Handle case where course is not found
                    DisplayAlert("Error", $"Course with ID {yoga.CourseID} not found.", "OK");
                }
            }
            else
            {
                // Handle case where yoga class is not found
                DisplayAlert("Error", $"YogaClass with ID {id} not found.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            DisplayAlert("Error", $"Error retrieving data: {ex.Message}", "OK");
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            await cartRepo.AddItemToCart(authent.GetCurrentUserEmail(), itemID);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}

