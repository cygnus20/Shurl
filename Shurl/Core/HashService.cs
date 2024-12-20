﻿using System.Security.Cryptography;
using System.Text;

namespace Shurl.Core;

public class HashService
{
    public string ComputeSha256(string input)
    {
        // Creata a SHA 256
        using (SHA256 sha256 = SHA256.Create())
        {
            // ComputeHash - returns byte array
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert byte array to a string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
