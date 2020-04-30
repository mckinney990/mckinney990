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
        private Button depositwithdraw, makeTransaction, viewNotifications, qrReader, btnViewProfile, viewHistory, viewFriends;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            balance = FindViewById<TextView>(Resource.Id.MainMenuBalance);
            fullname = FindViewById<TextView>(Resource.Id.MainMenuFullName);
            balance.Text = String.Format("Balance:\n{0:C}", Intent.GetFloatExtra("Balance", 0));
            fullname.Text = Intent.GetStringExtra("FullName");

            btnViewProfile = FindViewById<Button>(Resource.Id.btn_main_view_profile);
            btnViewProfile.Click += BtnViewProfile_Click;

            depositwithdraw = FindViewById<Button>(Resource.Id.btn_main_deposit_withdraw);
            depositwithdraw.Click += Depositwithdraw_Click;
        }

        private void Depositwithdraw_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(DepositWithdrawActivity));
            intent.PutExtra("PersonID", Intent.GetStringExtra("PersonID"));
            intent.PutExtra("Balance", Intent.GetFloatExtra("Balance", 0));
            this.StartActivityForResult(intent, 0);
        }

        private async void BtnViewProfile_Click(object sender, EventArgs e)
        {
            BsonObjectId ID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));

            var result = await ModelMethods.FindUserByID(ID);

            if (result != null)
            {
                Intent intent = new Intent(this, typeof(ProfileActivity));
                intent.PutExtra("FullName", result.FullName);
                intent.PutExtra("Username", result.Username);
                intent.PutExtra("Email", result.Email);
                intent.PutExtra("PhoneNumber", result.PhoneNumber);
                intent.PutExtra("PersonID", Intent.GetStringExtra("PersonID"));
                this.StartActivity(intent);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == 0)
                if (resultCode == Result.Ok)
                    balance.Text = data.GetFloatExtra("Balance", 0.0f).ToString();
        }
    }
}