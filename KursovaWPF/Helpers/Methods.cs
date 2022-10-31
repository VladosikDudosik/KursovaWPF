using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
namespace KursovaWPF.Helpers
{
    public static class Methods
    {
        public static string TextToSHA256(string text)
        {
            StringBuilder res = new StringBuilder();
            byte[] bytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
            for(int i = 0; i < bytes.Length; i++)
            {
                res.Append(bytes[i].ToString("x2"));   
            }
            return res.ToString();
        }
    }
}
