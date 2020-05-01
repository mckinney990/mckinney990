using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace MulliganWallet
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity
    {
        private TextView fullName, username, email, phoneNumber;
        private Button btnEdit, btnChangePassword, btnShowQR, btnPaymentMethods, btnSavedTransactions, btnExit;
        private AccountModel account;
        private UserModel user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profile);

            fullName = FindViewById<TextView>(Resource.Id.txtProfileName);
            username = FindViewById<TextView>(Resource.Id.txtProfileUsername);
            email = FindViewById<TextView>(Resource.Id.txtProfileEmail);
            phoneNumber = FindViewById<TextView>(Resource.Id.txtProfilePhoneNumber);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            fullName.Text = user.FullName;
            username.Text = user.Username;
            email.Text = user.Email;
            phoneNumber.Text = user.PhoneNumber;

            btnEdit = FindViewById<Button>(Resource.Id.btnEditProfile);
            btnChangePassword = FindViewById<Button>(Resource.Id.btnChangePassword);
            btnShowQR = FindViewById<Button>(Resource.Id.btnShowProfileQR);
            btnPaymentMethods = FindViewById<Button>(Resource.Id.btnPaymentMethods);
            btnSavedTransactions = FindViewById<Button>(Resource.Id.btnSavedTransactions);
            btnExit = FindViewById<Button>(Resource.Id.btnExitProfile);

            btnEdit.Click += BtnEdit_Click;
            btnChangePassword.Click += BtnChangePassword_Click;
            btnShowQR.Click += BtnShowQR_Click;
            btnPaymentMethods.Click += BtnPaymentMethods_Click;
            btnSavedTransactions.Click += BtnSavedTransactions_Click;
            btnExit.Click += (object sender, EventArgs args) => { Back_To_Main_Activity(); };
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ProfileEditActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "An email has been sent to the address on file with the steps to change your password.", ToastLength.Short).Show();
        }

        private void BtnShowQR_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "This is where we'd show a QR code for your profile if this were on a physical device.", ToastLength.Short).Show();
        }

        private void BtnPaymentMethods_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PaymentMethodActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void BtnSavedTransactions_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "This is where we'd show your saved transactions.", ToastLength.Short).Show();
        }

        private void Back_To_Main_Activity()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}