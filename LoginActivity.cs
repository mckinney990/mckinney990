using System;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace MulliganWallet
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText txtUserID, txtPassword;
        private Button btnLogin, btnForgotPassword, btnCancel;
        private LinearLayout layout;
        private ProgressBar progress;
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

            progress = FindViewById<ProgressBar>(Resource.Id.loginProgress);
            progress.Visibility = ViewStates.Invisible;

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

            btnCancel.Click += (object sender, EventArgs args) => { Finish(); };
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            progress.Visibility = ViewStates.Visible;
            String userid = txtUserID.Text;
            String password = txtPassword.Text;
            var result = await ModelMethods.FindUserByUserID(userid);
            progress.Visibility = ViewStates.Invisible;
            if (result != null && result.Password == password)
            {
                var account = await ModelMethods.FindAccountByPersonID(result.Id);
                Toast.MakeText(this, "Login Successful", ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra("Account", account.ToJson());
                intent.PutExtra("User", result.ToJson());
                this.StartActivity(intent);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Login failed.", ToastLength.Short).Show();
            }
        }
    }

}