using System.Text;

namespace Shurl.Core;

public class GetBaseUrl : IGetBaseUrl
{
    private readonly IHttpContextAccessor _accessor;
    private StringBuilder sb;

    public GetBaseUrl(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        sb = new StringBuilder();
        sb.Append(accessor?.HttpContext?.Request.Scheme);
        sb.Append("://");
        sb.Append(accessor?.HttpContext?.Request?.Host);
        sb.Append('/');
    }

    

    public string Url => sb.ToString();
}
