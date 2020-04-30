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

namespace MulliganWallet
{
    [Activity(Label = "DepositWithdrawActivity")]
    class DepositWithdrawActivity : Activity
    {
        private Button depositwithdraw, send, cancel, refresh;
        private Spinner spinner;
        private PaymentMethodAdapter adapter;
        private List<PaymentModel> models;
        private SynchronizationContext sc;
        private EditText balance, amt_change;
        float current_balance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.deposit_withdraw_funds);
            depositwithdraw = FindViewById<Button>(Resource.Id.btn_dw_change_type);
            send = FindViewById<Button>(Resource.Id.btn_dw_send);
            cancel = FindViewById<Button>(Resource.Id.btn_dw_cancel);
            spinner = FindViewById<Spinner>(Resource.Id.sp_dw_payment_method);
            refresh = FindViewById<Button>(Resource.Id.btn_dw_refresh_payment_methods);
            models = new List<PaymentModel>();
            adapter = new PaymentMethodAdapter(this, models);
            spinner.Adapter = adapter;
            sc = SynchronizationContext.Current;

            current_balance = Intent.GetFloatExtra("Balance", 0.0f);
            balance = FindViewById<EditText>(Resource.Id.txt_dw_current_balance);
            balance.Text = current_balance.ToString();
            amt_change = FindViewById<EditText>(Resource.Id.txt_dw_amount_moved);

            send.Click += Send_Click;

            depositwithdraw.Click += (object sender, EventArgs args) =>
            {
                if (depositwithdraw.Text == "Withdraw")
                    depositwithdraw.Text = "Deposit";
                else
                    depositwithdraw.Text = "Withdraw";
            };

            cancel.Click += (object sender, EventArgs args) => {
                SetResult(Result.Canceled);
                Finish(); 
            };

            refresh.Click += Refresh_Click;
        }

        private async void Refresh_Click(object sender, EventArgs e)
        {
            var PersonID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));
            var PaymentMethods = await ModelMethods.GetPaymentMethods(PersonID);
            if (PaymentMethods == null)
            {
                Toast.MakeText(this, "You don't have any payment methods on file. Please add a payment method and try again.", ToastLength.Long).Show();
            }
            else
            {
                sc.Post(new SendOrPostCallback(o => {
                    models.Clear();
                    adapter.NotifyDataSetChanged();
                }), null);
                foreach (var p in PaymentMethods)
                {
                    sc.Post(new SendOrPostCallback(o =>
                    {
                        models.Add(o as PaymentModel);
                        adapter.NotifyDataSetChanged();
                    }), p);
                }
            }
        }

        private void Send_Click(object sender, EventArgs e)
        {
            float change;
            try
            {
                float.TryParse(amt_change.Text, out change);
            }
            catch
            {
                Toast.MakeText(this, "Invalid change amount.", ToastLength.Short).Show();
                return;
            }
            if (depositwithdraw.Text == "Withdraw" && current_balance - change < 0)
            {
                Toast.MakeText(this, "You cannot remove more from your account that you have.", ToastLength.Short).Show();
            }
            else
            {
                var PersonID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));
                if (depositwithdraw.Text == "Withdraw")
                    current_balance -= change;
                else
                    current_balance += change;
                ModelMethods.ChangeAccountBalance(PersonID, current_balance);
                Intent intent = new Intent();
                intent.PutExtra("Balance", current_balance);
                SetResult(Result.Ok, intent);
                Finish();
            }
        }
    }
}