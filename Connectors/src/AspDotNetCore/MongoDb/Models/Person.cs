using MongoDB.Bson;

namespace MongoDb.Models
{
    public class Person
    {
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FavoriteThing { get; set; }
    }
}
