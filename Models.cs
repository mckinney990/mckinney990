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
using MongoDB.Bson.Serialization.Attributes;

namespace MulliganWallet
{
    public class UserModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public String FullName { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String Password { get; set; }
    }
    public class AccountModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public BsonObjectId PersonID { get; set; }
        public BsonObjectId[] FriendIDs { get; set; }
        public BsonObjectId[] SavedTransactions { get; set; }
        public int Balance { get; set; }
    }
    public class PaymentModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public String NameOnCard { get; set; }
        public String CardNumber { get; set; }
        public String ExpirtDate { get; set; }
        public String SecurityNumber { get; set; }
        public String ZipCode { get; set; }
    }
    public class TransactionModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public String Description { get; set; }
        public BsonObjectId SenderID { get; set; }
        public BsonObjectId RecipientID { get; set; }
        public String Memo { get; set; }
        public int Amount { get; set; }
        public BsonObjectId PaymentMethodID { get; set; }
    }
}