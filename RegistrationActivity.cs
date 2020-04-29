using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Android.Telephony;
using MongoDB.Driver;

namespace MulliganWallet
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : Activity
    {
        private EditText txtFullName, txtUsername, txtEmail, txtPhoneNumber, txtPassword, txtConfirmPassword;
        private Button btnRegister, btnCancel;
        private LinearLayout layout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.registration);

            txtFullName = FindViewById<EditText>(Resource.Id.txtRegisterFullName);
            txtUsername = FindViewById<EditText>(Resource.Id.txtRegisterUsername);
            txtEmail = FindViewById<EditText>(Resource.Id.txtRegisterEmail);
            txtPhoneNumber = FindViewById<EditText>(Resource.Id.txtRegisterPhoneNumber);
            txtPassword = FindViewById<EditText>(Resource.Id.txtRegisterPassword);
            txtConfirmPassword = FindViewById<EditText>(Resource.Id.txtRegisterConfirmPassword);

            btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancelRegistration);

            txtPhoneNumber.AddTextChangedListener(new PhoneNumberFormattingTextWatcher());

            layout = FindViewById<LinearLayout>(Resource.Id.RegistrationLayout);
            layout.Click += (object sender, EventArgs e) =>
            {
                InputMethodManager manager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
                manager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
            };

            btnRegister.Click += BtnRegister_Click;

            btnCancel.Click += (object sender, EventArgs e) =>
            {
                Finish();
            };

        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            String FullName = txtFullName.Text;
            String Username = txtUsername.Text;
            String Email = txtEmail.Text;
            String PhoneNumber = PhoneNumberUtils.NormalizeNumber(txtPhoneNumber.Text);
            String Password = txtPassword.Text;
            String Confirm = txtConfirmPassword.Text;

            if (Password != Confirm)
            {
                Toast.MakeText(this, "The passwords provided do not match. Please try again.", ToastLength.Short).Show();
            }
            else
            {
                ModelMethods.CreateUser(FullName, Username, Email, PhoneNumber, Password);
                Toast.MakeText(this, "Account created. Please try to login now.", ToastLength.Short).Show();
                Finish();
            }

        }
    }
}