using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using AutoFixture;

namespace DigitalDesign.Tests
{
    [TestClass()]
    public class WordCalculatorTests
    {
        [TestMethod()]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(1000)]
        public void GIVEN_generated_text_WHEN_CalculateWordsFullText_method_is_invoked_THEN_correct_wordmap_is_returned(int cycle)
        {
            //arrange
            var fixture = new Fixture();
            StringBuilder sb = new StringBuilder();
            Dictionary<string, int> actualMap = new Dictionary<string, int>();
            Random random = new Random();
            string[] wordSpaces = new string[4] {" ", "\n", "  ", "?" };

            for (int i = 1; i <= cycle; i++)
            {
                int randomWordCount = random.Next(1, 10);
                string word = fixture.Create<string>();
                for (int m = 1; m <= randomWordCount; m++)
                {
                    sb.Append(word);
                    sb.Append(wordSpaces[random.Next(0, 3)]);
                }
                actualMap.Add(word, randomWordCount);
            }

            var orderedActualMap = actualMap.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            WordCalculator calc = new WordCalculator(sb.ToString());

            //act
            var sut = calc.CalculateWordsFullText().OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            //assert
            Assert.IsTrue(Enumerable.SequenceEqual(sut, orderedActualMap));
        }
    }
}