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
    class NotificationItem
    {

        private long _id;
        private String _description = String.Empty;
        public NotificationItem(long id, String description)
        {
            _id = id;
            _description = description;
        }

        public long ID
        {
            get { return _id; }
        }

        public String Description
        {
            get { return _description; }
        }

    }
}