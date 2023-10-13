using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncludeConverter.Extensions;

namespace IncludeConverter.Mapper;

internal class AbsoluteToRelativeMapper : IncludeMapper
{
    public override void BuildIndex(string includeDirectory)
    {
        DirectoryTraveler traveler = new(null, VisitFile);
        traveler.Traverse(includeDirectory);
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
        if (Index.ContainsKey(filename))
        {
            Console.Write(" [Replace]");
        }

        // Will add if not exists, and update if exists.
        Index[filename] = path;

        Console.WriteLine($" as {path}");
    }
}
