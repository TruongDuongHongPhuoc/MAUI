using CourseWork.Models;
using System.Collections.ObjectModel;

namespace CourseWork;

public partial class PurchaseLogView : ContentPage
{
	private readonly Authen authen;
	private readonly PurchaseLogRepo purchaseRepo;
	private readonly YogaClassRepo yogaClassRepo;
    private readonly CourseRepo courseRepo;
	public PurchaseLogView(Authen authen)
	{
		InitializeComponent();
		this.authen = authen;

        purchaseRepo = new PurchaseLogRepo(this);
        yogaClassRepo = new YogaClassRepo();
        courseRepo = new CourseRepo();
        start();
	}
	private async void start()
	{
		List<PurchaseLogViewModel> purchaseLogViewModels = new List<PurchaseLogViewModel>();
		var log = await purchaseRepo.GetPurchaseLog(authen.GetCurrentUserEmail());
		if(log != null)
        {
            List<int> classIDS = log.yogaClassIDsBooked;
            foreach (int classID in classIDS) {
                YogaClass yogaClass = await yogaClassRepo.GetYogaClassWith(classID);
                Course course = await courseRepo.GetCourseWith(yogaClass.CourseID);
                purchaseLogViewModels.Add(new PurchaseLogViewModel
                {
                    Name = yogaClass.Name,
                    Date = yogaClass.Date,
                    Teacher = yogaClass.Teacher,
                    Price = course.PricePerClass
                });
            }
            PurchaseLogListView.ItemsSource = purchaseLogViewModels;
        }

    }
    //private async void start()
    //{
    //    float price = 0;
    //    classes = new ObservableCollection<YogaClassViewModel>();
    //    var cart = await cartRepo.GetCart(authen.GetCurrentUserEmail()); // Await the GetCart method
    //    if (cart != null)
    //    {
    //        List<int> classIDs = cart.yogaClassIDs;
    //        foreach (int classID in classIDs)
    //        {
    //            YogaClass yogaClass = await yogaClassRepo.GetYogaClassWith(classID);
    //            Course course = await courseRepo.GetCourseWith(yogaClass.CourseID); // Fetch the course data
    //            price += course.PricePerClass;
    //            classes.Add(new YogaClassViewModel
    //            {
    //                Id = yogaClass.Id,
    //                Name = yogaClass.Name,
    //                Teacher = yogaClass.Teacher,
    //                Comment = yogaClass.Comment,
    //                Date = yogaClass.Date,
    //                CurrentCapability = yogaClass.CurrentCapability,
    //                CourseID = yogaClass.CourseID,
    //                CourseName = course.Name,
    //                DayOfWeek = course.DaysOfWeek,
    //                PricePerClass = course.PricePerClass
    //            });
    //        }
    //        YogaClassList.ItemsSource = classes; // Use ObservableCollection for data binding
    //        btnBuy.Text = price.ToString();
    //    }
    //    else
    //    {
    //        await DisplayAlert("Info", "No items in the cart.", "OK");
    //    }
    //}

}