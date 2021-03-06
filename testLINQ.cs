using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.DataProvider.SQLite;

/**
 *   <connectionStrings>
 *       <add name="Data"
 *         connectionString = "Data Source=data.db;Version=3;FailIfMissing=True"
 *         providerName     = "SQLite" />
 *     </connectionStrings>
 */
namespace testLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Context())
            {
                var result =
                    (from item in db. Data
                     where item.Word.StartsWith(str)
                     orderby item.Frequency descending, item.Word
                     select item.Word).Take(10);

                foreach (var item in result) {
                    Console.WriteLine(item);
                }
            }
        }

        [Table(Name = "data")]
        public class Data
        {
            [Column(Name = "word")]
            public string Word { get; set; }

            [Column(Name = "frequency")]
            public int Frequency { get; set; }
        }

        public class Context : LinqToDB.Data.DataConnection
        {
            public Context() : base("Data") { }
            public LinqToDB.ITable<Data> Data { get { return GetTable<Data>(); } }
        }
    }
}
