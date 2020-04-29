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
using MongoDB.Driver;

namespace MulliganWallet
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity
    {
        private AccountModel account;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            GetAccount();
        }

        private async void GetAccount()
        {
            account = await ModelMethods.FindAccountByUserID(Intent.GetStringExtra("UserID"));
        }
    }
}