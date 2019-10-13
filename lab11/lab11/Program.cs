using System;

namespace l1
{
    class Program
    {
        static void Main(string[] args)
        {
            try{
                Task.Run(args[0], args[1], args[2]);
            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(".....");
            Console.ReadKey();
        }
    }
}
