using SE_HW_02.Infrastructure.Repositories;

namespace SE_HW_02.Tests.Infrastructure.Repositories
{
    public class IdCounterTests
    {
        [Fact]
        public void Test()
        {
            IdCounter counter = new();

            int current = counter.Next();
            for (int i = 0; i < 15; i++)
            {
                Assert.Equal(++current, counter.Next());
            }
        }
    }
}
