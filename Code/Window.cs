using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NeuralNetwork.Code;

public class Window : GameWindow {
    Vector4 backgroundColor = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
    Vector4 cellColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    public GameWindowSettings gameWindowSettings;

    public NativeWindowSettings nativeWindowSettings;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, string vertShad, string fragShad) 
    : base(gameWindowSettings, nativeWindowSettings) {
        this.gameWindowSettings = gameWindowSettings;
        this.nativeWindowSettings = nativeWindowSettings;

        shader = new Shader(vertShad, fragShad);
        camera = new Camera();
    }

    //The data that will be drawn
    float[] shaderData = {
        0.0f, 0.5f, 1.0f, 0.0f, 0.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 1.0f 
    };
    //The size of each piece of shader data
    int sizeofShaderData = 5*sizeof(float);
    //The number of triangles in the shader data
    int numofVertexes = 3;

    //The vertex buffer object
    int vbo;
    //The vertex array object
    int vao;

    Shader shader;

    Camera camera;

    float lastTime;
    float deltaTime;
    float debugTime;

    float xStretch = 1.6f;

    //Called once wwhen Window.Run is fi rst called
    protected override void OnLoad() {
        base.OnLoad();

        //Sets the background color of the window
        GL.ClearColor(backgroundColor.X, backgroundColor.Y, backgroundColor.Z, backgroundColor.W);

        //Sets up the vbo and vao
        vbo = GL.GenBuffer();
        vao = GL.GenVertexArray();
        lastTime = DateTime.Now.Second + (DateTime.Now.Millisecond / 1000f);
    }

    //Called every frame before OnRenderFrame
    protected override void OnUpdateFrame(FrameEventArgs args) {
        base.OnUpdateFrame(args);

        //Update delta time
        float curTime = DateTime.Now.Second + (DateTime.Now.Millisecond / 1000f);
        deltaTime = curTime - lastTime;
        lastTime = curTime;

        Input.Update(this);

        //Exits the window if the escape key is pressed
        if (Input.escape) {
            Close();
        }
        if (Input.w) {
            camera.yPos += camera.moveSpeed * deltaTime;
        }
        if (Input.s) {
            camera.yPos -= camera.moveSpeed * deltaTime;
        }
        if (Input.d) {
            camera.xPos += camera.moveSpeed * deltaTime;
        }
        if (Input.a) {
            camera.xPos -= camera.moveSpeed * deltaTime;
        }

        //Debugs
        //
        /*debugTime += deltaTime;
        if (debugTime >= 1) {
            Console.WriteLine("Triangles Drawn: " + shadData.Length/3);
            Console.WriteLine("Cam Position: " + camera.xPos + ", " + camera.yPos);
            debugTime = 0;
        }*/

        //Sets up the vbo with this frame's vertices
        //
        //Tells computer which buffer to apply changes to
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        //Fills the vbo with data
        //BufferUsageHint can accept:
        //StaticDraw- data will not change much
        //DynamicDraw- data will change a lot
        //StreamDraw- data will change everytime it is drawn
        GL.BufferData(BufferTarget.ArrayBuffer, numofVertexes * sizeofShaderData, shaderData, BufferUsageHint.StreamDraw);

        //Sets up the vao
        //
        //Tells computer which array to use
        GL.BindVertexArray(vao);
        //Sets up the array
        //index: specifies the location of the vertex attribute to be used by the shader
        //size: the number of items in each stride
        //type: the type of data included in the array
        //normalized: sets whether fixed point values should be normalized or not
        //stride: the length between the start of each data set
        //offset: specifies the where first component in the array is
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeofShaderData, 0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeofShaderData, 2*sizeof(float));
        //Enables the vao
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);

        //Unbinds the vbo and vao from the array buffer
        //Setting it to 0 essentially sets it to null
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
    }

    //Called every frame after OnUpdateFrame
    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);

        //Clears the screan
        GL.Clear(ClearBufferMask.ColorBufferBit);

        shader.Use();
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, numofVertexes);

        //Swaps the buffers to draw everything set this frame
        SwapBuffers();
    }
}
