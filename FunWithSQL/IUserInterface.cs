using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunWithSQL
{
    public interface IUserInterface
    {
        void Write(string message);
        string ReadLine();
        char ReadKey();
        void Clear();
    }

    public class ConsoleUserInterface : IUserInterface
    {
        public void Write(string message) => Console.WriteLine(message);
        public string ReadLine()
        {
            return Console.ReadLine();
        }
        public char ReadKey() => Console.ReadKey().KeyChar;
        public void Clear() => Console.Clear();
    }

}
