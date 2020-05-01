using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private Button depositwithdraw, makeTransaction, viewNotifications, qrReader, btnViewProfile, viewHistory, viewFriends, exit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            balance = FindViewById<TextView>(Resource.Id.MainMenuBalance);
            fullname = FindViewById<TextView>(Resource.Id.MainMenuFullName);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            balance.Text = String.Format("Balance:\n{0:C}", account.Balance);
            fullname.Text = Intent.GetStringExtra(user.FullName);

            btnViewProfile = FindViewById<Button>(Resource.Id.btn_main_view_profile);
            btnViewProfile.Click += BtnViewProfile_Click;

            depositwithdraw = FindViewById<Button>(Resource.Id.btn_main_deposit_withdraw);
            depositwithdraw.Click += Depositwithdraw_Click;

            makeTransaction = FindViewById<Button>(Resource.Id.btn_main_make_transaction);
            makeTransaction.Click += MakeTransaction_Click;

            viewNotifications = FindViewById<Button>(Resource.Id.btn_main_notifications);
            viewNotifications.Click += (object sender, EventArgs e) =>
            {
                Toast.MakeText(this, "This is where we'd implement notifications.", ToastLength.Short).Show();
            };

            qrReader = FindViewById<Button>(Resource.Id.btn_main_qr_reader);
            qrReader.Click += (object sender, EventArgs e) =>
            {
                Toast.MakeText(this, "This is where we'd implement the qr reader.", ToastLength.Short).Show();
            };

            viewHistory = FindViewById<Button>(Resource.Id.btn_main_history);
            viewHistory.Click += ViewHistory_Click;

            viewFriends = FindViewById<Button>(Resource.Id.btn_main_friends);
            viewFriends.Click += ViewFriends_Click;

            exit = FindViewById<Button>(Resource.Id.btn_exit_main_menu);
            exit.Click += (object sender, EventArgs e) => { Finish(); };
        }

        private async void ViewFriends_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(FriendListActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            List<UserModel> friends = await ModelMethods.GetListOfFriendsByAccount(account);
            intent.PutExtra("Friends", friends.ToJson());
            List<AccountModel> friendAccounds = await ModelMethods.GetListOfAccountsByListOfUsers(friends);
            intent.PutExtra("FriendAccounts", friendAccounds.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private async void ViewHistory_Click(object sender, EventArgs e)
        {
            var transactions = await ModelMethods.GetTransactionsInvolvingAccount(account);
            Intent intent = new Intent(this, typeof(TransactionHistoryActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Transactions", transactions.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void MakeTransaction_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TransactionMakeActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void Depositwithdraw_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(DepositWithdrawActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void BtnViewProfile_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ProfileActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == 0)
                if (resultCode == Result.Ok)
                {
                    RunOnUiThread(() =>
                    {
                        balance.Text = data.GetFloatExtra("Balance", 0.0f).ToString();
                        balance.PostInvalidate();
                    });
                }
        }
    }
}