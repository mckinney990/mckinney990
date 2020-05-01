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
    [Activity(Label = "FriendListActivity")]
    public class FriendListActivity : Activity
    {
        private AccountModel account;
        private UserModel user;
        private List<UserModel> friends;
        private List<AccountModel> friendAccounts;
        private ListView friend_list;
        private FriendAdapter adapter;
        private Button add, exit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.friend_list);

            account = BsonSerializer.Deserialize<AccountModel>(Intent.GetStringExtra("Account"));
            user = BsonSerializer.Deserialize<UserModel>(Intent.GetStringExtra("User"));
            friends = BsonSerializer.Deserialize<List<UserModel>>(Intent.GetStringExtra("Friends"));
            friendAccounts = BsonSerializer.Deserialize<List<AccountModel>>(Intent.GetStringExtra("FriendAccounts"));

            friend_list = FindViewById<ListView>(Resource.Id.list_friends);
            adapter = new FriendAdapter(this, friends);
            friend_list.Adapter = adapter;

            add = FindViewById<Button>(Resource.Id.btn_add_friend);
            exit = FindViewById<Button>(Resource.Id.btn_exit_friend_list);

            add.Click += Add_Click;
            exit.Click += Exit_Click;

            friend_list.ItemClick += Friend_list_ItemClick;

        }

        private void Friend_list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var otherUser = friends.ElementAt(e.Position);
            AccountModel otherAccount = null;
            foreach (var account in friendAccounts)
            {
                if (otherUser.Id == account.PersonID)
                {
                    otherAccount = account;
                    break;
                }
            }
            Intent intent = new Intent(this, typeof(OtherProfileActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("OtherAccount", otherAccount.ToJson());
            intent.PutExtra("OtherUser", otherUser.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(FriendAddActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            intent.PutExtra("Friends", friends.ToJson());
            this.StartActivity(intent);
            Finish();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Return();
        }

        private void Return()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("Account", account.ToJson());
            intent.PutExtra("User", user.ToJson());
            this.StartActivity(intent);
            Finish();
        }
    }
}