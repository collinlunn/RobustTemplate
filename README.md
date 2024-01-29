### Robust Template Project
This documenbt contains notes regarding the use of this template project.

### How to publish
Run RobustTemplate/Content.Packaging/Program.cs to create the project zip files in RobustTemplate/release

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