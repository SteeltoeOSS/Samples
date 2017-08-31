using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortuneTellerUI4.Services
{
    public interface IFortuneService
    {
        Task<string> RandomFortuneAsync();
    }
}
