using System;

namespace TestExitCode
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");
                //Environment.ExitCode = 1603;
                throw new Exception("agafaf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"afafaf code: {Environment.ExitCode}");
                throw;
            }
        }
    }
}
