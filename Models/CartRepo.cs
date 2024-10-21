using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Models
{

    class CartRepo
    {
        private readonly FirebaseClient firebaseClient;
        private readonly PurchaseLogRepo purchaseLogRepo;
        private readonly string TABLE_NAME = "cart";
        private readonly string DATABASE_LINK = "https://yogaapplicationapp-default-rtdb.asia-southeast1.firebasedatabase.app/";
        private readonly Page page;
        public CartRepo(Page page)
        {
            firebaseClient = new FirebaseClient(DATABASE_LINK);
            purchaseLogRepo = new PurchaseLogRepo(page);
            this.page = page;
        }
        public async Task AddCart(string userEmail, Cart cart)
        {
                userEmail = userEmail.Replace(".", "_").Replace("@", "_at_");
            // Add the cart to Firebase
            await firebaseClient
                    .Child(TABLE_NAME)
                    .Child(userEmail)
                    .PutAsync(cart);
           
        }
        public async Task<Cart> GetCart(string email)
        {
            email = email.Replace(".", "_").Replace("@", "_at_");
            var cart = await firebaseClient
                .Child(TABLE_NAME)
                .Child(email)
                .OnceSingleAsync<Cart>();
            return cart;
        }
        public async Task AddItemToCart(string email, int id)
        {
            try
            {
                // Get the existing cart
                var cart = await GetCart(email);
                var logs = await purchaseLogRepo.GetPurchaseLog(email);
                // get cart
                if (cart == null)
                {
                    cart = new Cart { yogaClassIDs = new List<int>() };
                }
                if(logs == null)
                {
                    logs = new PurchaseLog { yogaClassIDsBooked = new List<int>() };
                }
                

                // add item to cart
                if (!cart.yogaClassIDs.Contains(id) && !logs.yogaClassIDsBooked.Contains(id))
                {
                    // Add the new item to the cart
                    cart.yogaClassIDs.Add(id);

                    // Update the cart in Firebase
                    await AddCart(email, cart);
                    await page.DisplayAlert("Success", "Add to cart Success", "OK");
                }
                else
                {
                    await page.DisplayAlert("Error", "You already add this course please try again", "OK");
                }
            }
            catch (Exception ex)
            {
               await page.DisplayAlert("Err", ex.Message, "OK");
            }
        }
        public async void RemoveItemFromCart(string email,int yogaID)
        {
            try
            {
                // Get the existing cart
                var cart = await GetCart(email);
                if (cart != null && cart.yogaClassIDs.Contains(yogaID))
                {
                    // Remove the item from the cart
                    cart.yogaClassIDs.Remove(yogaID);

                    // Update the cart in Firebase
                    await AddCart(email, cart);
                    await page.DisplayAlert("Success", "Item removed from cart", "OK");
                }
                else
                {
                    await page.DisplayAlert("Error", "Item not found in cart", "OK");
                }
            }
            catch (Exception ex) { 
                await page.DisplayAlert("Error",ex.Message, "OK");  
            }
        }
        public async Task DeleteAllItemsFromCart(string email)
        {
            email = email.Replace(".", "_").Replace("@", "_at_");
            var cart = await GetCart(email);
            if (cart != null)
            {
                cart.yogaClassIDs.Clear();
                await AddCart(email, cart);
                await page.DisplayAlert("Success", "All items removed from cart", "OK");
            }
            else
            {
                await page.DisplayAlert("Error", "Cart not found", "OK");
            }
        }

    }
}

