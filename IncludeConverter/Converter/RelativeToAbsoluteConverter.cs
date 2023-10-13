using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncludeConverter.Extensions;
using IncludeConverter.Mapper;
using Microsoft.VisualBasic.CompilerServices;

namespace IncludeConverter.Converter;

internal class RelativeToAbsoluteConverter : IncludeConverter
{
    private readonly RelativeToAbsoluteMapper _mapper;

    public RelativeToAbsoluteConverter(RelativeToAbsoluteMapper mapper)
    {
        _mapper = mapper;
    }

    public override void Convert(string sourceDirectory)
    {
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
        Console.WriteLine($"Converting... {filename,-35}");

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
                    Console.WriteLine(newLine);
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
        string filename = Path.GetFileName(header);
        string? target = _mapper.Map(filename);
        if (target == null)
        {
            newInclude = include;
            return false;
        }

        newInclude = include[..begin] + '<' + target + '>' + include[(end + 1)..];

        return true;
    }
}