using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Fortune_Teller_Service.Models
{
    public class FortuneRepository : IFortuneRepository
    {
        private FortuneContext _db;
        Random _random = new Random();

        public FortuneRepository(FortuneContext db)
        {
            _db = db;
        }

        public async Task<List<FortuneEntity>> GetAllAsync()
        {
            var all = await _db.Fortunes.ToListAsync();
            return all;
        }

        public async Task<FortuneEntity> RandomFortuneAsync()
        {
            var count = _db.Fortunes.Count();
            var index = _random.Next() % count;
            var all = await _db.Fortunes.ToListAsync();
            return all[index];
        }
    }
}
