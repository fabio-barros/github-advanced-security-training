using System;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Data.SqlClient;
namespace GHAS
{
    public class FileOperations
    {
        // Insecure file reading
        public string ReadUserFile(string userInput)
        {
            string content = File.ReadAllText(userInput);
            return content;
        }

        // SQL injection vulnerability
        public void ExecuteQuery(string userId)
        {
            string connectionString = "Server=myServer;Database=myDB;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Id = '" + userId + "'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Path traversal vulnerability
        public void WriteToFile(string fileName, string content)
        {
            string fullPath = Path.Combine("C:\\data\\", fileName);
            File.WriteAllText(fullPath, content);
        }

        // Hardcoded credentials
        public void ConnectToDatabase()
        {
            string password = "SuperSecret123!";
            string connectionString = $"Server=myServer;Database=myDB;User=admin;Password={password}";
            // Connection logic here
        }
    }
}