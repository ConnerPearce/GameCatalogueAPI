using GameCatalogueAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameCatalogueAPI.Services
{
    public class DataService
    {
        private IMongoDatabase database;

        public DataService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);
        }


        // Grabs all records from any table
        public async Task<IEnumerable<T>> GetAllAsync<T>(string collection)
        {
            var items = database.GetCollection<T>(collection);

            return await items.Find(new BsonDocument()).ToListAsync();
        }

        // Gets games by name
        public async Task<IEnumerable<Game>> GetGameBySearch(string name)
        {
            var items = database.GetCollection<Game>("Game");

            // Used for converting name string to sentance case as in the database everything is stored in sentance case
            var lowerCase = name.ToLower();
            // matches the first sentance of the string as well as subsequent sentances
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            // Replaces the start of every sentance with a capital letter
            var result = r.Replace(lowerCase, e => e.Value.ToUpper());

            // Creates a filter for all the item. Will find the record where the name contains result
            var filter = Builders<Game>.Filter.Where(e => e.name.Contains(result));

            return await items.Find(filter).ToListAsync(); // Returns its findings as a list
        }

        // By making this method generic it can handle grabing records from any collection by the id
        public async Task<T> GetRecordByIdAsync<T>(string collection, string id)
        {
            var itemId = new ObjectId(id);
            var item = database.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("_id", itemId);

            return await item.Find(filter).FirstOrDefaultAsync();
        }

        // Gets the user
        // Had to do it this way so i could match the users login info
        public async Task<User> GetUserAsync(string uName, string pwrd)
        {
            var item = database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Where(e => e.UName == uName && e.Pwrd == pwrd);

            return await item.Find(filter).FirstOrDefaultAsync();
        }

        // Gets multiple records by Id
        // Only for wishlist and played games
        // Grabs the records associated with that userID
        public async Task<IEnumerable<T>> GetMultipleByID<T>(string id,string collection)
        {
            var items = database.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("UId", id);

            return await items.Find(filter).ToListAsync();
        }

        // Inserts a new record
        public async Task InsertAsync<T>(T item, string collection)
        {
            var db = database.GetCollection<T>(collection);
            await db.InsertOneAsync(item);

        }

        // Updates a record by ID
        public async Task<bool> UpdateAsync<T>(string id, T item, string collection)
        {
            var itemId = new ObjectId(id);
            var db = database.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("_id", itemId);
            var result = await db.ReplaceOneAsync(
                filter,
                item,
                new ReplaceOptions { IsUpsert = true }
                );

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Deletes a record by id
        public async Task<bool> DeleteAsync<T>(string id, string collection)
        {
            var itemId = new ObjectId(id);
            var item = database.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("_id", itemId);

            if (item == null)
              return false;
           
            var result = await item.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

    }
}
