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
using MongoDB.Bson.Serialization;

namespace MulliganWallet
{
    [Activity(Label = "PaymentEditActivity")]
    public class PaymentEditActivity : Activity
    {
        private EditText Description, NameOnCard, CardNumber, Expiry, SecurityNumber, ZipCode;
        private PaymentModel model;
        private Button save, remove, cancel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.payment_edit);

            model = BsonSerializer.Deserialize<PaymentModel>(Intent.GetStringExtra("Model"));

            Description = FindViewById<EditText>(Resource.Id.txt_payment_edit_description);
            Description.Text = model.Description;

            NameOnCard = FindViewById<EditText>(Resource.Id.txt_payment_edit_name_on_card);
            NameOnCard.Text = model.NameOnCard;

            CardNumber = FindViewById<EditText>(Resource.Id.txt_payment_edit_card_number);
            CardNumber.Text = model.CardNumber;

            Expiry = FindViewById<EditText>(Resource.Id.txt_payment_edit_expiry_date);
            Expiry.Text = model.ExpiryDate;

            SecurityNumber = FindViewById<EditText>(Resource.Id.txt_payment_edit_security_number);
            SecurityNumber.Text = model.SecurityNumber;

            ZipCode = FindViewById<EditText>(Resource.Id.txt_payment_edit_zip_code);
            ZipCode.Text = model.ZipCode;

            save = FindViewById<Button>(Resource.Id.btn_payment_edit_save);
            save.Click += Save_Click;

            remove = FindViewById<Button>(Resource.Id.btn_payment_edit_remove);
            remove.Click += Remove_Click;

            cancel = FindViewById<Button>(Resource.Id.btn_payment_edit_cancel);
            cancel.Click += (object sender, EventArgs e) => { Finish(); };

        }

        private void Save_Click(object sender, EventArgs e)
        {
            model.Description = Description.Text;
            model.NameOnCard = NameOnCard.Text;
            model.CardNumber = CardNumber.Text;
            model.ExpiryDate = Expiry.Text;
            model.SecurityNumber = SecurityNumber.Text;
            model.ZipCode = ZipCode.Text;
            var PersonID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));
            ModelMethods.UpdatePaymentMethod(PersonID, model);
            Finish();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var PersonID = ObjectId.Parse(Intent.GetStringExtra("PersonID"));
            ModelMethods.RemovePaymentMethod(PersonID, model);
            Finish();
        }
    }
}