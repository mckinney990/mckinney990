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
    [Activity(Label = "FriendActivity")]
    public class FriendActivity : Activity
    {
        private Button btnAdd,btnRemove,btnExit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friend_list);
            // Create your application here

            btnAdd = FindViewById<Button>(Resource.Id.btn_add_friend);
            btnAdd.Click += btnExit_Click;
            btnRemove = FindViewById<Button>(Resource.Id.btn_remove_friend);
            btnRemove.Click += btnExit_Click;
            btnExit = FindViewById<Button>(Resource.Id.btn_exit_payment_methods);
            btnExit.Click += btnExit_Click;
        }
        private void btnRemove_Click(object sender, System.EventArgs e)
        {

           // Intent intent = new Intent(this, typeof(MainActivity));
           // this.StartActivity(intent);
        }
        private void btnAdd_Click(object sender, System.EventArgs e)
        {

           // Intent intent = new Intent(this, typeof(MainActivity));
           //this.StartActivity(intent);
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }
    }
}