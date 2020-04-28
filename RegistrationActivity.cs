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

namespace MulliganWallet
{
    [Activity(Label = "RegistrationActivity")]
    public class RegistrationActivity : Activity
    {
        private EditText txtFullName, txtUsername, txtEmail, txtPhoneNumber, txtPassword, txtConfirmPassword;
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

            txtPhoneNumber.AddTextChangedListener(new PhoneNumberFormattingTextWatcher());

            layout = FindViewById<LinearLayout>(Resource.Id.RegistrationLayout);
            layout.Click += (object sender, EventArgs args) =>
            {
                InputMethodManager manager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
                manager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
            };

        }
    }
}