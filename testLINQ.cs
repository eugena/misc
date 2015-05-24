using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Mapping;
using LinqToDB.DataProvider.SQLite;


namespace testLINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Context())
            {
                var q =
                    (from c in db.Data
                    select c).Take(10);
                foreach (var c in q)
                    Console.WriteLine(c.Word);
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
