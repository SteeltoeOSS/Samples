using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortuneTellerService.Models
{
    public class FortuneRepository : IFortuneRepository
    {
        private FortuneContext _db;
        Random _random = new Random();

        public FortuneRepository(FortuneContext db)
        {
            _db = db;
        }
        public IEnumerable<Fortune> GetAll()
        {
            return _db.Fortunes.AsEnumerable();
        }

        public IEnumerable<Fortune> GetSome(List<string> list)
        {
            return _db.Fortunes.Where((f) =>
                list.Contains(f.Id.ToString())
            );
        }

        public Fortune RandomFortune()
        {
            int count = _db.Fortunes.Count();
            var index = _random.Next() % count;
            return GetAll().ElementAt(index);
        }
    }
}
