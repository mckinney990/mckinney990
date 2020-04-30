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
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profile);
            // Create your application here

        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }

        private void btnSavedTransactions_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }

        private void btnPaymentMethods_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }

        private void btnShowQR_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }

        private void btnChangePassword_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }

        private void btnEditProfile_Click(object sender, System.EventArgs e)
        {

            Intent intent = new Intent(this, typeof(MakeTransactionActivity));
            this.StartActivity(intent);
        }
    }
}