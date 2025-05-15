using FileStoringService.Application.Interfaces;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;

namespace FileStoringService.Infrastructure.Services
{
    /// <summary>
    /// Хранилище работ.
    /// </summary>
    public class WorkStorageService(IMinioClient minioClient, IConfiguration configuration) : IWorkStorageService
    {
        private readonly IMinioClient minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
        private readonly string bucket = configuration?.GetSection(ApplicationVariables.SRORAGE_BUCKET)?.Value ?? throw new EnvVariableException(ApplicationVariables.SRORAGE_BUCKET);

        /// <summary>
        /// Получить работу.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="work">Поток данных работы.</param>
        public async Task GetAsync(int workID, Stream work)
        {
            ArgumentNullException.ThrowIfNull(work, nameof(work));

            // Запрос работы.
            await minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString())
                .WithCallbackStream(async stream => await stream.CopyToAsync(work)));

            // Проверка работы.
            if (work.Length <= 0) throw new Exception("Не удалось получить работу");
            work.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Проверка существования бакета.
        /// </summary>
        private async Task CheckBucketExistanceAsync()
        {
            if (await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket))) return;
            await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));
        }

        /// <summary>
        /// Сохранение работы.
        /// </summary>
        /// <param name="stream">Поток данных.</param>
        /// <param name="workID">id работы.</param>
        public async Task SaveAsync(Stream stream, int workID)
        {
            // Проверка наличия бакета.
            await CheckBucketExistanceAsync();

            // Добавление работы.
            await minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString())
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("text/plain"));

            // Проверка работы.
            ObjectStat stat = await minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString()));

            if (stat.Size <= 0) throw new Exception("Не удалось сохранить работу");
        }

        /// <summary>
        /// Удалить работу.
        /// </summary>
        /// <param name="workID">id работы.</param>
        public async Task DeleteAsync(int workID)
        {
            // Удаление работы.
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString()));

            ObjectStat stat;
            try
            {
                // Проверка работы.
                stat = await minioClient.StatObjectAsync(new StatObjectArgs()
                    .WithBucket(bucket)
                    .WithObject(workID.ToString()));
            }
            catch
            {
                return;
            }

            if (0 < stat.Size) throw new Exception("Не удалось удалить работу");
        }
    }
}
