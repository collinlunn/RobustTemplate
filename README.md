### Robust Template Project
This document contains notes regarding the use of this template project.

### How to publish
Run ./Content.Packaging/Program.cs to create the project zip files in ./release. May need altered parameters to build all platforms.

### Common development errors
If it mysteriously breaks, run the following shell command from RobustTemplate:
    dotnet clean
    dotnet restore      
    dotnet build

Breakpoint not setting? "Source code is different" - make a small change and rebuild

### Engine Notes
Engine Documentation: https://docs.spacestation14.com/
(See "Robust Toolbox" subsection in left-side dropdown)

Engine Changelog: ./RobustToolbox/RELEASE-NOTES.md

### Shader Notes (Placeholder)
Spacw Wizard Shading Language (SWSL) Source Document: https://docs.spacestation14.com/en/robust-toolbox/rendering/shaders.html

Variable References:
vec4 COLOR - rgba of fragment (can be set)
vec4 FRAGCOORD - coordinates of fragment
UV - normilzed coordinates of fragment
vec2 TEXTURE_PIXEL_SIZE - inverse of texture resolution
vec2 SCREEN_PIXEL_SIZE - inverse of screen resolution
float TIME - seconds since game startup

zTextureSpec - returns vec4 color of provided sampler2D tex at given UV coords
zTexture - return vec4 color, shorthand for zTextureSpec but tex is TEXTURE
TEXTURE -> a uniform, is a sampler2D. Set by Clyde to be a texture.

### Environmental Variables (RuntimeConfig)
Can improve performance:
DOTNET_TieredPGO: 1
DOTNET_TC_QuickJitForLoops: 1
DOTNET_ReadyToRun: 0

Enable AVX operations (Depending on your processor, may reduce performance):
ROBUST_NUMERICS_AVX: true

Can set environment variables from Watchdog.

### Running Prod Servers
Server Hosting: https://docs.spacestation14.com/en/general-development/setup/server-hosting-tutorial.html
Watchdog Setup: https://docs.spacestation14.com/en/server-hosting/setting-up-ss14-watchdog.html

In the seperate watchdog project:
Publish by running Packager.csproj, files get put in ./release
Put server files (exe, dll, etc) into .\SS14.Watchdog\bin\instances\{insert instance name from app settings}\bin
Custom server/client update methodas can be madew by extending UpdateProvider