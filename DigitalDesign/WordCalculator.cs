using System.Collections.Concurrent;
using System.Text;


namespace DigitalDesign
{
    public class WordCalculator
    {
        private int _taskCount = Environment.ProcessorCount;
        public string Text { get; set; }
        public ConcurrentDictionary<string, int> map = new ConcurrentDictionary<string, int>();
        public WordCalculator( string text)
        {
            Text = text;
        }

        private int[] GetTextPointers()
        {
            int pointerCount = _taskCount + 1;
            int[] textPointers = new int[pointerCount];
            textPointers[0] = 0;
            textPointers[pointerCount - 1] = Text.Length;

            for (int i = 1; i < textPointers.Length - 1; i++)
            {
                textPointers[i] = (Text.Length / _taskCount) * i;

                while (textPointers[i] < Text.Length && !Char.IsWhiteSpace(Text[textPointers[i]]))
                {
                    ++textPointers[i];
                }
                if (textPointers[i] >= Text.Length)
                {
                    textPointers[i] = Text.Length;
                    continue;
                }
            }
            return textPointers;
        }

        private void CalculateWordsTextPart(int start, int end)
        {
            StringBuilder Word = new StringBuilder();

            for (int i = start; i < end; i++)
            {
                char symbol = Text[i];

                if (Char.IsLetter(symbol) || Char.IsNumber(symbol))
                {
                    Word.Append(char.ToLower(symbol));
                }
                else if ((symbol =='\'' || symbol == '-' || Char.IsNumber(symbol)) && Word.Length > 0)
                {
                    Word.Append(symbol);
                }
                else if (Word.Length > 0)
                {
                    map.AddOrUpdate(Word.ToString(), 1, (key, old) => old + 1);
                    Word.Clear();
                }
            }
            if (Word.Length > 0)
            {
                map.AddOrUpdate(Word.ToString(), 1, (key, old) => old + 1);
                Word.Clear();
            }
        }

        public Dictionary<string, int> CalculateWordsFullText() 
        {
            Task[] tasks = new Task[_taskCount];
            int[] textPointers = GetTextPointers();

            for (int i = 0; i < tasks.Length; i++)
            {
                int start = i;
                int end = i + 1;

                tasks[i] = Task.Run(() => CalculateWordsTextPart(textPointers[start], textPointers[end]));
            }
            Task.WaitAll(tasks);
            return map.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value); ;
        }
    }
}
