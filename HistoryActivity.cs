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
    [Activity(Label = "HistoryActivity")]
    public class HistoryActivity : Activity
    {
        private Button btnExit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.transaction_history);
            // Create your application here


            btnExit = FindViewById<Button>(Resource.Id.btnGotoMainFromHistory);
            btnExit.Click += btnExit_Click;
        }
        private void btnExit_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }

    }
}