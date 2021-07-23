using System.Threading.Tasks;

namespace FortuneTeller.UI.Services
{
    public interface IFortuneService
    {
        Task<Fortune> RandomFortuneAsync();
    }
}
