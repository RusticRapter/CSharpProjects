

using OpenTK.Windowing.Desktop;

public static class RunTime {

    static Window window;
    static GameWindowSettings gWinSets;
    static NativeWindowSettings nWinSets;


    public static void Run() {
        gWinSets = GameWindowSettings.Default;
        //Sets anything changed from default;
        gWinSets.UpdateFrequency = WindowSettings.refreshRate;
        gWinSets.Win32SuspendTimerOnDrag = WindowSettings.freezeOnDrag;

        nWinSets = NativeWindowSettings.Default;
        //Sets any settings changed from their defaults
        //nWinSets.Icon = winIcon;
        //not currently set up^
        nWinSets.IsEventDriven = false;
        nWinSets.Title = WindowSettings.winTitle;
        nWinSets.StartFocused = WindowSettings.startFocused;
        nWinSets.StartVisible = WindowSettings.StartVisible;
        nWinSets.WindowState = WindowSettings.winState;
        nWinSets.WindowBorder = WindowSettings.winBord;
        nWinSets.ClientSize = WindowSettings.winSize;
        nWinSets.MinimumClientSize = WindowSettings.minWinSize;
        nWinSets.MaximumClientSize = WindowSettings.maxWinSize;
        nWinSets.AspectRatio = WindowSettings.aspectRatio;
        nWinSets.Vsync = WindowSettings.vSync;


        window = new Window(gWinSets, nWinSets, @"C:\Users\cmcmu\VS Code Programs\ConwaysGame\code\shader.vert", @"C:\Users\cmcmu\VS Code Programs\ConwaysGame\code\shader.frag");

        window.Run();
    }
}