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
    [Activity(Label = "TransactionHistoryActivity")]
    public class TransactionHistoryActivity : Activity
    {
        private ListView list;
        private Button exit;
        private AccountModel account;
        private UserModel user;
        private List<TransactionModel> transactions;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.transaction_history);

            list = FindViewById<ListView>(Resource.Id.list_transactions);
            exit = FindViewById<Button>(Resource.Id.btn_exit_history);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));
            transactions = BsonSerializer.Deserialize<List<TransactionModel>>(Intent.GetStringExtra("Transactions"));

            TransactionAdapter adapter = new TransactionAdapter(this, transactions);
            list.Adapter = adapter;

            list.ItemClick += List_ItemClick;

            exit.Click += Exit_Click;
        }

        private async void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            BsonObjectId otherid;
            if (transactions.ElementAt(e.Position).SenderID != account.Id)
            {
                otherid = transactions.ElementAt(e.Position).SenderID;
            }
            else
            {
                otherid = transactions.ElementAt(e.Position).RecipientID;
            }
            var otherAccount = await ModelMethods.FindAccountByID(otherid);
            var otherUser = await ModelMethods.FindUserByID(otherAccount.PersonID);
            Intent intent = new Intent(this, typeof(TransactionViewActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Transactions", transactions.ToJson());
            intent.PutExtra("Position", e.Position);
            intent.PutExtra("OtherAccount", otherAccount.ToJson());
            intent.PutExtra("OtherUser", otherUser.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}