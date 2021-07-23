using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FortuneTeller.Service.Models
{
    public interface IFortuneRepository
    {
        IEnumerable<Fortune> GetAll();

        Fortune RandomFortune();
    }
}
