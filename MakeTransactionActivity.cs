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
    [Activity(Label = "MakeTransactionActivity")]
    public class MakeTransactionActivity : Activity
    {
        private Button btnCancel, btnSend, btnSave, btnTransactionType;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.transaction_create);
            // Create your application here
            btnTransactionType = FindViewById<Button>(Resource.Id.btnTransactionType);
            btnTransactionType.Click += btnTransactionType_Click;

            btnSave = FindViewById<Button>(Resource.Id.btnSaveTransaction);
            btnSave.Click += btnSave_Click;

            btnSend = FindViewById<Button>(Resource.Id.btnSendTransaction);
            btnSend.Click += btnSend_Click;

            btnCancel = FindViewById<Button>(Resource.Id.btnCancelTransaction);
            btnCancel.Click += btnCancel_Click;

        }
        private void btnCancel_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
        }
        private void btnSend_Click(object sender, System.EventArgs e)
        {

            //Intent intent = new Intent(this, typeof(MainActivity));
            //this.StartActivity(intent);
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {

            //Intent intent = new Intent(this, typeof(MainActivity));
            //this.StartActivity(intent);
        }
        private void btnTransactionType_Click(object sender, System.EventArgs e)
        {

            //Intent intent = new Intent(this, typeof(MainActivity));
            //this.StartActivity(intent);
        }
    }
}