using OpenTK.Windowing.GraphicsLibraryFramework;

public class Input {
    public static bool space = false;
    public static bool escape = false;
    public static bool w = false;
    public static bool a = false;
    public static bool s = false;
    public static bool d = false;

    public static void Update(Window window) {
        if (window.KeyboardState.IsKeyPressed(Keys.Space)) {space = true;}
        else if (window.KeyboardState.IsKeyReleased(Keys.Space)) {space = false;}
        if (window.KeyboardState.IsKeyPressed(Keys.Escape)) {escape = true;}
        else if (window.KeyboardState.IsKeyReleased(Keys.Escape)) {escape = false;}
        if (window.KeyboardState.IsKeyPressed(Keys.W)) {w = true;}
        else if (window.KeyboardState.IsKeyReleased(Keys.W)) {w = false;}
        if (window.KeyboardState.IsKeyPressed(Keys.A)) {a = true;}
        else if (window.KeyboardState.IsKeyReleased(Keys.A)) {a = false;}
        if (window.KeyboardState.IsKeyPressed(Keys.S)) {s = true;}
        else if (window.KeyboardState.IsKeyReleased(Keys.S)) {s = false;}
        if (window.KeyboardState.IsKeyPressed(Keys.D)) {d = true;}
        else if (window.KeyboardState.IsKeyReleased(Keys.D)) {d = false;}
    }
}
