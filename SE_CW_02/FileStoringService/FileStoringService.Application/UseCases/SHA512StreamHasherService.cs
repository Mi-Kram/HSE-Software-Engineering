using FileStoringService.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace FileStoringService.Application.UseCases
{
    /// <summary>
    /// Хэширования файла SHA512.
    /// </summary>
    public class SHA512StreamHasherService : IStreamHasherService
    {
        private readonly SHA512 sha256 = SHA512.Create();

        /// <summary>
        /// Хэширует поток данных. 
        /// </summary>
        /// <param name="stream">Поток данных.</param>
        /// <returns>Значение хэша. Null при ошибке.</returns>
        public async Task<string?> HashAsync(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream, nameof(stream));

            byte[] bytes = await sha256.ComputeHashAsync(stream);

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0) ++bytes[i];
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
