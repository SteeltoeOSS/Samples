using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortuneTeller.Service.Models
{
    public class FortuneRepository : IFortuneRepository
    {
        private readonly FortuneContext _db;
        private readonly Random _random = new Random();

        public FortuneRepository(FortuneContext db)
        {
            _db = db;
        }
        public IEnumerable<Fortune> GetAll()
        {
            return _db.Fortunes.AsEnumerable();
        }

        public Fortune RandomFortune()
        {
            var count = _db.Fortunes.Count();
            var index = _random.Next() % count;
            return GetAll().ElementAt(index);
        }
    }
}
