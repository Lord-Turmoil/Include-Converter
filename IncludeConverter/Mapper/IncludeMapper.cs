// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace IncludeConverter.Mapper;

internal abstract class IncludeMapper
{
    protected static readonly List<string> Extensions = new() { ".h", ".hpp", ".hxx", ".hh" };
    protected readonly Dictionary<string, string> Index = new();

    public void BuildIndex(IEnumerable<string> includeDirectories)
    {
        foreach (string includeDirectory in includeDirectories)
        {
            BuildIndex(includeDirectory);
        }
    }

    public abstract void BuildIndex(string includeDirectory);

    public string? Map(string include)
    {
        return !Index.ContainsKey(include) ? null : Index[include];
    }
}