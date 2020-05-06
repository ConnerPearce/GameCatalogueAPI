using GameCatalogueAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalogueAPI.Services
{
    public class DataService
    {
        private IMongoDatabase database;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Game> _game;
        private readonly IMongoCollection<Played> _played;
        private readonly IMongoCollection<Wishlist> _wishlist;

        public DataService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            database = client.GetDatabase(settings.DatabaseName);

            _user = database.GetCollection<User>(settings.UserCollectionName);
            _game = database.GetCollection<Game>(settings.GameCollectionName);
            _played = database.GetCollection<Played>(settings.PlayedCollectionName);
            _wishlist = database.GetCollection<Wishlist>(settings.WishlistCollectionName);
        }


        // Grabs all records from any table
        public async Task<IEnumerable<T>> GetAllAsync<T>(string collection)
        {
            if (collection != "User")
            {
                var items = database.GetCollection<T>(collection);

                return await items.Find(new BsonDocument()).ToListAsync();
            }
            else return null;

        }


        // By making this method generic it can handle grabing records from any collection by the id
        public async Task<T> GetRecordByIdAsync<T>(string collection, string id)
        {
            var item = database.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("_id", id);

            return await item.Find(filter).FirstAsync();
        }

        // Gets the user
        // Had to do it this way so i could match the users login info
        public async Task<User> GetUserAsync(string uName, string pwrd)
        {
            var item = database.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Where(e => e.UName == uName && e.Pwrd == pwrd);

            return await item.Find(filter).FirstAsync();
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
            var db = database.GetCollection<T>(collection);
            var result = await db.ReplaceOneAsync(
                new BsonDocument("_id", id),
                item,
                new ReplaceOptions { IsUpsert = true });

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        // Deletes a record by id
        public async Task<bool> DeleteAsync<T>(string id, string collection)
        {
            var item = database.GetCollection<T>(collection);
            var filter = Builders<T>.Filter.Eq("_id", id);

            if (item == null)
              return false;
           
            var result = await item.DeleteOneAsync(filter);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

    }
}
