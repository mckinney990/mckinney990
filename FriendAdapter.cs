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

namespace MulliganWallet
{
    class FriendAdapter : BaseAdapter<UserModel>
    {
        private List<UserModel> friends;
        private Context context;
        public FriendAdapter(Context context, List<UserModel> list)
        {
            friends = list;
            this.context = context;
        }
        public override UserModel this[int position] 
        {
            get
            {
                return friends.ElementAt(position);
            }
        }

        public override int Count => friends.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.friend_item, null, false);
            }
            TextView view = row.FindViewById<TextView>(Resource.Id.txt_friend_name);
            view.Text = this[position].FullName;
            return row;
        }
    }
}