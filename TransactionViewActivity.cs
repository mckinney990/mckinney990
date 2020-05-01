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
    [Activity(Label = "TransactionViewActivity")]
    public class TransactionViewActivity : Activity
    {
        private AccountModel account, otherAccount;
        private UserModel user, otherUser;
        private List<TransactionModel> transactions;
        private int position;
        private TextView Description, Sender, Recipient, Memo, Accepted, Amount;
        private Button accept, deny, cancel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.transaction_view);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            otherAccount = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("OtherAccount"));
            otherUser = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("OtherUser"));

            transactions = BsonSerializer.Deserialize<List<TransactionModel>>(Intent.GetStringExtra("Transactions"));
            position = Intent.GetIntExtra("Position", 0);

            accept = FindViewById<Button>(Resource.Id.btn_trans_view_accept);
            deny = FindViewById<Button>(Resource.Id.btn_trans_view_deny);
            cancel = FindViewById<Button>(Resource.Id.btn_trans_view_cancel);

            TransactionModel transaction = transactions.ElementAt(position);

            if (transaction.RecipientID == account.Id || transaction.Accepted == "Yes")
            {
                accept.Enabled = false;
                deny.Enabled = false;
            }

            Description = FindViewById<TextView>(Resource.Id.txt_trans_view_description);
            Description.Text = transaction.Description;

            Sender = FindViewById<TextView>(Resource.Id.txt_trans_view_sender);
            Recipient = FindViewById<TextView>(Resource.Id.txt_trans_view_recipient);

            if (transactions.ElementAt(position).SenderID == account.Id)
            {
                Sender.Text = user.FullName;
                Recipient.Text = otherUser.FullName;
            }
            else
            {
                Sender.Text = user.FullName;
                Recipient.Text = otherUser.FullName;
            }

            Memo = FindViewById<TextView>(Resource.Id.txt_trans_view_memo);
            Memo.Text = transaction.Memo;

            Accepted = FindViewById<TextView>(Resource.Id.txt_trans_view_accepted);
            Accepted.Text = transaction.Accepted;

            Amount = FindViewById<TextView>(Resource.Id.txt_trans_view_amount);
            Amount.Text = String.Format("{0:C}", transaction.Amount);

            accept.Click += Accept_Click;
            deny.Click += Deny_Click;
            cancel.Click += Cancel_Click;

        }

        private void Accept_Click(object sender, EventArgs e)
        {
            var transaction = transactions.ElementAt(position);
            transaction.Accepted = "Yes";
            ModelMethods.UpdateTransactionAccepted(transaction);
            account.Balance += transaction.Amount;
            ModelMethods.UpdateAccount(account);
            Return();
        }

        private void Deny_Click(object sender, EventArgs e)
        {
            var transaction = transactions.ElementAt(position);
            transaction.Accepted = "No";
            ModelMethods.UpdateTransactionAccepted(transaction);
            otherAccount.Balance += transaction.Amount;
            ModelMethods.UpdateAccount(otherAccount);
            Return();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Return();
        }

        private void Return()
        {
            Intent intent = new Intent(this, typeof(TransactionHistoryActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Transactions", transactions.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}