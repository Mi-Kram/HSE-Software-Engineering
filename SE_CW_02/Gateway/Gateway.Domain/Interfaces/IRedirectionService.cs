namespace Gateway.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для проксирования запросов.
    /// </summary>
    /// <typeparam name="SourceRequestT">Исходный запрос.</typeparam>
    /// <typeparam name="RedirectedRequestT">проксированный запрос.</typeparam>
    /// <typeparam name="RequestArg">Параметр для проксирования запроса.</typeparam>
    /// <typeparam name="SourceResponseT">Исходный ответ.</typeparam>
    /// <typeparam name="RedirectedResponseT">проксированный ответ.</typeparam>
    /// <typeparam name="ResponseArg">Параметр для проксирования ответа.</typeparam>
    public interface IRedirectionService<
        SourceRequestT, RedirectedRequestT, RequestArg,
        SourceResponseT, RedirectedResponseT, ResponseArg>
        where RequestArg : class
        where ResponseArg : class
    {
        /// <summary>
        /// проксировать запрос.
        /// </summary>
        /// <param name="originalRequest">Запрос, который надо проксировать.</param>
        /// <param name="redirectArg">Параметр для проксирования запроса.</param>
        /// <returns>проксированный запрос.</returns>
        Task<RedirectedRequestT> RedirectRequestAsync(SourceRequestT originalRequest, RequestArg? redirectArg = null);

        /// <summary>
        /// проксировать ответ.
        /// </summary>
        /// <param name="originalResponse">Ответ, куда надо проксировать другой ответ.</param>
        /// <param name="response">Ответ для проксирования.</param>
        Task RedirectResponseAsync(SourceResponseT originalResponse, RedirectedResponseT response, ResponseArg? redirectArg = null);
    }
}
