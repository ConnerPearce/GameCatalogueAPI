using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCatalogueAPI.Models
{
    public class Game
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public double? Rating { get; set; } // Can be null as some games dont have ratings (NOT AGE RATING)
        public string[] Platform { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}
