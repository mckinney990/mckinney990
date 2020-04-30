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

namespace MulliganWallet
{
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity
    {
        private TextView fullName, username, email, phoneNumber;
        private Button btnEdit, btnChangePassword, btnShowQR, btnPaymentMethods, btnSavedTransactions, btnExit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profile);
            fullName = FindViewById<TextView>(Resource.Id.txtProfileName);
            username = FindViewById<TextView>(Resource.Id.txtProfileUsername);
            email = FindViewById<TextView>(Resource.Id.txtProfileEmail);
            phoneNumber = FindViewById<TextView>(Resource.Id.txtProfilePhoneNumber);

            fullName.Text = Intent.GetStringExtra("FullName");
            username.Text = Intent.GetStringExtra("Username");
            email.Text = Intent.GetStringExtra("Email");
            phoneNumber.Text = Intent.GetStringExtra("PhoneNumber");

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
            btnExit.Click += (object sender, EventArgs args) => { Finish(); };
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "An email has been sent to the address on file with the steps to change your password.", ToastLength.Long).Show();
        }

        private void BtnShowQR_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "This is where we'd show a QR code for your profile if this were on a physical device.", ToastLength.Short).Show();
        }

        private void BtnPaymentMethods_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PaymentMethodActivity));
            intent.PutExtra("PersonID", Intent.GetStringExtra("PersonID"));
            this.StartActivity(intent);
        }

        private void BtnSavedTransactions_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}