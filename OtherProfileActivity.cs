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
    [Activity(Label = "OtherProfileActivity")]
    public class OtherProfileActivity : Activity
    {
        private TextView FullName, Username, Email, PhoneNumber;
        private Button RemoveFriend, Block, ShowQR, CreateTransaction, SavedTransactions, Exit;
        private AccountModel account, otherAccount;
        private UserModel user, otherUser;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.profile_other);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            otherAccount = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("OtherAccount"));

            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));
            otherUser = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("OtherUser"));

            FullName = FindViewById<TextView>(Resource.Id.txt_other_profile_name);
            Username = FindViewById<TextView>(Resource.Id.txt_other_profile_email);
            Email = FindViewById<TextView>(Resource.Id.txt_other_profile_email);
            PhoneNumber = FindViewById<TextView>(Resource.Id.txt_other_profile_phone);

            FullName.Text = otherUser.FullName;
            Username.Text = otherUser.Username;
            Email.Text = otherUser.Email;
            PhoneNumber.Text = otherUser.PhoneNumber;

            RemoveFriend = FindViewById<Button>(Resource.Id.btn_other_remove_friend);
            Block = FindViewById<Button>(Resource.Id.btn_other_block);
            ShowQR = FindViewById<Button>(Resource.Id.btn_other_show_qr);
            CreateTransaction = FindViewById<Button>(Resource.Id.btn_other_create_trans);
            SavedTransactions = FindViewById<Button>(Resource.Id.btn_other_saved_trans);
            Exit = FindViewById<Button>(Resource.Id.btn_other_exit);

            RemoveFriend.Click += RemoveFriend_Click;
            Block.Click += Block_Click;
            ShowQR.Click += ShowQR_Click;
            CreateTransaction.Click += CreateTransaction_Click;
            SavedTransactions.Click += SavedTransactions_Click;
            Exit.Click += Exit_Click;
        }

        private void RemoveFriend_Click(object sender, EventArgs e)
        {
            if (account.FriendIDs == null)
                account.FriendIDs = new List<BsonObjectId>();
            if (account.FriendIDs.Contains(otherAccount.Id))
            {
                account.FriendIDs.Remove(otherAccount.Id);
                Toast.MakeText(this, "Removed from friend list.", ToastLength.Short).Show();
                ModelMethods.UpdateAccount(account);
                Return();
            }
        }

        private void Block_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Blocked", ToastLength.Short).Show();
            Return();
        }

        private void ShowQR_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "QR CODE", ToastLength.Short).Show();
        }

        private void CreateTransaction_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TransactionMakeActivity));
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Account", account.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void SavedTransactions_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "This is where we'd show this person's saved transactions", ToastLength.Short).Show();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Return();
        }

        private async void Return()
        {
            Intent intent = new Intent(this, typeof(FriendListActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            var friends = await ModelMethods.GetListOfFriendsByAccount(account);
            intent.PutExtra("Friends", friends.ToJson());
            var friendAccounts = await ModelMethods.GetListOfAccountsByListOfUsers(friends);
            intent.PutExtra("FriendAccounts", friendAccounts.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}