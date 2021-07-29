using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Steeltoe.Actuators.Models
{
    public class LoggingViewModel
    {
        public string SelectedNamespace { get; set; }

        public string SelectedLevel { get; set; }

        public List<SelectListItem> Levels { get; set; }
        
        public List<SelectListItem> Namespaces { get; set; }

    }
}
