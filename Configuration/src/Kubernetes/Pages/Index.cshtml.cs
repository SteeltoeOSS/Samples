using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Common;
using Steeltoe.Common.Kubernetes;

namespace Kubernetes.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApplicationInstanceInfo _applicationInstanceInfo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IndexModel> _logger;
        public string ConfigMapName;
        public string UserName;
        public string K8sNamespace;

        public IndexModel(IApplicationInstanceInfo applicationInstanceInfo, IConfiguration configuration, ILogger<IndexModel> logger)
        {
            _applicationInstanceInfo = applicationInstanceInfo;
            _configuration = configuration;
            _logger = logger;
        }

        public void OnGet()
        {
            _logger?.LogInformation("OnGetAsync");
            ConfigMapName = _configuration["configMapName"];
            UserName = _configuration["username"];
            if (_applicationInstanceInfo is KubernetesApplicationOptions k8sInfo)
            {
                _logger?.LogInformation("Found KubernetesApplicationOptions");
                K8sNamespace = k8sInfo.NameSpace;
            }
        }
    }
}
