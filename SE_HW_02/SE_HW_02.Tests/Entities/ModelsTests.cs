using SE_HW_02.Entities.Models;

namespace SE_HW_02.Entities.Tests
{
    public class ModelsTests
    {
        [Theory]
        [InlineData(3, "name", "type", 2000, 7, 13, true, true, "Meat", 7)]
        public void AnimalTests(int id, string name, string type, int year, int month, int day, bool isMale, bool isHealthy, string food, int enclosureID)
        {
            DateTime birthday = new DateTime(year, month, day);

            Animal animal = new Animal()
            {
                ID = id,
                Name = name,
                Type = type,
                Birthday = birthday,
                IsMale = isMale,
                IsHealthy = isHealthy,
                FavoriteFood = food,
                EnclosureID = enclosureID
            };

            Assert.Equal(id, animal.ID);
            Assert.Equal(name, animal.Name);
            Assert.Equal(type, animal.Type);
            Assert.Equal(birthday, animal.Birthday);
            Assert.Equal(isMale, animal.IsMale);
            Assert.Equal(isHealthy, animal.IsHealthy);
            Assert.Equal(food, animal.FavoriteFood);
            Assert.Equal(enclosureID, animal.EnclosureID);

            Assert.Throws<ArgumentNullException>(() => animal.Name = null!);
            Assert.Throws<ArgumentNullException>(() => animal.Type = null!);
            Assert.Throws<ArgumentNullException>(() => animal.FavoriteFood = null!);

            Animal clone = animal.Clone();
            Assert.Equal(clone.ID, animal.ID);
            Assert.Equal(clone.Name, animal.Name);
            Assert.Equal(clone.Type, animal.Type);
            Assert.Equal(clone.Birthday, animal.Birthday);
            Assert.Equal(clone.IsMale, animal.IsMale);
            Assert.Equal(clone.IsHealthy, animal.IsHealthy);
            Assert.Equal(clone.FavoriteFood, animal.FavoriteFood);
            Assert.Equal(clone.EnclosureID, animal.EnclosureID);
        }

        [Theory]
        [InlineData(5, 3, 4.2f)]
        public void DimensionTests(float width, float length, float height)
        {
            Dimensions dim = new Dimensions()
            {
                Width = width,
                Length = length,
                Height = height
            };


            Assert.Equal(width, dim.Width);
            Assert.Equal(length, dim.Length);
            Assert.Equal(height, dim.Height);

            Assert.Throws<ArgumentException>(() => dim.Width = -1);
            Assert.Throws<ArgumentException>(() => dim.Length = -1);
            Assert.Throws<ArgumentException>(() => dim.Height = -1);
        }

        [Theory]
        [InlineData(3, "type", 20, 30, 12, 5, 7)]
        public void EnclosureTests(int id, string type, float width, float length, float height, int amount, int capacity)
        {
            Dimensions size = new Dimensions()
            {
                Width = width,
                Length = length,
                Height = height
            };

            Enclosure enclosure = new Enclosure()
            {
                ID = id,
                Type = type,
                Size = size,
                AnimalsAmount = amount,
                AnimalsCapacity = capacity
            };

            Assert.Equal(id, enclosure.ID);
            Assert.Equal(type, enclosure.Type);
            Assert.Equal(size, enclosure.Size);
            Assert.Equal(amount, enclosure.AnimalsAmount);
            Assert.Equal(capacity, enclosure.AnimalsCapacity);

            Assert.Throws<ArgumentNullException>(() => enclosure.Type = null!);
            Assert.Throws<ArgumentException>(() => enclosure.AnimalsAmount = -1);
            Assert.Throws<ArgumentException>(() => enclosure.AnimalsCapacity= -1);

            Enclosure clone = enclosure.Clone();
            Assert.Equal(clone.ID, enclosure.ID);
            Assert.Equal(clone.Type, enclosure.Type);
            Assert.Equal(clone.Size, enclosure.Size);
            Assert.Equal(clone.AnimalsAmount, enclosure.AnimalsAmount);
            Assert.Equal(clone.AnimalsCapacity, enclosure.AnimalsCapacity);
        }

        [Theory]
        [InlineData(3, 5, 12, 13, 24, "food")]
        public void FeedingScheduleTests(int id, int animalID, int hours, int minutes, int seconds, string food)
        {
            TimeOnly time = new TimeOnly(hours, minutes, seconds);

            FeedingSchedule schedule = new FeedingSchedule()
            {
                ID = id,
                AnimalID = animalID,
                Time = time,
                Food = food
            };

            Assert.Equal(id, schedule.ID);
            Assert.Equal(animalID, schedule.AnimalID);
            Assert.Equal(time, schedule.Time);
            Assert.Equal(food, schedule.Food);

            Assert.Throws<ArgumentNullException>(() => schedule.Food = null!);

            FeedingSchedule clone = schedule.Clone();
            Assert.Equal(clone.ID, schedule.ID);
            Assert.Equal(clone.AnimalID, schedule.AnimalID);
            Assert.Equal(clone.Time, schedule.Time);
            Assert.Equal(clone.Food, schedule.Food);
        }

    }
}
