using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            balance = FindViewById<TextView>(Resource.Id.MainMenuBalance);
            fullname = FindViewById<TextView>(Resource.Id.MainMenuFullName);
            //Task.Run(() => GetData(Intent.GetStringExtra("UserID")));
        }

        private async void GetData(String id)
        {
            account = await ModelMethods.FindAccountByUserID(id);
            user = await ModelMethods.FindUserByID(account.PersonID);
            balance.Text = String.Format("Balance:\n{0:C}", account.Balance);
            fullname.Text = user.FullName;
        }
    }
}