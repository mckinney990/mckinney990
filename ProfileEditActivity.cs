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
    [Activity(Label = "ProfileEditActivity")]
    public class ProfileEditActivity : Activity
    {
        private EditText FullName, Username, Email, PhoneNumber;
        private Button Save, Cancel;
        private AccountModel account;
        private UserModel user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profile_edit);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            FullName = FindViewById<EditText>(Resource.Id.txt_profile_edit_full_name);
            Username = FindViewById<EditText>(Resource.Id.txt_profile_edit_username);
            Email = FindViewById<EditText>(Resource.Id.txt_profile_edit_email);
            PhoneNumber = FindViewById<EditText>(Resource.Id.txt_profile_edit_phone_number);

            Save = FindViewById<Button>(Resource.Id.btn_profile_edit_send);
            Cancel = FindViewById<Button>(Resource.Id.btn_profile_edit_cancel);

            FullName.Text = user.FullName;
            Username.Text = user.Username;
            Email.Text = user.Email;
            PhoneNumber.Text = user.PhoneNumber;

            Save.Click += Save_Click;
            Cancel.Click += Cancel_Click;

        }

        private void Save_Click(object sender, EventArgs e)
        {
            user.FullName = FullName.Text;
            user.Username = Username.Text;
            user.Email = Email.Text;
            user.PhoneNumber = PhoneNumber.Text;
            ModelMethods.UpdateUserProfile(user);
            Back_To_Profile();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Back_To_Profile();
        }

        private void Back_To_Profile()
        {
            Intent intent = new Intent(this, typeof(ProfileActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}