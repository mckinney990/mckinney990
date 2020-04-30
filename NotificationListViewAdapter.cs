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
    class NotificationListViewAdapter : BaseAdapter<NotificationItem>
    {

        private List<NotificationItem> mItems;
        private Context mContext;
        private Activity mActivity;

        public NotificationListViewAdapter(Context context, List<NotificationItem> items, Activity activity)
        {
            mItems = items;
            mContext = context;
            mActivity = activity;
        }
        public override int Count
        {
            get { return mItems.Count; }
        }

        public override long GetItemId(int position)
        {
            return mItems[position].ID;
        }

        public override NotificationItem this[int position]
        {
            get { return mItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            NotificationViewHolder holder = null;
            View row = convertView;
            NotificationItem notification = this[position];
            if (row != null)
                holder = row.Tag as NotificationViewHolder;
            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.notification_item, null, false);
                holder = new NotificationViewHolder();
                holder.description = row.FindViewById<TextView>(Resource.Id.txtNotificationListItemDsc);
                holder.viewButton = row.FindViewById<Button>(Resource.Id.btnViewNotificationFromList);
                holder.dismissButton = row.FindViewById<Button>(Resource.Id.btnDismissNotificationFromList);

                holder.viewButton.Click += (object sender, EventArgs e) =>
                {
                    int pos = (int)(((Button)sender).GetTag(Resource.Id.btnViewNotificationFromList));
                    mItems.RemoveAt(pos);
                    mActivity.RunOnUiThread(() => this.NotifyDataSetChanged());
                };

                holder.dismissButton.Click += (object sender, EventArgs e) =>
                {
                    int pos = (int)(((Button)sender).GetTag(Resource.Id.btnDismissNotificationFromList));
                    mItems.RemoveAt(pos);
                    mActivity.RunOnUiThread(() => this.NotifyDataSetChanged());
                };

                row.Tag = holder;
            }

            holder.description.Text = notification.Description;
            holder.viewButton.SetTag(Resource.Id.btnViewNotificationFromList, position);
            holder.dismissButton.SetTag(Resource.Id.btnDismissNotificationFromList, position);

            return row;
        }

        private class NotificationViewHolder : Java.Lang.Object
        {
            public TextView description { get; set; }
            public Button viewButton { get; set; }
            public Button dismissButton { get; set; }
        }

    }
}