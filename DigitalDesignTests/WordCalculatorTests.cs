using System.Collections.Concurrent;
using System.Linq;
using Xunit;

namespace DigitalDesign.Tests
{
    
    public class WordCalculatorTests
    {
        [Fact]

        public static void GetWordsTest()
        {
            //Arrange

            string text = "Костер открыто и резко направил острие своей книги против чёрных Сов реакции, против Силы и Коварства, царствующих не только в эпоху Филиппа Второго";
            int taskCount = Environment.ProcessorCount;
            WordCalculator calculator = new WordCalculator(taskCount, text);

            Dictionary<string, int> dict = new Dictionary<string, int>
            {
                { "костер", 1 },
                { "открыто", 1 },
                { "и", 2 },
                { "резко", 1 },
                { "направил", 1 },
                { "острие", 1 },
                { "своей", 1 },
                { "книги", 1 },
                { "против", 2 },
                { "чёрных", 1 },
                { "cов", 1 },
                { "реакции", 1 },
                { "cилы", 1 },
                { "коварства", 1 },
                { "царствующих", 1 },
                { "не", 1 },
                { "только", 1 },
                { "в", 1 },
                { "эпоху", 1 },
                { "филиппа", 1 },
                { "второго", 1 }
            };

            //Act

            calculator.GetWords();
            var sut = calculator.Map.AsEnumerable();



            //Assert
            Assert.True(dict.Any());
        }
    }
}