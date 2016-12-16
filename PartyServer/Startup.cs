using System.Web.Http;

using Owin;

using Swashbuckle.Application;

namespace PartyServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            var config = new HttpConfiguration { IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always };
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Docs");
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c => c.DisableValidator());

            // Web API routes
            config.MapHttpAttributeRoutes();

            appBuilder.UseWebApi(config);
        }

        protected static string GetXmlCommentsPath()
        {
            return $@"{System.AppDomain.CurrentDomain.BaseDirectory}\PartyServer.XML";
        }
    }
}