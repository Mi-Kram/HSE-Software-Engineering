using FileStoringService.Application.Interfaces;
using FileStoringService.Application.UseCases;
using FileStoringService.Domain.Entities;
using FileStoringService.Domain.Exceptions;
using FileStoringService.Domain.Interfaces;
using FileStoringService.Domain.Interfaces.Scripts;
using FileStoringService.Domain.Models;
using Moq;

namespace FileStoringService.Tests.Application.UseCases
{
    public class WorkServiceTests
    {
        private static WorkInfo CloneWork(WorkInfo work) => new()
        {
            ID = work.ID,
            UserID = work.UserID,
            Uploaded = work.Uploaded,
            Hash = work.Hash
        };

        public static (IWorkRepository, IWorkStorageService) GetServices(List<(WorkInfo info, string data)> works)
        {
            Mock<IWorkRepository> mockRepository = new();
            Mock<IWorkStorageService> mockStorage= new();

            {
                mockRepository.Setup(x => x.GetAllAsync()).Returns(() => Task.FromResult(works.Select(x => CloneWork(x.info))));

                mockRepository.Setup(x => x.GetAsync(It.IsAny<int>())).Returns((int arg) =>
                {
                    foreach ((WorkInfo work, _) in works)
                    {
                        if (work.ID == arg)
                        {
                            return Task.FromResult<WorkInfo?>(CloneWork(work));
                        }
                    }

                    return Task.FromResult<WorkInfo?>(null);
                });

                mockRepository.Setup(x => x.GetAllByHashAsync(It.IsAny<string>()))
                    .Returns((string arg) => Task.FromResult(works
                    .Where(x => x.info.Hash == arg)
                    .Select(x => CloneWork(x.info))));

                mockRepository.Setup(x => x.GetAllByUserIDAsync(It.IsAny<int>()))
                    .Returns((int arg) => Task.FromResult(works
                    .Where(x => x.info.UserID == arg)
                    .Select(x => CloneWork(x.info))));

                mockRepository.Setup(x => x.GetAddScriptAsync())
                    .Returns(() => Task.FromResult(new Mock<IAddWorkScript>().Object));

                mockRepository.Setup(x => x.GetRemoveScriptAsync())
                    .Returns(() => Task.FromResult(new Mock<IRemoveWorkScript>().Object));
            }

            {
                mockStorage.Setup(x => x.GetAsync(It.IsAny<int>(), It.IsAny<Stream>()))
                    .Returns((int arg1, Stream arg2) =>
                    {
                        foreach (var item in works)
                        {
                            if (item.info.ID != arg1) continue;
                            using (StreamWriter sw = new(arg2, leaveOpen: true)) sw.Write(item.data);
                            arg2.Seek(0, SeekOrigin.Begin);
                            break;
                        }
                        return Task.CompletedTask;
                    });

                mockStorage.Setup(x => x.SaveAsync(It.IsAny<Stream>(), It.IsAny<int>()))
                    .Returns((Stream arg1, int arg2) =>
                    {
                        for (int i = 0; i < works.Count; i++)
                        {
                            if (works[i].info.ID != arg2) continue;
                            using StreamReader sr = new(arg1, leaveOpen: true);
                            works[i] = (works[i].info, sr.ReadToEnd());
                            break;
                        }
                        return Task.CompletedTask;
                    });

                mockStorage.Setup(x => x.DeleteAsync(It.IsAny<int>()))
                    .Returns((int arg1) =>
                    {
                        for (int i = 0; i < works.Count; i++)
                        {
                            if (works[i].info.ID == arg1)
                            {
                                works[i] = (works[i].info, "");
                                break;
                            }
                        }
                        return Task.CompletedTask;
                    });
            }

            return (mockRepository.Object, mockStorage.Object);
        }

        public static IStreamHasherService GetHasher() => new SHA512StreamHasherService();

        [Fact]
        public async Task GetAllWorksAsync_Tests()
        {
            List<(WorkInfo info, string data)> works = [];
            var (repository, storage) = GetServices(works);
            WorkService workService = new(repository, GetHasher(), storage);

            Assert.Empty(await workService.GetAllWorksAsync());

            works.Add((new WorkInfo(), ""));
            Assert.Single(await workService.GetAllWorksAsync());
        }

        [Theory]
        [InlineData(5)]
        public async Task GetWorkAsync_Tests(int workID)
        {
            var (repository, storage) = GetServices([(new WorkInfo() { ID = 5 }, "")]);
            WorkService workService = new(repository, GetHasher(), storage);

            Assert.NotNull(await workService.GetWorkAsync(workID));
            Assert.Null(await workService.GetWorkAsync(workID + 1));
        }

        [Fact]
        public async Task UploadWorkAsync_Success_Tests()
        {
            List<(WorkInfo info, string data)> works = [];
            var (repository, storage) = GetServices(works);
            WorkService workService = new(repository, GetHasher(), storage);

            using MemoryStream ms = new();
            using (StreamWriter sw = new(ms, leaveOpen: true))
                await sw.WriteAsync("work data");
            ms.Seek(0, SeekOrigin.Begin);

            await workService.UploadWorkAsync(ms, new UploadWorkData() { UserID = 5, Title = "file.txt" });
        }

        [Theory]
        [InlineData(5, "work data")]
        public async Task UploadWorkAsync_Fail_Tests(int userID, string data)
        {
            IStreamHasherService hasher = GetHasher();
            List<(WorkInfo info, string data)> works = [];

            using MemoryStream ms = new();
            using (StreamWriter sw = new(ms, leaveOpen: true))
                await sw.WriteAsync(data);
            ms.Seek(0, SeekOrigin.Begin);

            works.Add((new WorkInfo() { UserID = userID, Hash = await hasher.HashAsync(ms) ?? "" }, data));
            ms.Seek(0, SeekOrigin.Begin);

            var (repository, storage) = GetServices(works);
            WorkService workService = new(repository, hasher, storage);

            using MemoryStream empty = new();

            UploadWorkData uploadData = new UploadWorkData() { UserID = userID, Title = "file.txt" };
            await Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await workService.UploadWorkAsync(null!, uploadData));

            await Assert.ThrowsAsync<ArgumentException>(async ()
                => await workService.UploadWorkAsync(empty, uploadData));

            await Assert.ThrowsAsync<WorkAlreadyUploadedException>(async ()
                => await workService.UploadWorkAsync(ms, uploadData));
        }

        [Theory]
        [InlineData(5, "work data")]
        public async Task DownloadWorkAsync_Tests(int id, string data)
        {
            List<(WorkInfo info, string data)> works = [(new WorkInfo() { ID = id }, data)];
            var (repository, storage) = GetServices(works);
            WorkService workService = new(repository, GetHasher(), storage);

            using MemoryStream ms = new();
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await workService.DownloadWorkAsync(id + 1, null!));
            await Assert.ThrowsAsync<ArgumentException>(async () => await workService.DownloadWorkAsync(id + 1, ms));

            await workService.DownloadWorkAsync(id, ms);
            ms.Seek(0, SeekOrigin.Begin);

            using StreamReader sr = new(ms);
            Assert.Equal(data, await sr.ReadToEndAsync());
        }

        [Fact]
        public async Task DeleteWorkAsync_Tests()
        {
            List<(WorkInfo info, string data)> works = [(new WorkInfo() { ID = 5 }, "")];
            var (repository, storage) = GetServices(works);
            WorkService workService = new(repository, GetHasher(), storage);

            await Assert.ThrowsAsync<ArgumentException>(async () => await workService.DeleteWorkAsync(6));
            await workService.DeleteWorkAsync(5);
        }
    }
}
