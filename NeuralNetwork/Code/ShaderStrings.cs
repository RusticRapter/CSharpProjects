using System;

namespace NeuralNetwork.Code;

public static class ShaderStrings {
    public static string vertShader = @"
        #version 330
        layout (location = 0) in vec2 aPosition;
        layout (location = 1) in vec3 aColor;

        out vec4 fragColor;

        void main()
        {
            gl_Position = vec4(aPosition, 0.0, 1.0);

            fragColor = vec4(aColor, 1.0);
        }
    ";

    public static string fragShader = @"
        #version 330

        in vec4 fragColor;

        out vec4 outColor;

        void main()
        {
            outColor = vec4(fragColor);
        }
    ";
}
