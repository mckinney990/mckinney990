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
    [Activity(Label = "FriendAddActivity")]
    public class FriendAddActivity : Activity
    {
        private EditText userID;
        private Button add, cancel;
        private AccountModel account;
        private UserModel user;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add_friend);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));

            userID = FindViewById<EditText>(Resource.Id.txt_friend_add_uid);
            add = FindViewById<Button>(Resource.Id.btn_friend_add_add);
            cancel = FindViewById<Button>(Resource.Id.btn_friend_add_exit);

            add.Click += Add_Click;
            cancel.Click += Cancel_Click;

        }

        private async void Add_Click(object sender, EventArgs e)
        {
            var friend = await ModelMethods.FindAccountByUserID(userID.Text);
            if (friend == null)
            {
                Toast.MakeText(this, "There is no user by that user ID.", ToastLength.Short).Show();
            }
            else
            {
                if (account.FriendIDs == null)
                    account.FriendIDs = new List<BsonObjectId>();
                account.FriendIDs.Add(friend.Id);
                ModelMethods.UpdateAccount(account);
                Return();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
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