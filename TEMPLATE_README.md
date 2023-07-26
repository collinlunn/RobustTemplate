### Robust Template Project

How to publish server exe:
From root dir: 
    python Tools/package_server_build.py --platform win-x64 --hybrid-acz

How to fix build if it mysteriously breaks:
From root dir: 
    dotnet clean
    dotnet restore      
    dotnet build
    dotnet run --project
    dotnet run --project Content.Client

Breakpoint not setting? (source code is diff) - make a small change and rebuild

TODO:
    make own launcher for client?
    how do shaders work?
    copy yaml linter and test setup?
    How do the content.tools scripts work?
    how to have non-standard fov?
    how to make custom cursor?
    What to overlays do?
    Make options menu
    Make actual design doc w/ feature list
