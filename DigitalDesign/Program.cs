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
        catch (IOException ex)
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
            using (StreamWriter sw = new StreamWriter("result.txt", false, Encoding.UTF8))
            {
                foreach (var item in result)
                {
                    sw.WriteLine($"{item.Key} \t {item.Value}");
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
