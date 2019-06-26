using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fortune_Teller_UI.Services
{
    public interface IFortuneService
    {
        Task<List<Fortune>> AllFortunesAsync();
        Task<Fortune> RandomFortuneAsync();
    }
}
