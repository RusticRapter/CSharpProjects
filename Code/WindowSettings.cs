using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;

public static class WindowSettings {
    //Settings for Game Window Settings (gWinSets)
    //
    //Sets the max refresh rate for the window
    //0.0 uncaps the refresh rate
    public static double refreshRate = 0.0;
    //Sets if the window stops drawing while being dragged around
    public static bool freezeOnDrag = false;

    //Settings for Native Window Settings (nWinSets)
    //
    //Sets the window's icon
    public static WindowIcon? winIcon;
    //Sets the window's title
    public static string winTitle = "New Window";
    //Sets whether the window starts in focus
    public static bool startFocused = true;
    //Sets whether the window starts visible
    public static bool StartVisible = true;
    //Sets what state the window is in
    //Acceptable values are: Fullscreen, Maximized, Minimized, Normal
    public static WindowState winState = WindowState.Normal;
    //Sets the window's border state
    //Acceptable values are: Fixed, Hidden, Resizable
    public static WindowBorder winBord = WindowBorder.Resizable;
    //Sets the default window size
    public static Vector2i winSize = new Vector2i(960, 540);
    //Sets the minimum size for the window
    //If set to null, there is no minimum
    public static Vector2i? minWinSize = new Vector2i (320, 180);
    //Sets the maximum size for the window
    //If set to null, there is no maximum
    public static Vector2i? maxWinSize = null;
    //Sets an aspect ratio for the window to be locked to
    //If set to null, the window isn't locked to an aspect ration
    public static (int numerator, int denominator)? aspectRatio = (16, 9);
    //Sets in the window should have VSync enabled
    //Acceptable values are: On, Off, Adaptive
    public static VSyncMode vSync = VSyncMode.Off;
}
