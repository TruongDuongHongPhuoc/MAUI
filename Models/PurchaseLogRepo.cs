using CourseWork.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork
{
    class PurchaseLogRepo
    {
        private readonly FirebaseClient firebaseClient;
        private readonly string TABLE_NAME = "purchaseLog";
        private readonly string DATABASE_LINK = "/";
        private readonly Page page;
        public PurchaseLogRepo(Page page)
        {
            firebaseClient = new FirebaseClient(DATABASE_LINK);
            this.page = page;
        }
        public async Task AddPurchaseLog(string userEmail, PurchaseLog log)
        {
            userEmail = userEmail.Replace(".", "_").Replace("@", "_at_");
            // Add the cart to Firebase
            await firebaseClient
                    .Child(TABLE_NAME)
                    .Child(userEmail)
                    .PutAsync(log);

        }
        public async Task<PurchaseLog> GetPurchaseLog(string email)
        {
            email = email.Replace(".", "_").Replace("@", "_at_");
            var log = await firebaseClient
                .Child(TABLE_NAME)
                .Child(email)
                .OnceSingleAsync<PurchaseLog>();
            return log;
        }
        public async Task AddItemToLog(string email, int id)
        {
            try
            {
                // Get the existing cart
                var log = await GetPurchaseLog(email);
                if (log == null)
                {
                    log = new PurchaseLog { yogaClassIDsBooked = new List<int>() };
                }
                if (!log.yogaClassIDsBooked.Contains(id))
                {
                    // Add the new item to the cart
                    log.yogaClassIDsBooked.Add(id);

                    // Update the cart in Firebase
                    await AddPurchaseLog(email, log);
                    await page.DisplayAlert("Success", "add to purchase Log success", "OK");
                }
                else
                {
                    await page.DisplayAlert("Error", "You already add this purchase log please try again", "OK");
                }
            }
            catch (Exception ex)
            {
                await page.DisplayAlert("Err", ex.Message, "OK");
            }
        }
        public async void RemoveItemFromCart(string email, int yogaID)
        {
            // Get the existing cart
            var log = await GetPurchaseLog(email);
            if (log != null && log.yogaClassIDsBooked.Contains(yogaID))
            {
                // Remove the item from the cart
                log.yogaClassIDsBooked.Remove(yogaID);

                // Update the cart in Firebase
                await AddPurchaseLog(email, log);
                await page.DisplayAlert("Success", "Item removed from Purchase Log", "OK");
            }
            else
            {
                await page.DisplayAlert("Error", "Item not found in Purchase Log", "OK");
            }
        }
    }
}
