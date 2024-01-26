namespace UrlShortener.Domain.Algorithm;

public interface IUrlShortenerAlgoritm
{
    string GenerateUniqueUrl();
}

public class UrlShortenerAlgoritm : IUrlShortenerAlgoritm
{
    private const int NumberOfCharsInShortUrl = 7;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private readonly Random _random = new();

    public string GenerateUniqueUrl()
    {
        var codeChars = new char[NumberOfCharsInShortUrl];

        for(int i = 0; i < NumberOfCharsInShortUrl; i++)
        {
            var randomIndex = _random.Next(Alphabet.Length - 1);

            codeChars[i] = Alphabet[randomIndex];
        }

        var code = new string(codeChars);

        return code;
    }
}