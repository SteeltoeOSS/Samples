using System.Collections.Generic;

namespace FortuneTellerService4.Models
{
    public interface IFortuneRepository
    {
        IEnumerable<Fortune> GetAll();

        Fortune RandomFortune();
    }
}
