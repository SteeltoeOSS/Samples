using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public interface IFortuneService
    {
        Task<Fortune> RandomFortuneAsync();

        Task<List<Fortune>> GetFortunesAsync(List<int> fortuneIds);

        Task<Fortune> RandomFortuneWithRetryAsync();

        Task<Fortune> RandomFortuneUserCommandAsync();

        Task<Fortune> RandomFortuneDefaultCommandAsync();
    }
}
