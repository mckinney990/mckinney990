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
    class TransactionAdapter : BaseAdapter<TransactionModel>
    {
        private Context context;
        private List<TransactionModel> list;

        public TransactionAdapter(Context context, List<TransactionModel> items)
        {
            this.context = context;
            list = items;
        }

        public override TransactionModel this[int position]
        {
            get
            {
                return list.ElementAt(position);
            }
        }

        public override int Count => list.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(context).Inflate(Resource.Layout.transaction_item, null, false);
            }
            TextView view = row.FindViewById<TextView>(Resource.Id.txt_trans_item_string);
            var elem = list.ElementAt(position);
            view.Text = String.Format("{0:s} || {1:s}",elem.DateCreated.ToString("g"), elem.Description);
            return row;
        }
    }
}