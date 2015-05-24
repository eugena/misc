using System;
using System.Collections.Generic;
using System.Linq;


/**
 * Приложение поиска вариантов строк по фрагменту.
 * В качестве хранилища используется текстовый файл test.in.
 */
namespace konturEditorSecond
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)  {
                /**
                 * Если задана строка, читаем файл, готовим данные для LINQ в виде Dictionary, 
                 * затем выводим найденные строки
                 */
                try {
                    string str = args[0];
                    string fileName = "test.in";
                    if (args.Length > 1) {
                        fileName = args[1];
                    }

                    string[] lines = System.IO.File.ReadAllLines(fileName);
                    uint n = Convert.ToUInt32(lines[0]);
                    Dictionary<string, uint> Data = new Dictionary<string, uint>();
                    for (int i = 1; i <= n; i++) {
                        string[] d = lines[i].Split(' ');
                        Data.Add(d[0], Convert.ToUInt16(d[1]));
                    }
                    var result =
                        (from item in Data
                         where item.Key.StartsWith(str)
                         orderby item.Value descending, item.Key
                         select item.Key).Take(10);
                    foreach (string item in result) {
                        Console.WriteLine(item);
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.ToString());
                }
            } else {
                /**
                 * Если строка не задана, предполагаем, что пользователь не знает, что делать 
                 * - выводим справку
                 */
                Console.WriteLine("Программа автоподбора вариантов строк по фрагменту.");
                Console.WriteLine("Фрагмент строки является обязательным параметром.");
                Console.WriteLine("Вторым (необязательным) параметром является путь к файлу с данными.");
            }
        }
    }
}
