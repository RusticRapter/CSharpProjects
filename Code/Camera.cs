using System;

//Stores basic information about a camera
public class Camera {
    //Speed of the camera
    public float moveSpeed = 1.0f;
    public float zoomSpeed = 1.0f;

    public float xPos;
    public float yPos;

    public Camera() {
        xPos = 0.0f;
        yPos = 0.0f;
    }
}
