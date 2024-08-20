using System.Drawing;
using System.Numerics;

public static class Structs {
    public struct shaderData {
        public shaderData(float x, float y, Vector4 color) {
            X = x;
            Y = y;
            TriColor = color;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public Vector4 TriColor { get; set; }
    };
}