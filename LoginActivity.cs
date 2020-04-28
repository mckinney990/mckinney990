using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using Newtonsoft.Json;

namespace MulliganWallet
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText txtUserID, txtPassword;
        private Button btnLogin, btnForgotPassword, btnCancel;
        private List<LoginItem> loginItems;
        private LinearLayout layout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);

            txtUserID = FindViewById<EditText>(Resource.Id.txtLoginUserID);
            txtPassword = FindViewById<EditText>(Resource.Id.txtLoginPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnDoLogin);
            btnForgotPassword = FindViewById<Button>(Resource.Id.btnForgotPassword);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancelLogin);
            layout = FindViewById<LinearLayout>(Resource.Id.LoginLayout);

            AssetManager assets = this.Assets;
            using (StreamReader reader = new StreamReader(assets.Open("dummy_logins.json")))
            {
                loginItems = JsonConvert.DeserializeObject<List<LoginItem>>(reader.ReadToEnd());
            }

            layout.Click += (object sender, EventArgs e) =>
            {
                InputMethodManager manager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
                manager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
            };

            btnLogin.Click += BtnLogin_Click;

            btnForgotPassword.Click += (object sender, EventArgs e) =>
            {
                if (txtUserID.Text == null || txtUserID.Text == String.Empty)
                {
                    Toast.MakeText(this, "Please enter a user ID to send the password reset email to.", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Email sent to the account asscoiated with that user ID.", ToastLength.Long).Show();
                    Finish();
                }
            };

            btnCancel.Click += (object sender, EventArgs args) => 
            {
                txtUserID.Text = String.Empty;
                txtPassword.Text = String.Empty;
                Finish();
            };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            String userid = txtUserID.Text;
            String password = txtPassword.Text;
            foreach (LoginItem item in loginItems)
            {
                if (userid == item.Username || userid == item.Email || userid == item.PhoneNumber)
                {
                    if (password != item.Password)
                        break;
                    Toast.MakeText(this, "Login successful.", ToastLength.Short).Show();
                    Intent intent = new Intent(this, typeof(MainMenuActivity));
                    this.StartActivity(intent);
                    return;
                }
            }
            Toast.MakeText(this, "Login failed.", ToastLength.Short).Show();
        }

    }
}