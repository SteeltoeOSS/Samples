using System.Collections.Generic;

namespace FortuneTellerWinService.Models
{
    public interface IFortuneRepository
    {
        IEnumerable<Fortune> GetAll();

        Fortune RandomFortune();
    }
}
