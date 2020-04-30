using System;
using System.Collections;
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
    [Activity(Label = "PaymentMethodActivity")]
    public class PaymentMethodActivity : Activity
    {
        private Spinner spinner;
        private PaymentMethodAdapter adapter;
        private List<PaymentModel> models;
        private SynchronizationContext sc;
        private TextView description, nameoncard, number, expiry, security, zip;
        private Button add, edit, exit, refresh;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.payment_list);

            spinner = FindViewById<Spinner>(Resource.Id.spPaymentMethods);
            models = new List<PaymentModel>();

            adapter = new PaymentMethodAdapter(this, models);
            spinner.Adapter = adapter;

            sc = SynchronizationContext.Current;

            description = FindViewById<TextView>(Resource.Id.txtPaymentMethodDescription);
            nameoncard = FindViewById<TextView>(Resource.Id.txtNameOnCard);
            number = FindViewById<TextView>(Resource.Id.txtCardNumber);
            expiry = FindViewById<TextView>(Resource.Id.txtExpiry);
            security = FindViewById<TextView>(Resource.Id.txtSecurityCode);
            zip = FindViewById<TextView>(Resource.Id.txtZipCode);

            add = FindViewById<Button>(Resource.Id.btn_add_pmethod);
            edit = FindViewById<Button>(Resource.Id.btn_edit_pmethod);
            exit = FindViewById<Button>(Resource.Id.btn_exit_payment_methods);
            refresh = FindViewById<Button>(Resource.Id.btn_pm_refresh);

            add.Click += Add_Click;
            add.Click += Refresh_Click;
            edit.Click += Edit_Click;
            exit.Click += (object sender, EventArgs args) => { Finish(); };
            refresh.Click += Refresh_Click;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PaymentAddActivity));
            intent.PutExtra("PersonID", Intent.GetStringExtra("PersonID"));
            this.StartActivity(intent);
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (models.Count == 0)
            {
                Toast.MakeText(this, "Please refresh and select a payment method before trying to edit something.", ToastLength.Short).Show();
            }
            else
            {
                var PersonID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));
                PaymentModel model = models[spinner.SelectedItemPosition];
                Intent intent = new Intent(this, typeof(PaymentEditActivity));
                intent.PutExtra("Model", model.ToJson());
                intent.PutExtra("PersonID", Intent.GetStringExtra("PersonID"));
                this.StartActivity(intent);
            }
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
    }

    public class PaymentMethodAdapter : BaseAdapter<PaymentModel>
    {
        private Context mContext;
        private List<PaymentModel> mItems;
        public PaymentMethodAdapter(Context context, List<PaymentModel> items)
        {
            mContext = context;
            mItems = items;
        }

        public override PaymentModel this[int position]
        {
            get
            {
                return mItems.ElementAt(position);
            }
        }

        public override int Count => mItems.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.payment_spinner_row, null, false);
            }
            TextView view = row.FindViewById<TextView>(Resource.Id.txt_payment_spinner_row);
            view.Text = mItems[position].Description;
            return row;
        }
    }
}