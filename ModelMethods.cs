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
        public static async Task<AccountModel> FindAccountByUserID(BsonObjectId id)
        {
            try
            {
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", id);
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
                Balance = 0
            };
            await Database.Users.InsertOneAsync(user);
            await Database.Accounts.InsertOneAsync(account);
            return;
        }
        public static async void AddPaymentMethod(BsonObjectId PersonID, PaymentModel model)
        {
            var account = await FindAccountByUserID(PersonID);
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
        public static async Task<List<PaymentModel>> GetPaymentMethods(BsonObjectId ID)
        {
            try
            {
                var account = await FindAccountByUserID(ID);
                return account.PaymentMethods.ToList();
            }
            catch
            {
                return null;
            }
        }
        public static async void UpdatePaymentMethod(BsonObjectId PersonID, PaymentModel model)
        {
            var account = await FindAccountByUserID(PersonID);
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
            var account = await FindAccountByUserID(PersonID);
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
            var account = await FindAccountByUserID(PersonID);
            if (account == null)
            {
                Console.WriteLine("FAILED!");
            }
            else
            {
                account.Balance = balance;
                var filter = Builders<AccountModel>.Filter.Eq("PersonID", PersonID);
                var update = Builders<AccountModel>.Update.Set("Balance", account.Balance);
                await Database.Accounts.UpdateOneAsync(filter, update);
            }
        }
    }
}