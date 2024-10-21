
using CourseWork.Models;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.ObjectModel;
using System.Reactive;

namespace CourseWork
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        YogaClassRepo yogaClassRepo;
        Authen authen;
        CartRepo CartRepo;
        FirebaseAuthClient client;
        CourseRepo courseRepo;
        // only for load Viewmodel only
        ObservableCollection<YogaClassViewModel> yogaClassViewModels;
        // temp string
        string temp;
        // DataBase
        List<YogaClassViewModel> db;
        public MainPage(FirebaseAuthClient client)
        {
            InitializeComponent();
            yogaClassRepo = new YogaClassRepo();
            authen = new Authen(client);
            this.client = client;
            //CounterBtn.Text = ""+ yogaClasses.Count;
            LoadYogaClassViewModel();
            CartRepo = new CartRepo(this);
            courseRepo = new CourseRepo();
           


        }

        //private async void LoadYogaClasses()
        //{
        //    try
        //    {
        //        var yogaClasses = await yogaClassRepo.GetYogaClassesAsync();
        //        YogaClassListView.ItemsSource = yogaClasses;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        Console.WriteLine($"Error retrieving yoga classes: {ex.Message}");
        //    }
        //}
        private async void LoadDataToDb()
        {
            db = new List<YogaClassViewModel>();
          
            // Clear the db list before loading new data
            db.Clear();

            // Add each item from yogaClassViewModels to db
            foreach (var item in yogaClassViewModels)
            {
                db.Add(item);
            }

            await DisplayAlert("Err", "" + db.Count, "ok");
        }
        private async void LoadYogaClassViewModel()
        {
            yogaClassViewModels = new ObservableCollection<YogaClassViewModel>();
            try
            {
                var yogaClasses = await yogaClassRepo.GetYogaClassesAsync();
                foreach (var yogaClass in yogaClasses)
                {
                    var course = await courseRepo.GetCourseWith(yogaClass.CourseID); // Fetch the course data
                    temp = yogaClass.Name;
                    yogaClassViewModels.Add(new YogaClassViewModel
                    {
                        Id = yogaClass.Id,
                        Name = yogaClass.Name,
                        Teacher = yogaClass.Teacher,
                        Comment = yogaClass.Comment,
                        Date = yogaClass.Date,
                        CurrentCapability = yogaClass.CurrentCapability,
                        CourseID = yogaClass.CourseID,
                        CourseName = course.Name,
                        PricePerClass = course.PricePerClass
                    });
                }

                YogaClassListView.ItemsSource = yogaClassViewModels; // Bind the data to the ListView
                LoadDataToDb();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load yoga classes: {ex.Message}", "OK");
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
                    //Cart c1 = new Cart();
                    //c1.yogaClassIDs = new List<int> { id };
                    await CartRepo.AddItemToCart(authen.GetCurrentUserEmail().ToString(), id);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("FAIL", ex.Message, "OK");
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartView(client));
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            try
            {
                if (yogaClassViewModels == null || !yogaClassViewModels.Any())
                {
                    await DisplayAlert("Error", "No yoga classes available to search.", "OK");
                    return;
                }

                var query = SearchBar.Text?.ToLower();
                if (query != null)
                {
                    var filteredClasses = yogaClassViewModels.Where(c => c.Date.ToLower().Contains(query)).ToList();
                    if (filteredClasses.Any())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            YogaClassListView.ItemsSource = new ObservableCollection<YogaClassViewModel>(filteredClasses);
                        });
                        await DisplayAlert("Info", "Item: " + filteredClasses.Count, "OK");
                    }
                    else
                    {
                        await DisplayAlert("Info", "No yoga classes found matching the search query.", "OK");
                    }
                }
                else
                {
                    LoadYogaClassViewModel();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void Button_Clicked_3(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null)
                {
                    var id = (int)button.CommandParameter;
                    await Navigation.PushAsync(new ViewDetail(id, authen));
                }
            }
            catch(Exception ex) {
                await DisplayAlert("Error", ex.Message, "OK");

            }
        }
    }
}
