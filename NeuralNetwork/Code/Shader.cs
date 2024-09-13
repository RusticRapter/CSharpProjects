using OpenTK.Graphics.OpenGL;

namespace NeuralNetwork.Code;

public class Shader {
    int handle;

    public Shader(string vertShad, string fragShad) {
        //The vertex shader
        int vertShader;
        //The fragment shader
        int fragShader;
        //The source code for the shaders
        string vertShaderSource = vertShad;
        string fragShaderSource = fragShad;

        //Creates the vertex shader
        vertShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertShader, vertShaderSource);

        //Creates the fragment shader
        fragShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragShader, fragShaderSource);

        //Compiles the vertex shader
        GL.CompileShader(vertShader);

        //Checks for any problems with complilation
        //If the shader does not compile properly, it will output the log to the console
        GL.GetShader(vertShader, ShaderParameter.CompileStatus, out int vertSuccess);
        if (vertSuccess == 0) {
            string infoLog = GL.GetShaderInfoLog(vertShader);
            Console.WriteLine(infoLog);
        }

        //Compiles the fragment shader
        GL.CompileShader(fragShader);

        //Checks for any problems with complilation
        //If the shader does not compile properly, it will output the log to the console
        GL.GetShader(fragShader, ShaderParameter.CompileStatus, out int fragSuccess);
        if (fragSuccess == 0) {
            string infoLog = GL.GetShaderInfoLog(fragShader);
            Console.WriteLine(infoLog);
        }

        //Creates the program that contains both shader parts
        handle = GL.CreateProgram();

        //Attaches the shade parts to the program
        GL.AttachShader(handle, vertShader);
        GL.AttachShader(handle, fragShader);

        //Links the program together
        GL.LinkProgram(handle);

        //Checks for errors when creating the program
        //If the program does not link properly, it will output the log to the console
        GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int linkSuccess);
        if (linkSuccess == 0) {
            string infoLog = GL.GetProgramInfoLog(handle);
            Console.WriteLine(infoLog);
        }

        //Clean up of the shader parts
        GL.DetachShader(handle, vertShader);
        GL.DetachShader(handle, fragShader);
        GL.DeleteShader(vertShader);
        GL.DeleteShader(fragShader);
    }

    public void Use() {
        GL.UseProgram(handle);
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            GL.DeleteProgram(handle);

            disposedValue = true;
        }
    }

    ~Shader()
    {
        if (disposedValue == false)
        {
            Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
        }
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
