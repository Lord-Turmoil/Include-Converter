// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace IncludeConverter.Converter;

internal abstract class IncludeConverter
{
    protected static readonly List<string> Extensions = new()
        { ".h", ".hpp", ".hxx", ".hh", ".c", ".cpp", ".cc", ".cxx" };

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