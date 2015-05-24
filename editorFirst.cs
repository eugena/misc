using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.DataProvider.SQLite;

/**
 * Приложение поиска вариантов строк по фрагменту.
 * В качестве хранилища используется база данных data.db.
 */
namespace konturEditorFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0) {
                try {
                    /**
                     * Если задана строка, соединяемся с базой, выполняем запрос, 
                     * затем выводим найденные строки
                     */
                    const string connTemplate = "Data Source={0};Version=3;FailIfMissing=True";
                    string str = args[0];
                    string dbName = "data.db";
                    if (args.Length > 1) {
                        dbName = args[1];
                    }
                    using (var db = SQLiteTools.CreateDataConnection(string.Format(connTemplate, dbName))) {
                        var result =
                            (from item in db.GetTable<Data>()
                             where item.Word.StartsWith(str)
                             orderby item.Frequency descending, item.Word
                             select item.Word).Take(10);
                        foreach (string item in result) {
                            Console.WriteLine(item);
                        }
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

        /**
         * Маппинг таблицы
         */
        [Table(Name = "data")]
        public class Data
        {
            [Column(Name = "word")]
            public string Word { get; set; }

            [Column(Name = "frequency")]
            public int Frequency { get; set; }
        }
    }
}
