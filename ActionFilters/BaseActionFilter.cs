using System;
using System.Net.Http;
using System.Web.Http.Filters;

namespace ActionFilters
{
    public abstract class BaseActionFilter : ActionFilterAttribute
    {
        public BaseActionFilter(ResponsePolicy responsePolicy)
        {
            Policy = responsePolicy;
        }

        protected ResponsePolicy Policy { get; }

        protected void SetResponse<TResponse>(HttpActionExecutedContext context, TResponse response)
            where TResponse : HttpResponseMessage
        {
            switch (Policy)
            {
                case ResponsePolicy.Propagate:
                    context.Request.Properties["last_response"] = response;
                    break;
                case ResponsePolicy.PreferOwn:
                    context.Response = response;
                    break;
                case ResponsePolicy.TakePropagatesd:
                    context.Response = (HttpResponseMessage)context.Request.Properties["last_response"];
                    break;
                default:
                    throw new NotSupportedException($"{Policy} is not supported.");
            }
        }
    }
}
