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
        public string id { get; set; }
        [BsonElement("Name")] // This specifys that the database has a different name to the property
        public string name { get; set; }
        [BsonElement("ReleaseDate")] // For the sake of making my app generic i made the names the same as the RAWG Api
        public DateTime released { get; set; }
        [BsonElement("Image")]
        public string background_image { get; set; }
        public string Summary { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public double? Rating { get; set; } // Can be null as some games dont have ratings (NOT AGE RATING)
        public string[] Platform { get; set; }


    }
}
