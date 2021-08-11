using Steeltoe.Actuators.Models;
using System.Threading.Tasks;

namespace Steeltoe.Actuators.Services
{
    public interface ILogLevelService
    {
        Task<DynamicLogLevels> GetLogLevelsAndNamespaces();

        Task<DynamicLogLevel> SetLogLevels(string name, string level);
    }
}
