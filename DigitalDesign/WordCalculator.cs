﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Text;


namespace DigitalDesign
{
    public class WordCalculator
    {
        public int TaskCount { get; set; }
        public string Text { get; set; }

        public ConcurrentDictionary<string, int> Map = new ConcurrentDictionary<string, int>();

        public WordCalculator(int cpuCores, string text)
        {
            TaskCount = cpuCores;
            Text = text;
        }

        private int[] GetTextPointers()
        {
            int pointerCount = TaskCount + 1;
            int[] textPointers = new int[pointerCount];
            textPointers[0] = 0;
            textPointers[pointerCount - 1] = Text.Length;

            for (int i = 1; i < textPointers.Length - 1; i++)
            {
                textPointers[i] = (Text.Length / TaskCount) * i;

                if (textPointers[i] >= Text.Length)
                {
                    textPointers[i] = Text.Length;
                    continue;
                }
                while (textPointers[i] < Text.Length && !Char.IsWhiteSpace(Text[textPointers[i]]))
                {
                    ++textPointers[i];
                }
            }
            return textPointers;
        }

        //test




        private void Count(int start, int end)
        {
            StringBuilder Word = new StringBuilder();

            for (int i = start; i < end; i++)
            {
                char symbol = Text[i];

                if (Char.IsLetter(symbol))
                {
                    Word.Append(char.ToLower(symbol));
                }
                else if ((symbol == '-') && Word.Length > 0)
                {
                    Word.Append(symbol);
                }
                else if (Word.Length > 0)
                {
                    Map.AddOrUpdate(Word.ToString(), 1, (key, old) => old + 1);
                    Word.Clear();
                }
            }
            if (Word.Length > 0)
            {
                Map.AddOrUpdate(Word.ToString(), 1, (key, old) => old + 1);
                Word.Clear();
            }
        }


        public void GetWords() 
        {
            Task[] tasks = new Task[TaskCount];
            int[] textPointers = GetTextPointers();

            for (int i = 0; i < tasks.Length; i++)
            {
                int start = i;
                int end = i + 1;

                tasks[i] = Task.Run(() => Count(textPointers[start], textPointers[end]));
            }
            Task.WaitAll(tasks);
        }


    }
}