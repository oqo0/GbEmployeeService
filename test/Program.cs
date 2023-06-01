using Microsoft.Data.SqlClient;

string connectionString = "server=localhost;port=3306;database=EmployeeDatabase;user id=root;password=Poi132poi_;";

string request = "SELECT * FROM EmployeeDatabase.Departments;";

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    Console.WriteLine(connection.Database);
}

