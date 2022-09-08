using DigitalDesign;
using System.Data;
using System.Text;

class Program
{
    public static void Main(string[] args) 
    {
        Console.WriteLine("Введите путь к файлу.");
        string? text = null;

        try
        {
            text = File.ReadAllText(Console.ReadLine(), Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Нажмите Enter.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        int cpuCount = Environment.ProcessorCount;
        WordCalculator calculator = new WordCalculator(cpuCount, text);
        calculator.CalculateWordsFullText();
        var result = calculator.map.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        try
        {
            using (StreamWriter streamWriter = new StreamWriter("result.txt", false, Encoding.UTF8))
            {
                foreach (var item in result)
                {

                    streamWriter.WriteLine("{0,-30} {1,5}", item.Key, item.Value);
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Нажмите Enter.");
            Console.ReadLine();
            Environment.Exit(0);
        }
        Console.WriteLine("Результат подсчета слов записан в файл result.txt");
        Console.ReadLine();
    }
}
