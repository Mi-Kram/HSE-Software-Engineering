namespace GatewayService.Domain.Interfaces
{
    public interface IRedirectionService<
        SourceRequestT, RedirectedRequestT, RequestArg,
        SourceResponseT, RedirectedResponseT, ResponseArg>
        where RequestArg : class
        where ResponseArg : class
    {
        Task<RedirectedRequestT> RedirectRequestAsync(SourceRequestT originalRequest, RequestArg? redirectArg = null);
        Task RedirectResponseAsync(SourceResponseT originalResponse, RedirectedResponseT response, ResponseArg? redirectArg = null);
    }
}
