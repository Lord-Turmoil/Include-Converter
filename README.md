# Include Converter

Copyright &copy; Tony's Studio 2023

---

## Overview

This tool can convert include directives in C/C++ between "" and <>. If you use CMake or MSVC project that can detect header files automatically, and yet also need raw include directives where these tools are not available, it is exactly what you are looking for.

It is implemented with C#, so you may have .NET framework installed.

## Usage

```bash
IncludeConverter.exe [rootDir] [-a] -i [incDir1 incDir2 ...] -s [srcDir1 srcDir2 ...]
```

- `rootDir`: The root directory of your project, or sub-project, to avoid too long path in the later directories. If you don't add this, it will take the current directory as the root, thus you may need to use absolute path later.
- `-a`: If this is added, it will convert absolute path to relative path, i.e. <> to "", otherwise, it will convert "" to <>.
- `-i`: Parameters following `-i` will be regarded as include directories.
- `-s`: Parameters following `-s` will be regarded as source file directories. Of course, it can include header files, since we also want to change include directives in them.

If you specified `rootDir`, then you can simply use relative path for include directories and source directories.

## Demo

You can find a demo CMake project in Release page, with `IncludeConverter.exe` included. You can simply open it with Visual Studio or other IDE you like.

Under the project directory, run `rel2abs.bat` to convert relative include directives to absolute include directives. ("" to <>). Run `abs2rel.bat` to convert <> to "".

---

## Limitations

It currently use one-to-one mapping, so it won't work as expected if you have header files with the same name in one project. Although it can be avoided by separate these header files in different runs.