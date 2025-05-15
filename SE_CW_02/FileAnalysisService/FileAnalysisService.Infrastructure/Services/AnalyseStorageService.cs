using FileAnalysisService.Domain.Interfaces;
using Minio.DataModel.Args;
using Minio.DataModel;
using Minio;
using FileAnalysisService.Domain.Exceptions;
using FileAnalysisService.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace FileAnalysisService.Infrastructure.Services
{
    /// <summary>
    /// Сервис для хранения объектов анализа.
    /// </summary>
    public class AnalyseStorageService(IMinioClient minioClient, IConfiguration configuration) : IAnalyseStorageService
    {
        private readonly IMinioClient minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
        private readonly string bucket = configuration?.GetSection(ApplicationVariables.WORDS_CLOUD_SRORAGE_BUCKET)?.Value ?? throw new EnvVariableException(ApplicationVariables.WORDS_CLOUD_SRORAGE_BUCKET);

        /// <summary>
        /// Получение картинки облака слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="image">Поток изображения.</param>
        public async Task<string> GetWordsCloudImageAsync(int workID, Stream image)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            // Проверка изображения.
            ObjectStat stat = await minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString()));
            if (stat.Size <= 0) throw new Exception("Не удалось получить изображение");

            // Запрос изобрадения.
            await minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString())
                .WithCallbackStream(async stream => await stream.CopyToAsync(image)));

            // Проверка изображения.
            if (image.Length <= 0) throw new Exception("Не удалось получить изображение");
            
            image.Seek(0, SeekOrigin.Begin);
            return stat.ContentType;
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
        /// Сохранение картинки облака слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        /// <param name="image">Поток изображения.</param>
        /// <param name="contentType">Тип изображения.</param>
        public async Task SaveWordsCloudImageAsync(int workID, Stream image, string contentType)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));
            ArgumentNullException.ThrowIfNullOrWhiteSpace(contentType, nameof(contentType));

            // Проверка наличия бакета.
            await CheckBucketExistanceAsync();

            // Добавление изображения.
            await minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString())
                .WithStreamData(image)
                .WithObjectSize(image.Length)
                .WithContentType(contentType));

            // Проверка изображения.
            ObjectStat stat = await minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(workID.ToString()));

            if (stat.Size <= 0) throw new Exception("Не удалось сохранить работу");
        }

        /// <summary>
        /// Удаление картинки облака слов.
        /// </summary>
        /// <param name="workID">id работы.</param>
        public async Task DeleteWordsCloudImageAsync(int workID)
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
