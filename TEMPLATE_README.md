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
    how to make custom cursor?
    Make options menu
    Make actual design doc w/ feature list

Shader notes:
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