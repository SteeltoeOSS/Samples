

using System.Collections.Generic;

namespace FortuneTellerService4.Models
{
    public class FortuneContext 
    {
 
        public FortuneContext(Dictionary<int, Fortune> dbset) 
        {
            Fortunes = dbset;
        }
        public Dictionary<int, Fortune> Fortunes { get; private set; }
    }
}
