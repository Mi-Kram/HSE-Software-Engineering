namespace Main.Tests
{
    public class RandomVetClinicTests
    {
        [Theory]
        [InlineData(1_000_000, 0.13f, 0.01f)]
        [InlineData(1_000_000, 0.27f, 0.01f)]
        [InlineData(1_000_000, 0.56f, 0.01f)]
        [InlineData(1_000_000, 0.75f, 0.01f)]
        public void IsHealthy_ShouldReturnTrueWithCertainProbability(int total, float target, float delta)
        {
            int success = 0;
            RandomVetClinic vetClinic = new(target);

            for (int i = 0; i < total; i++)
                if (vetClinic.IsHealthy(null!)) 
                    ++success;

            float result = (float)success / total;
            Assert.InRange(result, target - delta, target + delta);
        }
    }
}
