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
using MongoDB.Bson.Serialization;

namespace MulliganWallet
{
    [Activity(Label = "PaymentMethodActivity")]
    public class PaymentMethodActivity : Activity
    {
        private Spinner spinner;
        private PaymentMethodAdapter adapter;
        private TextView description, nameoncard, number, expiry, security, zip;
        private Button add, edit, exit;
        private UserModel user;
        private AccountModel account;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.payment_list);

            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));
            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));

            spinner = FindViewById<Spinner>(Resource.Id.spPaymentMethods);

            if (account.PaymentMethods == null)
                account.PaymentMethods = new List<PaymentModel>();

            adapter = new PaymentMethodAdapter(this, account.PaymentMethods);
            spinner.Adapter = adapter;
            spinner.ItemSelected += Spinner_ItemSelected;

            description = FindViewById<TextView>(Resource.Id.txtPaymentMethodDescription);
            nameoncard = FindViewById<TextView>(Resource.Id.txtNameOnCard);
            number = FindViewById<TextView>(Resource.Id.txtCardNumber);
            expiry = FindViewById<TextView>(Resource.Id.txtExpiry);
            security = FindViewById<TextView>(Resource.Id.txtSecurityCode);
            zip = FindViewById<TextView>(Resource.Id.txtZipCode);

            if (account.PaymentMethods.Count > 0)
            {
                var method = account.PaymentMethods.ElementAt(0);
                description.Text = method.Description;
                nameoncard.Text = method.NameOnCard;
                number.Text = method.CardNumber;
                expiry.Text = method.ExpiryDate;
                security.Text = method.SecurityNumber;
                zip.Text = method.ZipCode;
            }

            add = FindViewById<Button>(Resource.Id.btn_add_pmethod);
            edit = FindViewById<Button>(Resource.Id.btn_edit_pmethod);
            exit = FindViewById<Button>(Resource.Id.btn_exit_payment_methods);

            add.Click += Add_Click;
            edit.Click += Edit_Click;
            exit.Click += (object sender, EventArgs args) => 
            {
                Intent intent = new Intent(this, typeof(ProfileActivity));
                intent.PutExtra("User", user.ToJson());
                intent.PutExtra("Account", account.ToJson());
                this.StartActivity(intent);
                Finish();
            };
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var method = account.PaymentMethods.ElementAt(e.Position);
            description.Text = method.Description;
            nameoncard.Text = method.NameOnCard;
            number.Text = method.CardNumber;
            expiry.Text = method.ExpiryDate;
            security.Text = method.SecurityNumber;
            zip.Text = method.ZipCode;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PaymentAddActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (account.PaymentMethods == null || account.PaymentMethods.Count == 0)
            {
                Toast.MakeText(this, "Please refresh and select a payment method before trying to edit something.", ToastLength.Short).Show();
            }
            else
            {
                PaymentModel model = account.PaymentMethods.ElementAt(spinner.SelectedItemPosition);
                Intent intent = new Intent(this, typeof(PaymentEditActivity));
                intent.PutExtra("Model", model.ToJson());
                intent.PutExtra("User", user.ToJson());
                intent.PutExtra("Account", account.ToJson());
                this.StartActivity(intent);
                Finish();
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