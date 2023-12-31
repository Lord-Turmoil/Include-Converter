﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using IncludeConverter.Converter;

namespace IncludeConverter;

internal static class Program
{
    private static bool _isSilent = false;

    public static void Main(string[] args)
    {
        bool isInclude = false;
        bool isSource = false;
        bool relativeToAbsolute = true;
        string rootDirectory = Directory.GetCurrentDirectory();

        List<string> includeDirectories = new();
        List<string> sourceDirectories = new();
        foreach (string arg in args)
        {
            if (arg == "-i")
            {
                isInclude = true;
                isSource = false;
            }
            else if (arg == "-s")
            {
                isInclude = false;
                isSource = true;
            }
            else if (arg == "--to-quote")
            {
                relativeToAbsolute = false;
            }
            else if (arg == "--silent")
            {
                _isSilent = true;
            }
            else
            {
                if (isInclude)
                {
                    if (!(arg.EndsWith('\\') || arg.EndsWith('/')))
                    {
                        includeDirectories.Add(arg + '\\');
                    }
                    else
                    {
                        includeDirectories.Add(arg);
                    }
                }

                if (isSource)
                {
                    sourceDirectories.Add(arg);
                }

                if (!isInclude && !isSource)
                {
                    rootDirectory = arg;
                }
            }
        }

        Directory.SetCurrentDirectory(rootDirectory);

        if (relativeToAbsolute)
        {
            RelativeToAbsolute(includeDirectories, sourceDirectories);
        }
        else
        {
            AbsoluteToRelative(includeDirectories, sourceDirectories);
        }
    }

    private static void RelativeToAbsolute(IEnumerable<string> incList, IEnumerable<string> srcList)
    {
        var mapper = new IncludeMapper(_isSilent);
        mapper.BuildIndex(incList);
        var converter = new RelativeToAbsoluteConverter(mapper, _isSilent);
        converter.Convert(srcList);
    }

    private static void AbsoluteToRelative(IEnumerable<string> incList, IEnumerable<string> srcList)
    {
        var mapper = new IncludeMapper(_isSilent);
        mapper.BuildIndex(incList);
        var converter = new AbsoluteToRelativeConverter(mapper, _isSilent);
        converter.Convert(srcList);
    }
}