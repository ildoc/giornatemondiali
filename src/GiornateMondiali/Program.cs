using GiornateMondiali.Core;

namespace GiornateMondiali
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var d in GmCore.GetSpecialDays(2023))
                Console.WriteLine(d);
            Console.ReadLine();
        }
    }
}