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
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    public class AccountModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public BsonObjectId PersonID { get; set; }
        public List<BsonObjectId> FriendIDs { get; set; }
        public List<BsonObjectId> SavedTransactions { get; set; }
        public List<PaymentModel> PaymentMethods { get; set; }
        public float Balance { get; set; }
    }
    public class PaymentModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public string Description { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityNumber { get; set; }
        public string ZipCode { get; set; }
    }
    public class TransactionModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public string Description { get; set; }
        public BsonObjectId SenderID { get; set; }
        public BsonObjectId RecipientID { get; set; }
        public string Memo { get; set; }
        public float Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public BsonObjectId PaymentMethodID { get; set; }
        public String Accepted { get; set; }
    }
    public class NotificationModel
    {
        [BsonId]
        public BsonObjectId Id { get; set; }
        public BsonObjectId PersonID { get; set; }
        public BsonObjectId DataID { get; set; }
        public bool Viewed { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}