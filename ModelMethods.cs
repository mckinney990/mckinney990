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
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System.Threading.Tasks;

namespace MulliganWallet
{
    
    public class Database
    {
        public static IMongoDatabase DB = new MongoClient("mongodb://mduser:DF$16Mongo@159.65.37.158:27017/?authSource=admin").GetDatabase("MulliganWallet");
        public static IMongoCollection<AccountModel> Accounts = DB.GetCollection<AccountModel>("Accounts");
        public static IMongoCollection<UserModel> Users = DB.GetCollection<UserModel>("Users");
        public static IMongoCollection<TransactionModel> Transactions = DB.GetCollection<TransactionModel>("Transactions");
    }
    public class ModelMethods
    {
        public static async Task<UserModel> FindUserByUserID(String userid)
        {
            try
            {
                var filter = 
                      Builders<UserModel>.Filter.Eq("Username", userid)
                    | Builders<UserModel>.Filter.Eq("Email", userid)
                    | Builders<UserModel>.Filter.Eq("PhoneNumber", userid);
                var results = await Database.Users.FindAsync(filter);
                var result = await results.FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<UserModel> FindUserByID(BsonObjectId id)
        {
            try
            {
                var filter = Builders<UserModel>.Filter.Eq("Id", id);
                var results = await Database.Users.FindAsync(filter);
                var result = await results.FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<AccountModel> FindAccountByID(BsonObjectId ID)
        {
            try
            {
                var filter = Builders<AccountModel>.Filter.Eq("Id", ID);
                var results = await Database.Accounts.FindAsync(filter);
                var result = await results.FirstAsync();
                return result;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<AccountModel> FindAccountByUserID(string UserID)
        {
            try
            {
                var filter = Builders<UserModel>.Filter.Eq("Username", UserID)
                    | Builders<UserModel>.Filter.Eq("Email", UserID)
                    | Builders<UserModel>.Filter.Eq("PhoneNumber", UserID);
                var userResults = await Database.Users.FindAsync(filter);
                var user = await userResults.FirstAsync();
                var account = await FindAccountByPersonID(user.Id);
                return account;
            }
            catch
            {
                return null;
            }
        }
        public static async Task<AccountModel> FindAccountByPersonID(BsonObjectId PersonID)
        {
            try
            {
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", PersonID);
                var results = await Database.Accounts.FindAsync(filter);
                var result = await results.FirstAsync();
                return result;
            } 
            catch
            {
                return null;
            }
        }
        public static async void CreateUser(String fullname, String username, String email, String phonenumber, String password)
        {
            UserModel user = new UserModel
            {
                Id = ObjectId.GenerateNewId(DateTime.UtcNow),
                FullName = fullname,
                Username = username,
                Email = email,
                PhoneNumber = phonenumber,
                Password = password
            };
            AccountModel account = new AccountModel
            {
                Id = ObjectId.GenerateNewId(DateTime.UtcNow),
                PersonID = user.Id,
                Balance = 0,
                FriendIDs = new List<BsonObjectId>(),
                PaymentMethods = new List<PaymentModel>(),
                SavedTransactions = new List<BsonObjectId>()
            };
            await Database.Users.InsertOneAsync(user);
            await Database.Accounts.InsertOneAsync(account);
            return;
        }
        public static async void AddPaymentMethod(BsonObjectId PersonID, PaymentModel model)
        {
            var account = await FindAccountByPersonID(PersonID);
            if (account == null)
            {
                Console.WriteLine("FAILED!");
            }
            else
            {
                if (account.PaymentMethods == null)
                {
                    account.PaymentMethods = new List<PaymentModel>() { model };
                }
                else
                {
                    account.PaymentMethods.Add(model);
                }
                var payment_methods = account.PaymentMethods;
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", PersonID);
                var update = Builders<AccountModel>.Update.Set("PaymentMethods", payment_methods);
                await Database.Accounts.UpdateOneAsync(filter, update);
            }
        }
        public static async void UpdatePaymentMethod(BsonObjectId PersonID, PaymentModel model)
        {
            var account = await FindAccountByPersonID(PersonID);
            if (account == null)
            {
                Console.WriteLine("FAILED!");
            }
            else
            {
                if (account.PaymentMethods == null)
                {
                    account.PaymentMethods = new List<PaymentModel>() { model };
                }
                else
                {
                    account.PaymentMethods.Remove(account.PaymentMethods.Find(p => p.Id == model.Id));
                    account.PaymentMethods.Add(model);
                }
                var payment_methods = account.PaymentMethods;
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", PersonID);
                var update = Builders<AccountModel>.Update.Set("PaymentMethods", payment_methods);
                await Database.Accounts.UpdateOneAsync(filter, update);
            }
        }
        public static async void RemovePaymentMethod(BsonObjectId PersonID, PaymentModel model)
        {
            var account = await FindAccountByPersonID(PersonID);
            if (account == null)
            {
                Console.WriteLine("FAILED!");
            }
            else
            {
                if (account.PaymentMethods == null)
                {
                    return;
                }
                else
                {
                    account.PaymentMethods.Remove(account.PaymentMethods.Find(p => p.Id == model.Id));
                }
                var payment_methods = account.PaymentMethods;
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", PersonID);
                var update = Builders<AccountModel>.Update.Set("PaymentMethods", payment_methods);
                await Database.Accounts.UpdateOneAsync(filter, update);
            }
        }
        public static async void ChangeAccountBalance(BsonObjectId PersonID, float balance)
        {
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", PersonID);
                var update = Builders<AccountModel>.Update.Set("Balance", balance);
                await Database.Accounts.UpdateOneAsync(filter, update);
        }
        public static async void UpdateUserProfile(UserModel user)
        {
            var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
            var update = Builders<UserModel>.Update.Set("FullName", user.FullName)
                .Set("Username", user.Username)
                .Set("Email", user.Email)
                .Set("PhoneNumber", user.PhoneNumber);
            await Database.Users.UpdateOneAsync(filter, update);
        }
        public static async void CreateTransaction(TransactionModel trans)
        {
            await Database.Transactions.InsertOneAsync(trans);
        }
        public static async void UpdateAccount(AccountModel account)
        {
            var filter = Builders<AccountModel>.Filter.Eq("Id", account.Id);
            var update = Builders<AccountModel>.Update.Set("FriendIDs", account.FriendIDs)
                .Set("SavedTransactions", account.SavedTransactions)
                .Set("PaymentMethods", account.PaymentMethods)
                .Set("Balance", account.Balance);
            await Database.Accounts.UpdateOneAsync(filter, update);
        }
        public static async Task<List<TransactionModel>> GetTransactionsInvolvingAccount(AccountModel account)
        {
            var filter = Builders<TransactionModel>.Filter.Eq("SenderID", account.Id)
                | Builders<TransactionModel>.Filter.Eq("RecipientID", account.Id);
            var results = await Database.Transactions.Find(filter).SortBy(bson => bson.DateCreated).Limit(100).ToListAsync();
            return results;
        }

        public static async void UpdateTransactionAccepted(TransactionModel transaction)
        {
            var filter = Builders<TransactionModel>.Filter.Eq("Id", transaction.Id);
            var update = Builders<TransactionModel>.Update.Set("Accepted", transaction.Accepted);
            await Database.Transactions.UpdateOneAsync(filter, update);
        }

        public static async Task<List<UserModel>> GetListOfFriendsByAccount(AccountModel account)
        {
            List<UserModel> friends = new List<UserModel>();
            if (account.FriendIDs == null)
                account.FriendIDs = new List<BsonObjectId>();
            foreach (var id in account.FriendIDs)
            {
                var friendAccount = await FindAccountByID(id);
                var friendUser = await FindUserByID(friendAccount.PersonID);
                friends.Add(friendUser);
            }
            return friends;
        }
        public static async Task<List<AccountModel>> GetListOfAccountsByListOfUsers(List<UserModel> users)
        {
            List<AccountModel> accounts = new List<AccountModel>();
            foreach (var user in users)
            {
                var account = await FindAccountByPersonID(user.Id);
                accounts.Add(account);
            }
            return accounts;
        }
    }
}