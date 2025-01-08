
using PrintEngine.Core.Models;
using System.Net;

namespace PrintEngine.Core.Services
{
    public abstract class BaseExternalService<TRequest, TResponse>
        : IDisposable
    {
        private IHttpClientFactory _Factory;
        public const int SendRequestCount = 3;
        private HttpStatusCode[] errorCodes =
        {
            HttpStatusCode.BadGateway,
            HttpStatusCode.BadRequest,
            HttpStatusCode.Conflict,
            HttpStatusCode.Forbidden,
            HttpStatusCode.InternalServerError
        };

        protected BaseExternalService(IHttpClientFactory factory)
        {
            _Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        protected async Task<TResponse> SendRequestAsync(TRequest request)
        {
            try
            {
                for (int i = 1; i <= SendRequestCount; i++)
                {
                    try
                    {
                        using var httpClient = _Factory.CreateClient();
                        httpClient.Timeout = TimeSpan.FromSeconds(20);

                        return await SendRequest(httpClient, request);
                    }
                    catch (HttpRequestException x)
                    {
                        if (errorCodes.Any(c => c == x.StatusCode)
                            && i == SendRequestCount)
                            throw new PrintTemplateException(x.ToString());
                    }
                }
                throw new PrintTemplateException("Ошибка вызова внешнего сервиса");
            }
            catch (Exception x)
            {
                throw new PrintTemplateException(x.Message);
            }
        }
        protected abstract Task<TResponse> SendRequest(HttpClient httpClient, TRequest request);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Factory = null;
            }
        }
    }
}
