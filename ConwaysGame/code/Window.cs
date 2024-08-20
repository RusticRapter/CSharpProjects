using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using static Structs;

public class Window : GameWindow
{
    Vector4 backgroundColor = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
    Vector4 cellColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

    public GameWindowSettings gameWindowSettings;

    public NativeWindowSettings nativeWindowSettings;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, string vertShadPath, string fragShadPath) 
    : base(gameWindowSettings, nativeWindowSettings) {
        this.gameWindowSettings = gameWindowSettings;
        this.nativeWindowSettings = nativeWindowSettings;

        shader = new Shader(vertShadPath, fragShadPath);
        camera = new Camera();
        shadData = new shaderData[0];
    }

    //The data that will be drawn
    shaderData[] shadData;

    //The vertex buffer object
    int vbo;
    //The vertex array object
    int vao;

    Shader shader;

    Camera camera;

    float lastTime;
    float deltaTime;
    float debugTime;
    float frameTime;
    int frame;

    float xStretch = 1.6f;

    int sizeOfShadData = 6*sizeof(float);

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

        shadData = World.SetVerts(camera.xPos, camera.yPos, cellColor);
        for (int i = 0; i < shadData.Length; i++) {
            shadData[i].X = (shadData[i].X - camera.xPos) / xStretch;
            shadData[i].Y -= camera.yPos;
        }

        frameTime += deltaTime;
        if (Input.space && frameTime >= 0.5f) {
            World.NextFrame();
            frame++;
            frameTime = 0f;
            Console.WriteLine("Frame " + frame);
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
        GL.BufferData(BufferTarget.ArrayBuffer, shadData.Length * sizeOfShadData, shadData, BufferUsageHint.DynamicDraw);

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
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeOfShadData, 0);
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeOfShadData, 2*sizeof(float));
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
        GL.DrawArrays(PrimitiveType.Triangles, 0, shadData.Length);

        //Swaps the buffers to draw everything set this frame
        SwapBuffers();
    }
}