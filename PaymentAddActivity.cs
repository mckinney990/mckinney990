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
using MongoDB.Bson;

namespace MulliganWallet
{
    [Activity(Label = "PaymentAddActivity")]
    public class PaymentAddActivity : Activity
    {
        private EditText description, nameoncard, number, expiry, security, zip;
        private List<EditText> fields;
        private Button add, cancel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.payment_add);
            description = FindViewById<EditText>(Resource.Id.txt_pa_description);
            nameoncard = FindViewById<EditText>(Resource.Id.txt_pa_name_on_card);
            number = FindViewById<EditText>(Resource.Id.txt_pa_card_number);
            expiry = FindViewById<EditText>(Resource.Id.txt_pa_expiry);
            security = FindViewById<EditText>(Resource.Id.txt_pa_security_code);
            zip = FindViewById<EditText>(Resource.Id.txt_pa_zip_code);

            fields = new List<EditText>() { description, nameoncard, number, expiry, security, zip };

            add = FindViewById<Button>(Resource.Id.btn_pa_add);
            cancel = FindViewById<Button>(Resource.Id.btn_pa_cancel);

            add.Click += Add_Click;
            cancel.Click += (object sender, EventArgs e) => { Finish(); };
        }

        private void Add_Click(object sender, EventArgs e)
        {
            foreach (var field in fields)
                if (field.Text == String.Empty)
                {
                    Toast.MakeText(this, "Please fill out all fiends before adding payment method.", ToastLength.Short).Show();
                    return;
                }
            PaymentModel model = new PaymentModel
            {
                Description = description.Text,
                NameOnCard = nameoncard.Text,
                CardNumber = number.Text,
                ExpiryDate = expiry.Text,
                SecurityNumber = security.Text,
                ZipCode = security.Text
            };
            var ID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));
            ModelMethods.AddPaymentMethod(ID, model);
            Toast.MakeText(this, "Payment method added.", ToastLength.Short).Show();
            Finish();
        }
    }
}