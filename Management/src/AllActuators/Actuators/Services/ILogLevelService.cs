using Steeltoe.Actuators.Models;
using System.Threading.Tasks;

namespace Steeltoe.Actuators.Services
{
    public interface ILogLevelService
    {
        Task<LogLevelsAndNamespaces> GetLogLevelsAndNamespaces();

        Task<LogNamespace> SetLogLevels(string name, string level);
    }
}
