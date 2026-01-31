using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace GHAS
{
    // Intentional insecure patterns for static analysis testing (use only in a test branch).
    public class Class1
    {
        // 1) SQL injection via string concatenation
        public void SqlInjection(string userInput)
        {
            // fake connection string placeholder (do not use in production)
            var connStr = "Server=localhost;Database=test;User Id=test;Password=test;";
            using var conn = new SqlConnection(connStr);
            var sql = "SELECT * FROM Users WHERE name = '" + userInput + "';"; // vulnerable
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read()) { /* ... */ }
        }

        // 2) Command injection via Process.Start with external input
        public void CommandInjection(string cmdArg)
        {
            // Dangerous: passing untrusted input to shell
            var psi = new ProcessStartInfo("cmd.exe", "/c " + cmdArg)
            {
                UseShellExecute = false
            };
            Process.Start(psi);
        }

        // 3) Insecure SSL validation (accept all certificates)
        public HttpClient InsecureSsl()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; // insecure
            return new HttpClient();
        }

        // 4) Hardcoded credentials / plaintext secrets (fake placeholder)
        public string GetHardcodedSecret()
        {
            // FAKE_TEST_SECRET used only for static detection tests
            string apiKey = "FAKE_TEST_SECRET_XXXXXXXXXXXXXXXX";
            return apiKey;
        }

        // 5) Insecure deserialization using BinaryFormatter
        public object InsecureDeserialize(byte[] payload)
        {
            var bf = new BinaryFormatter(); // unsafe
            using var ms = new MemoryStream(payload);
            return bf.Deserialize(ms);
        }

        // 6) Weak cryptography: MD5 for password hashing
        public string WeakHash(string password)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = md5.ComputeHash(bytes); // weak algorithm
            return Convert.ToHexString(hash);
        }

        // 7) Predictable token using System.Random
        public string PredictableToken()
        {
            var rnd = new Random(1234); // predictable seed
            return rnd.Next().ToString();
        }

        // 8) Unsafe HTML constructed from library/untrusted input (XSS)
        public string UnsafeHtmlFromLibraryInput(string libraryInput)
        {
            // Assume libraryInput comes from a third-party library or external source and is not HTML-encoded.
            // Vulnerable: directly embedding untrusted content into HTML can lead to XSS.
            var html = "<form action=\"/submit\" method=\"post\">" +
                       "<label>Name</label><input name=\"name\" value=\"" + libraryInput + "\" />" +
                       "<label>Comment</label><textarea name=\"comment\">" + libraryInput + "</textarea>" +
                       "<button type=\"submit\">Send</button>" +
                       "</form>";
            return html;
        }
    }
}
