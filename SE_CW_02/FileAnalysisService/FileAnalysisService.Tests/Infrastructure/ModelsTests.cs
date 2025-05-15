using FileAnalysisService.Infrastructure.Models;

namespace FileAnalysisService.Tests.Infrastructure
{
    public class ModelsTests
    {
        [Fact]
        public void WordsCloudOptions_Tests()
        {
            WordsCloudOptions options = new()
            {
                BackgroundColor = "abc",
                Case = "abc",
                CleanWords = true,
                FontFamily = "abc",
                FontScale = 55,
                FontWeight = "abc",
                Format = "abc",
                Height = 55,
                Language = "abc",
                LoadGoogleFonts = "abc",
                MaxNumWords = 55,
                MinWordLength = 55,
                Padding = 55,
                RemoveStopwords = true,
                Rotation = 55,
                Scale = "abc",
                Text = "abc",
                UseWordList = true,
                Width = 55
            };

            Assert.NotNull(options.BackgroundColor);
            Assert.NotNull(options.Case);
            Assert.True(options.CleanWords);
            Assert.NotNull(options.FontFamily);
            Assert.Equal(55, options.FontScale);
            Assert.NotNull(options.FontWeight);
            Assert.NotNull(options.Format);
            Assert.Equal(55, options.Height);
            Assert.NotNull(options.Language);
            Assert.NotNull(options.LoadGoogleFonts);
            Assert.Equal(55, options.MaxNumWords);
            Assert.Equal(55, options.MinWordLength);
            Assert.Equal(55, options.Padding);
            Assert.True(options.RemoveStopwords);
            Assert.Equal(55, options.Rotation);
            Assert.NotNull(options.Scale);
            Assert.NotNull(options.Text);
            Assert.True(options.UseWordList);
            Assert.Equal(55, options.Width);
        }


    }
}
