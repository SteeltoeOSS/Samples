using System.Collections.Generic;

namespace FortuneTeller.Service.Models
{
    public interface IFortuneRepository
    {
        IEnumerable<Fortune> GetAll();

        Fortune RandomFortune();
    }
}
