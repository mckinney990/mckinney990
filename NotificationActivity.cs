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
    [Activity(Label = "NotificationActivity")]
    public class NotificationActivity : Activity
    {
        private long lastPress;
        private List<NotificationItem> mItems;
        private ListView mListView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.notifications);

            mListView = FindViewById<ListView>(Resource.Id.list_notifications);

            mItems = new List<NotificationItem>();
            NotificationListViewAdapter adapter = new NotificationListViewAdapter(this, mItems, this);

            mListView.Adapter = adapter;

            Button btnClearNotifications = FindViewById<Button>(Resource.Id.btn_clear_notifications);
            Button btnExit = FindViewById<Button>(Resource.Id.btn_exit_notifications);

            btnExit.Click += (object sender, EventArgs e) => { Finish(); };
        }

        public override void OnBackPressed()
        {
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            if (currentTime - lastPress > 2000)
            {
                Toast.MakeText(this, "Press back again to exit", ToastLength.Long).Show();
                lastPress = currentTime;
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}