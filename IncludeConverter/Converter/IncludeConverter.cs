using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncludeConverter.Extensions;

namespace IncludeConverter.Converter;

internal abstract class IncludeConverter
{
    protected static readonly List<string> Extensions = new() { ".h", ".hpp", ".hxx", ".hh", ".c", ".cpp", ".cc", ".cxx" };

    public void Convert(IEnumerable<string> sourceDirectories)
    {
        foreach (string sourceDirectory in sourceDirectories)
        {
            Convert(sourceDirectory);
        }
    }

    public abstract void Convert(string sourceDirectory);

    protected static bool IsInclude(string line)
    {
        return line.StartsWith("#include");
    }
}