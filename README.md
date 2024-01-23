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

### Shader Notes (Placeholder)
https://docs.godotengine.org/en/stable/tutorials/shaders/introduction_to_shaders.html
https://docs.godotengine.org/en/stable/tutorials/shaders/shader_reference/shading_language.html#doc-shading-language
https://docs.godotengine.org/en/stable/tutorials/shaders/shader_reference/canvas_item_shader.html
https://www.khronos.org/opengl/wiki/Built-in_Variable_(GLSL)#Fragment_shader_inputs
https://thebookofshaders.com/

Variables:
vec4 COLOR - rgba of fragment (can be set)
vec4 FRAGCOORD - coordinates of fragment
UV - normilzed coordinates of fragment
vec2 TEXTURE_PIXEL_SIZE - inverse of texture resolution
vec2 SCREEN_PIXEL_SIZE - inverse of screen resolution
float TIME - seconds since game startup

zTextureSpec - returns vec4 color of provided sampler2D tex at given UV coords
zTexture - return vec4 color, shorthand for zTextureSpec but tex is TEXTURE
TEXTURE -> a uniform, is a sampler2D. Set by Clyde to be a texture.