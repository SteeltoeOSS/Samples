using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace AutofacCloudFoundry.Helpers
{
    public static class ServiceCredentialsHelper
    {
        public static MvcHtmlString ShowCredentials(this HtmlHelper helper, Dictionary<string, Credential> credentials)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>");
            if (credentials != null)
            {
                foreach (KeyValuePair<string, Credential> pair in credentials)
                {
                    GenerateCredentialHtml(pair.Key, pair.Value, sb);
                }
            }
            sb.Append("</ul>");
            return new MvcHtmlString(sb.ToString());
        }
        private static void GenerateCredentialHtml(string key, Credential credential, StringBuilder output)
        {
            if (!string.IsNullOrEmpty(credential.Value))
            {
                output.Append("<li>");
                output.Append(key + "=" + credential.Value);
                output.Append("</li>");
            }
            else
            {

                output.Append("<li>");
                output.Append(key);
                output.Append("</li>");

                output.Append("<ul>");
                foreach (KeyValuePair<string, Credential> pair in credential)
                {
                    GenerateCredentialHtml(pair.Key, pair.Value, output);
                }
                output.Append("</ul>");
            }
        }
    }
}