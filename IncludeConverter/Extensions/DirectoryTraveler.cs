// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

namespace IncludeConverter.Extensions;

internal class DirectoryTraveler
{
    private readonly Action<string>? _visitDirectory;
    private readonly Action<string>? _visitFile;

    public DirectoryTraveler(Action<string>? visitDirectory, Action<string>? visitFile)
    {
        _visitDirectory = visitDirectory;
        _visitFile = visitFile;
    }

    public void Traverse(string path)
    {
        // Check path is file or directory.
        if (File.Exists(path))
        {
            TraverseFile(path);
        }

        if (Directory.Exists(path))
        {
            TraverseDirectory(path);
        }
    }

    private void TraverseDirectory(string path)
    {
        _visitDirectory?.Invoke(path);

        foreach (string file in Directory.EnumerateFiles(path))
        {
            TraverseFile(file);
        }

        foreach (string dir in Directory.EnumerateDirectories(path))
        {
            TraverseDirectory(dir);
        }
    }

    private void TraverseFile(string path)
    {
        _visitFile?.Invoke(path);
    }
}