using System;
using System.Security.Cryptography;
using System.Text;

// Abstract Builder
abstract class Builder
{
    public abstract void BuildHeader(string header);
    public abstract void BuildAuthors(string authors);
    public abstract void BuildContent(string content);
    public abstract void BuildHash(string hash);
    public abstract XMLDocument GetResult();
}

// Concrete Builder
class ConcreteBuilder : Builder
{
    private XMLDocument document = new XMLDocument();

    public override void BuildHeader(string header)
    {
        document.AddElement("Header", header);
    }

    public override void BuildAuthors(string authors)
    {
        document.AddElement("Authors", authors);
    }

    public override void BuildContent(string content)
    {
        document.AddElement("Content", content);
    }

    public override void BuildHash(string hash)
    {
        document.AddElement("Hash", hash);
    }

    public override XMLDocument GetResult()
    {
        return document;
    }
}

// Director
class Director
{
    private Builder builder;

    public Director(Builder builder)
    {
        this.builder = builder;
    }

    public void Construct(string header, string authors, string content, string hash)
    {
        builder.BuildHeader(header);
        builder.BuildAuthors(authors);
        builder.BuildContent(content);
        builder.BuildHash(hash);
    }
}

// Product
class XMLDocument
{
    private StringBuilder xml = new StringBuilder();

    public void AddElement(string tag, string content)
    {
        xml.AppendLine($"<{tag}>{content}</{tag}>");
    }

    public override string ToString()
    {
        return xml.ToString();
    }
}

// Validator
class Validator
{
    public static bool ValidateHash(string content, string expectedHash)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            string computedHash = Convert.ToBase64String(hashBytes);
            return computedHash == expectedHash;
        }
    }
}

// Client
class Program
{
    static void Main(string[] args)
    {
        // Sample TXT data
        string header = "Sample Article";
        string authors = "John Doe, Jane Smith";
        string content = "This is the content of the article.";
        string hash = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(content)));

        // Validate hash
        if (!Validator.ValidateHash(content, hash))
        {
            Console.WriteLine("Hash validation failed.");
            return;
        }

        // Build XML
        Builder builder = new ConcreteBuilder();
        Director director = new Director(builder);
        director.Construct(header, authors, content, hash);

        XMLDocument xmlDocument = builder.GetResult();
        Console.WriteLine("Generated XML:\n" + xmlDocument);
    }
}