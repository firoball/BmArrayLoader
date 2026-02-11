# BmArrayLoader
Loader for bitmap array files
It is aimed for loading indexed files with palettes only and targeted for a pretty specific use case.

Formats:

- LBM (PBM only)
- PCX (Version 5 only)

This is an early WIP.

With migration to .NET Standard 2.1 and .NET 10.0 for multi platform support, 
explicit Visual Studio support has been dropped.
Only a .csproj file is delivered - which in turn can also be openend by Visual Studio. 

## Build Commands

### Build Executable
.Net 10.0 (or future compatible) will be used for building.

#### Debug
`dotnet build [-c debug]`

Output: `bin/debug/`
Used for development

#### Release
`dotnet build -c release`

Output: `bin/release/`
Used for building release binary

#### Publish
`dotnet publish -c release`

Output: `bin/release/publish/` 

Used for publishing release binary with all dependencies

### Build DLL
.Net Standard2.1 will be used for building.

#### Debug
`dotnet build [-c debugdll]`

Output: `bin/debugdll/`

Used for development

#### Release
`dotnet build -c releasedll`

Output: `bin/releasedll/`

Used for building release binary

#### Publish
`dotnet publish -c releasedll`

Output: `bin/releasedll/publish/`

Used for publishing release binary with all dependencies

## GitHub

Source code can be obtained at
https://github.com/firoball/BmArrayLoader

## Legal stuff

Please respect [license.txt](license.txt) (Attribution-NonCommercial 4.0 International)

-firoball

[https://firoball.de](https://firoball.de)