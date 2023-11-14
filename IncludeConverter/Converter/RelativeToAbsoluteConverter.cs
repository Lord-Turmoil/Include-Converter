// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using IncludeConverter.Extensions;

namespace IncludeConverter.Converter;

internal class RelativeToAbsoluteConverter : IncludeConverter
{
    private readonly IncludeMapper _mapper;

    public RelativeToAbsoluteConverter(IncludeMapper mapper, bool silent)
        : base(silent)
    {
        _mapper = mapper;
    }

    public override void Convert(string sourceDirectory)
    {
        Console.WriteLine("Converting {0}...", sourceDirectory);
        DirectoryTraveler traveler = new(null, VisitFile);
        traveler.Traverse(sourceDirectory);
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
            Console.WriteLine($"Converting... {filename,-35}");
        }

        ModifySourceFile(path);
    }


    private void ModifySourceFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        List<string> newLines = new();
        bool overwrite = false;
        foreach (string line in lines)
        {
            string newLine;
            if (IsInclude(line))
            {
                if (ConvertInclude(line, path, out newLine))
                {
                    overwrite = true;
                    if (!_silent)
                    {
                        Console.WriteLine(newLine);
                    }
                }
            }
            else
            {
                newLine = line;
            }

            newLines.Add(newLine);
        }

        if (overwrite)
        {
            File.WriteAllLines(path, newLines);
        }
    }

    private bool ConvertInclude(string include, string path, out string newInclude)
    {
        int begin = include.IndexOf('"');
        int end = include.IndexOf('"', begin + 1);
        if (begin == -1 || end == -1)
        {
            newInclude = include;
            return false;
        }

        string header = include.Substring(begin + 1, end - begin - 1);

        var baseUri = new Uri(Path.GetFullPath(path));
        var headerUri = new Uri(baseUri, header);
        string? target = _mapper.QuoteToAngle(headerUri.AbsolutePath);
        if (target == null)
        {
            // If it is already an absolute, just wrapped with ""
            if (_mapper.AngleToQuote(header) != null)
            {
                target = header;
            }
            else
            {
                newInclude = include;
                return false;
            }
        }

        newInclude = include[..begin] + '<' + target + '>' + include[(end + 1)..];

        return true;
    }
}