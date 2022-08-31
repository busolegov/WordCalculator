using DigitalDesign;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
using System.Data;
using System.Text;

class Program
{
    public static void Main(string[] args) 
    {
        //Console.WriteLine("Введите путь к файлу.");
        string path = "C:\\Users\\6yc\\Desktop\\test-text2.txt";

        string? text = null;

        try
        {
            //text = File.ReadAllText(Console.ReadLine());
            text = File.ReadAllText(path);
        }
        catch (Exception)
        {

            Console.WriteLine("Ошибка чтения файла.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        Console.WriteLine(text.Length);

        int procCount = Environment.ProcessorCount;



        WordCalculator calculator = new WordCalculator(procCount, text);
        calculator.GetWords();

        var result = calculator.Map.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        foreach (var item in result)
        {
            Console.WriteLine($"{item.Value} - {item.Key}");
        }

        using (StreamWriter sw = new StreamWriter("result.txt", false, Encoding.UTF8))
        {
            foreach (var item in result)
            {
                sw.WriteLine($"{item.Key} - {item.Value}");
            }
        }

        Console.ReadLine();
    }
}
