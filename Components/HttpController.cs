using System.Net;

namespace HttpLauncher.Components
{
    public interface HttpController
    {
        HttpStatusCode ProcessRequest(HttpListenerRequest request, HttpListenerResponse response);
    }
}
