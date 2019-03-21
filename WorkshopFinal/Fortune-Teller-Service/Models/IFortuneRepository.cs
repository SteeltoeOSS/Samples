using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fortune_Teller_Service.Models
{
    public interface IFortuneRepository
    {
        Task<List<FortuneEntity>> GetAllAsync();

        Task<FortuneEntity> RandomFortuneAsync();
    }
}
