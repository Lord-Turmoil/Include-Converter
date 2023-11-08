// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using IncludeConverter.Extensions;

namespace IncludeConverter.Converter;

internal class IncludeMapper
{
    private static readonly List<string> Extensions = new() { ".h", ".hpp", ".hxx", ".hh" };
    private readonly Dictionary<string, string> QuoteToAngleIndex = new();
    private readonly Dictionary<string, string> AngleToQuoteIndex = new();
    private Uri _includeDirecetory;

    public void BuildIndex(IEnumerable<string> includeDirectories)
    {
        foreach (string includeDirectory in includeDirectories)
        {
            BuildIndex(includeDirectory);
        }
    }

    public void BuildIndex(string includeDirectory)
    {
        _includeDirecetory = new Uri(Path.GetFullPath(includeDirectory));

        DirectoryTraveler traveler = new(null, VisitFile);
        traveler.Traverse(includeDirectory);
    }

    public string? QuoteToAngle(string include)
    {
        return !QuoteToAngleIndex.ContainsKey(include) ? null : QuoteToAngleIndex[include];
    }

    public string? AngleToQuote(string include)
    {
        return !AngleToQuoteIndex.ContainsKey(include) ? null : AngleToQuoteIndex[include];
    }

    private void VisitFile(string path)
    {
        string extension = Path.GetExtension(path);
        if (!Extensions.Contains(extension))
        {
            return;
        }

        string filename = Path.GetFileName(path);
        Console.Write($"Indexing... {filename,-35}");
        if (QuoteToAngleIndex.ContainsKey(filename))
        {
            Console.Write(" [Replace]");
        }

        // Will add if not exists, and update if exists.
        var fullPath = new Uri(Path.GetFullPath(path)).AbsolutePath;
        var include = new Uri(fullPath);
        string target = _includeDirecetory.MakeRelativeUri(include).ToString();
        QuoteToAngleIndex[fullPath] = target;
        AngleToQuoteIndex[target] = fullPath;

        Console.WriteLine($" as {target}");
    }
}