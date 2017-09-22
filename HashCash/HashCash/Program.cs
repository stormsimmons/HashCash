using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HashCash
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var check = Verify("1:2:" + DateTime.Now.ToString("YYMMDD") + ":GetPDF::");
            watch.Stop();
            Console.WriteLine(check ? "valid" : "Invalid");
            Console.WriteLine(watch.Elapsed);
            Console.ReadLine();
        }

        public static bool Verify(string header)
        {
            var isValid = false;
            var counter = 0;
            var random = new Random();
            var text = header + Convert.ToBase64String(BitConverter.GetBytes(random.Next(int.MaxValue)));
            while (!isValid)
            {
                int NumOfBits = int.Parse((text + ":" + Convert.ToBase64String(BitConverter.GetBytes(counter))).Substring(2, 1));
                byte[] arr = Enumerable.Repeat((byte)0x00, NumOfBits).ToArray();
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes((text + ":" + Convert.ToBase64String(BitConverter.GetBytes(counter)))));

                isValid = hash.Take(NumOfBits).SequenceEqual(arr);
                counter++;
            }
            Console.WriteLine(counter);
            return isValid;
        }
    }
}