using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    [Activity(Label = "TransactionMakeActivity")]
    public class TransactionMakeActivity : Activity
    {
        private EditText Description, Recipient, Memo, Amount;
        private Button ChangeType, Send, Save, Cancel;
        private Spinner Spinner;
        private PaymentMethodAdapter Adapter;
        private AccountModel account;
        private UserModel user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.transaction_create);

            Description = FindViewById<EditText>(Resource.Id.txt_create_trans_desc);
            Recipient = FindViewById<EditText>(Resource.Id.txt_create_trans_recipient);
            Memo = FindViewById<EditText>(Resource.Id.txt_create_trans_memo);
            Amount = FindViewById<EditText>(Resource.Id.txt_create_trans_amount);

            ChangeType = FindViewById<Button>(Resource.Id.btn_create_trans_change_type);
            Send = FindViewById<Button>(Resource.Id.btn_create_trans_send);
            Save = FindViewById<Button>(Resource.Id.btn_create_trans_save);
            Cancel = FindViewById<Button>(Resource.Id.btn_create_trans_cancel);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            Spinner = FindViewById<Spinner>(Resource.Id.sp_create_trans_spinner);
            Adapter = new PaymentMethodAdapter(this, account.PaymentMethods);
            Spinner.Adapter = Adapter;

            ChangeType.Click += ChangeType_Click;
            Send.Click += Send_Click;
            Save.Click += Save_Click;
            Cancel.Click += Cancel_Click;
        }

        private void ChangeType_Click(object sender, EventArgs e)
        {
            if (ChangeType.Text == "Send Money")
                ChangeType.Text = "Receive Money";
            else
                ChangeType.Text = "Send Money";
        }
        private async Task<TransactionModel> GetModel()
        {
            var recipientAccount = await ModelMethods.FindAccountByUserID(Recipient.Text);
            if (recipientAccount != null)
            {
                return new TransactionModel
                {
                    Description = Description.Text,
                    SenderID = ChangeType.Text == "Send Money" ? recipientAccount.Id : account.Id,
                    RecipientID = ChangeType.Text == "Send Money" ? account.Id : recipientAccount.Id,
                    Memo = Memo.Text,
                    Amount = float.Parse(Amount.Text),
                    DateCreated = DateTime.UtcNow,
                    PaymentMethodID = Adapter[Spinner.SelectedItemPosition].Id
                };
            }
            else
                return null;
        }

        private async void Send_Click(object sender, EventArgs e)
        {
            var model = await GetModel();
            if (account.PaymentMethods == null || account.PaymentMethods.Count == 0)
            {
                Toast.MakeText(this, "No payment methods are associated with this account. Please create one first.", ToastLength.Short).Show();
            }
            else if (account.Balance - model.Amount < 0 || ChangeType.Text != "Receive Money")
            {
                Toast.MakeText(this, "You don't have enough mount to create this transaction.", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Transaction created.", ToastLength.Short).Show();
                account.Balance -= model.Amount;
                ModelMethods.CreateTransaction(model);
                Return();
            }
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            if (account.PaymentMethods == null || account.PaymentMethods.Count == 0)
            {
                Toast.MakeText(this, "No payment methods are associated with this account. Please create one first.", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Transaction saved.", ToastLength.Short).Show();
                var model = await GetModel();
                if (model != null)
                {
                    if (account.SavedTransactions == null)
                        account.SavedTransactions = new List<BsonObjectId>();
                    account.SavedTransactions.Add(model.Id);
                    ModelMethods.UpdateAccount(account);
                    Return();
                }
                else
                {
                    Toast.MakeText(this, "No recipient found by that User ID.", ToastLength.Short).Show();
                }
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Return();
        }

        private void Return()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}