using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 4 - Star 1, Answer: {0}", MineAdventCoins("bgvyzdsv"));
            Console.WriteLine("Day 4 - Star 2, Answer: {0}", MineAdventCoins("bgvyzdsv", 6));
            Console.ReadKey();
        }

        static int MineAdventCoins(string key, int zeros = 5)
        {
            int i = 0;
            string startsWith = string.Empty.PadLeft(zeros, '0');

            using (MD5 md5Hasher = MD5.Create())
            {
                do
                {
                    string hash = GetMd5HashBitConvertor(md5Hasher, string.Format("{0}{1}", key, i));

                    if (hash.StartsWith(startsWith, StringComparison.Ordinal))
                    {
                        break;
                    }

                    i++;
                } while (true);
            }

            return i;
        }

        static string GetMd5HashBitConvertor(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            return BitConverter.ToString(data).Replace("-", string.Empty);
        }
    }
}
