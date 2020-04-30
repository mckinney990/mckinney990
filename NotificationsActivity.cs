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
   
  [Activity(Label = "NotificationsActivity")]
    public class NotificationsActivity : Activity
    {
        private Button btnExit, btnClear;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.notifications);
            // Create your application here

            btnExit = FindViewById<Button>(Resource.Id.btn_exit_notifications);
            btnExit.Click += btnExit_Click;

            btnClear = FindViewById<Button>(Resource.Id.btn_clear_notifications);
            btnClear.Click += btnClear_Click;
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }
    }
}