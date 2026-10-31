using System.Security.Cryptography;

namespace url.Application.Services;

public class ShortCodeGenerator
{
    public const string Characters="0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";  //Random Base64 technique 

    public const int CodeLength=10;

    public string Generate()
    {
        var result=new char[CodeLength];

        for(int i = 0; i < CodeLength; i++)
        {
            var index=RandomNumberGenerator.GetInt32(Characters.Length);

            result[i]=Characters[index];
        }

        return new string(result);
    }
}