using System.Net;
using System.Security.Principal;

namespace HttpLauncher.Components
{
    public interface HttpAuthenticator
    {
        bool IsEnabled { get; }
        bool AuthenticateRequest(HttpListenerRequest request, IPrincipal user);
    }
}
