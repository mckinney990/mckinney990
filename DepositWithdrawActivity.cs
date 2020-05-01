using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    [Activity(Label = "DepositWithdrawActivity")]
    class DepositWithdrawActivity : Activity
    {
        private Button depositwithdraw, send, cancel;
        private Spinner spinner;
        private PaymentMethodAdapter adapter;
        private EditText balance, changed;
        private AccountModel account;
        private UserModel user; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.deposit_withdraw_funds);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            depositwithdraw = FindViewById<Button>(Resource.Id.btn_dw_change_type);
            send = FindViewById<Button>(Resource.Id.btn_dw_send);
            cancel = FindViewById<Button>(Resource.Id.btn_dw_cancel);

            spinner = FindViewById<Spinner>(Resource.Id.sp_dw_payment_method);

            adapter = new PaymentMethodAdapter(this, account.PaymentMethods);
            spinner.Adapter = adapter;

            balance = FindViewById<EditText>(Resource.Id.txt_dw_current_balance);
            balance.Text = String.Format("{0:C}", account.Balance);

            changed = FindViewById<EditText>(Resource.Id.txt_dw_amount_moved);

            send.Click += Send_Click;

            depositwithdraw.Click += (object sender, EventArgs args) =>
            {
                if (depositwithdraw.Text == "Withdraw")
                    depositwithdraw.Text = "Deposit";
                else
                    depositwithdraw.Text = "Withdraw";
            };

            cancel.Click += (object sender, EventArgs args) => {
                Return(); 
            };
        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (account.PaymentMethods == null || account.PaymentMethods.Count == 0)
            {
                Toast.MakeText(this, "Please add a payment method before depositing or withdrawing.", ToastLength.Short).Show();
                return;
            }
            float change;
            try
            {
                float.TryParse(changed.Text, out change);
            }
            catch
            {
                Toast.MakeText(this, "Invalid change amount.", ToastLength.Short).Show();
                return;
            }
            if (depositwithdraw.Text == "Withdraw" && account.Balance - change < 0)
            {
                Toast.MakeText(this, "You cannot remove more from your account that you have.", ToastLength.Short).Show();
            }
            else
            {
                if (depositwithdraw.Text == "Withdraw")
                    account.Balance -= change;
                else
                    account.Balance += change;
                ModelMethods.UpdateAccount(account);
                Return();
            }
        }

        private void Return()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}