using Npgsql;

namespace Test_Task_Datetime
{
    public class DBQuery
    {
        public DateOnly Date { get; set; }

        public static void SelectData()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM your_table", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader.GetInt32(0)} - {reader.GetString(1)}");
                        }
                    }
                }
            }
        }
        public static void InsertData(string name, int age)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO newtable (name, age) VALUES (@name, @age)";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Data inserted successfully.");
                }
            }
        }  
    }
}
