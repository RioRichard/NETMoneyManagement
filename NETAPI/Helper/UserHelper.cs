using System.Security.Cryptography;
using System.Text;
using NuGet.Common;

namespace NETAPI.Helper;

public static class UserHelper
{
    public static string HashPassword(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return "";
        }
        var hashByte = SHA512.HashData(Encoding.UTF8.GetBytes(text));
        return BitConverter.ToString(hashByte).Replace("-", "").ToLower();
    }
}