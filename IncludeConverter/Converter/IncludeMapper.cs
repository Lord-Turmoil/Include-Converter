// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using IncludeConverter.Extensions;

namespace IncludeConverter.Converter;

internal class IncludeMapper
{
    private static readonly List<string> Extensions = new() { ".h", ".hpp", ".hxx", ".hh" };
    private readonly Dictionary<string, string> QuoteToAngleIndex = new();
    private readonly Dictionary<string, string> AngleToQuoteIndex = new();
    private Uri _includeDirecetory;

    private bool _silent;

    public IncludeMapper(bool silent)
    {
        _silent = silent;
    }

    public void BuildIndex(IEnumerable<string> includeDirectories)
    {
        foreach (string includeDirectory in includeDirectories)
        {
            BuildIndex(includeDirectory);
        }
    }

    public void BuildIndex(string includeDirectory)
    {
        Console.WriteLine("Building index for {0}...", includeDirectory);
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
        if (!_silent)
        {
            Console.WriteLine($"Indexing... {filename,-35}");
        }
        if (QuoteToAngleIndex.ContainsKey(filename))
        {
            if (!_silent)
            {
                Console.Write(" [Replace]");
            }
        }

        // Will add if not exists, and update if exists.
        string fullPath = new Uri(Path.GetFullPath(path)).AbsolutePath;
        var include = new Uri(fullPath);
        string target = _includeDirecetory.MakeRelativeUri(include).ToString();
        QuoteToAngleIndex[fullPath] = target;
        AngleToQuoteIndex[target] = fullPath;

        if (!_silent)
        {
            Console.WriteLine($" as {target}");
        }
    }
}