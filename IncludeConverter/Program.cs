// See https://aka.ms/new-console-template for more information

using IncludeConverter.Converter;
using IncludeConverter.Mapper;

namespace IncludeConverter;

class Program
{
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
            else if (arg == "-a")
            {
                relativeToAbsolute = false;
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

    private static void RelativeToAbsolute(List<string> incList, List<string> srcList)
    {
        var mapper = new RelativeToAbsoluteMapper();
        mapper.BuildIndex(incList);
        var converter = new RelativeToAbsoluteConverter(mapper);
        converter.Convert(srcList);
    }

    private static void AbsoluteToRelative(List<string> incList, List<string> srcList)
    {
        var mapper = new AbsoluteToRelativeMapper();
        mapper.BuildIndex(incList);
        var converter = new AbsoluteToRelativeConverter(mapper);
        converter.Convert(srcList);
    }
}
