using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MulliganWallet
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity
    {
        private AccountModel account;
        private UserModel user;
        private TextView balance, fullname;
        private Button btnGoToNotifications, btnGoToMakeTransaction, btnGoToProfile, btnGoToHistory, btnGoToFriends;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            balance = FindViewById<TextView>(Resource.Id.MainMenuBalance);
            fullname = FindViewById<TextView>(Resource.Id.MainMenuFullName);
            //Task.Run(() => GetData(Intent.GetStringExtra("UserID")));

            btnGoToNotifications = FindViewById<Button>(Resource.Id.btnNotificationsFromMain);
            btnGoToNotifications.Click += btnGoToNotifications_Click;

            btnGoToFriends = FindViewById<Button>(Resource.Id.btnFriendsFromMain);
            btnGoToFriends.Click += btnGoToFriends_Click;

            btnGoToHistory = FindViewById<Button>(Resource.Id.btnHistoryFromMain);
            btnGoToHistory.Click += btnGoToHistory_Click;

            btnGoToMakeTransaction = FindViewById<Button>(Resource.Id.btnMakeTransactionFromMain);
            btnGoToMakeTransaction.Click += btnGoToMakeTransaction_Click;

            btnGoToProfile  = FindViewById<Button>(Resource.Id.btnProfileFromMain);
            btnGoToProfile.Click += btnGoToProfile_Click;
        }

        private async void GetData(String id)
        {
            account = await ModelMethods.FindAccountByUserID(id);
            user = await ModelMethods.FindUserByID(account.PersonID);
            balance.Text = String.Format("Balance:\n{0:C}", account.Balance);
            fullname.Text = user.FullName;
        }

        private void btnGoToNotifications_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(NotificationsActivity));
            this.StartActivity(intent);
        }
        private void btnGoToFriends_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(FriendActivity));
            this.StartActivity(intent);
        }
        private void btnGoToHistory_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(HistoryActivity));
            this.StartActivity(intent);
        }
        private void btnGoToMakeTransaction_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }
        private void btnGoToProfile_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }
    }
}