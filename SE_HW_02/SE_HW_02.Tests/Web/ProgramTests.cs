using SE_HW_02.Web;

namespace SE_HW_02.Tests.Web
{
    public class ProgramTests
    {
        [Fact]
        public void Test()
        {
            Assert.NotNull(Program.InitApp([]));
        }
    }
}
