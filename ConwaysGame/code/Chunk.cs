using System.Net;
using System.Numerics;
using OpenTK.Graphics.ES20;
using static Structs;

public class Chunk {
    public Vector2 position;
    public float[] vertData;

    public int fidelity;

    public bool[,] cellData;

    public Chunk(Vector2 pos, int fidel, string data) {
        position = pos;
        fidelity = fidel;

        vertData = SetVertData();
        cellData = SetCellData(data);
    }
    public Chunk(Vector2 pos, int fidel) {
        position = pos;
        fidelity = fidel;

        vertData = SetVertData();
        cellData = new bool[fidel,fidel];
    }

    float[] SetVertData() {
        float[] outData = new float[fidelity*fidelity*12];

        for(int i = 0; i < fidelity; i++) {
            for(int j = 0; j < fidelity; j++) {
                int arrPos = (i*fidelity + j)*12;
                outData[arrPos] = position.X + j*(1.0f/fidelity);
                outData[arrPos + 1] = position.Y - i*(1.0f/fidelity);
                outData[arrPos + 2] = position.X + (j+1)*(1.0f/fidelity);
                outData[arrPos + 3] = position.Y - i*(1.0f/fidelity);
                outData[arrPos + 4] = position.X + j*(1.0f/fidelity);
                outData[arrPos + 5] = position.Y - (i+1)*(1.0f/fidelity);

                outData[arrPos + 6] = position.X + (j+1)*(1.0f/fidelity);
                outData[arrPos + 7] = position.Y - i*(1.0f/fidelity);
                outData[arrPos + 8] = position.X + (j+1)*(1.0f/fidelity);
                outData[arrPos + 9] = position.Y - (i+1)*(1.0f/fidelity);
                outData[arrPos + 10] = position.X + j*(1.0f/fidelity);
                outData[arrPos + 11] = position.Y - (i+1)*(1.0f/fidelity);
            }
        }

        return outData;
    }
    
    bool[,] SetCellData(string Data) {
        bool [,] outData = new bool[fidelity,fidelity];
            for(int i = 0; i < fidelity; i++) {
                for(int j = 0; j < fidelity; j++) {
                    outData[j,i] = Data[i*16 + j] == '1' ? true : false;
                }
            }

        return outData;
    }

    public shaderData[] ToShaderData(Vector4 cellColor) {
        List<shaderData> output = new List<shaderData>();

        for(int i = 0; i < fidelity; i++) {
            for(int j = 0; j < fidelity; j++) {
                if(cellData[j,i]) {
                    for(int n = 0; n < 6; n++) {
                        output.Add(new shaderData(vertData[(i*16 + j)*12 + 2*n], vertData[(i*16 + j)*12 + 2*n + 1], cellColor));
                    }
                }
            }
        }

        return output.ToArray();
    }
    
    public List<Chunk> UpdateChunk(List<Chunk> chunks) {
        Chunk outChunk = new Chunk(position, fidelity);
        List<Chunk> output = new List<Chunk>();
        output.Add(outChunk);

        //Chunks surrounding this chunk
        Chunk upLeftChunk = null;
        Chunk leftChunk = null;
        Chunk downLeftChunk = null;

        Chunk upChunk = null;
        Chunk downChunk = null;

        Chunk upRightChunk = null;
        Chunk rightChunk = null;
        Chunk downRightChunk = null;

        foreach (Chunk c in chunks) {
            //Check if the surrounding chunks exist
            //Up Left
            if (c.position == position + new Vector2 (-1, -1)) {
                upLeftChunk = c;
            }
            //Left
            else if (c.position == position + new Vector2 (-1, 0)) {
                leftChunk = c;
            }
            //Down Left
            else if (c.position == position + new Vector2 (-1, 1)) {
                downLeftChunk = c;
            }
            //Up
            else if (c.position == position + new Vector2 (0, -1)) {
                upChunk = c;
            }
            //Down
            else if (c.position == position + new Vector2 (0, 1)) {
                downChunk = c;
            }
            //Up Right
            else if (c.position == position + new Vector2 (1, -1)) {
                upRightChunk = c;
            }
            //Right
            else if (c.position == position + new Vector2 (1, 0)) {
                rightChunk = c;
            }
            //Down Right
            else if (c.position == position + new Vector2 (1, 1)) {
                downRightChunk = c;
            }
            //Break if all are found
            if (upLeftChunk != null && leftChunk != null && downLeftChunk != null && upChunk != null && downChunk != null && upRightChunk != null && rightChunk != null && downRightChunk != null) {
                break;
            }
        }
        //If chunks don't exist, create them
        //Up Left
        if (upLeftChunk == null) {
            upLeftChunk = new Chunk(position + new Vector2(-1, -1), fidelity);
        }
        //Left
        if (leftChunk == null) {
            leftChunk = new Chunk(position + new Vector2(-1, 0), fidelity);
        }
        //Down Left
        if (downLeftChunk == null) {
            downLeftChunk = new Chunk(position + new Vector2(-1, 1), fidelity);
        }
        //Up
        if (upChunk == null) {
            upChunk = new Chunk(position + new Vector2(0, -1), fidelity);
        }
        //Down
        if (downChunk == null) {
            downChunk = new Chunk(position + new Vector2(0, 1), fidelity);
        }
        //Up Right
        if (upRightChunk == null) {
            upRightChunk = new Chunk(position + new Vector2(1, -1), fidelity);
        }
        //Right
        if (upLeftChunk == null) {
            rightChunk = new Chunk(position + new Vector2(1, 0), fidelity);
        }
        //Down Right
        if (upLeftChunk == null) {
            downRightChunk = new Chunk(position + new Vector2(1, 1), fidelity);
        }

        for (int i = 0; i < fidelity; i++) {
            for (int j = 0; j < fidelity; j++) {
                int neighbors = 0;

                //Check each neighbor for life
                //Up Left
                //If on far left
                if (j == 0) {
                    //If in top left corner
                    if (i == 0) {
                        if (upLeftChunk.cellData[fidelity-1, fidelity-1]) {neighbors++;}
                    }
                    //Any other far left
                    else {
                        if (leftChunk.cellData[fidelity-1, i-1]) {neighbors++;}
                    }
                }
                //If on top but not top left corner
                else if (i == 0) {
                    if (upChunk.cellData[j-1, fidelity-1]) {neighbors++;}
                }
                //Anything not on far left or top
                else {
                    if (cellData[j-1, i-1]) {neighbors++;}
                }
                //
                //Left
                //If on far left
                if (j == 0) {
                    if(leftChunk.cellData[fidelity-1, i]) {neighbors++;}
                }
                //Anything not on far left
                else {
                    if(cellData[j-1, i]) {neighbors++;}
                }
                //
                //Down Left
                //If on far left
                if (j == 0) {
                    //If in bottom left corner
                    if (i == (fidelity-1)) {
                        if (downLeftChunk.cellData[fidelity-1, 0]) {neighbors++;}
                    }
                    //Any other far left
                    else {
                        if (leftChunk.cellData[fidelity-1, i+1]) {neighbors++;}
                    }
                }
                //If on bottom but not bottom left corner
                else if (i == 0) {
                    if (downChunk.cellData[j-1, 0]) {neighbors++;}
                }
                //Anything not on far left or bottom
                else {
                    if (cellData[j-1, i+1]) {neighbors++;}
                }

                //Apply GoL rules
                if (cellData[j,i]) {
                    switch(neighbors) {
                        case <= 1:
                            outChunk.cellData[j,i] = false;
                            break;
                        case >= 4:
                            outChunk.cellData[j,i] = false;
                            break;
                        default:
                            outChunk.cellData[j,i] = true;
                            break;
                    }                                   
                }
                else {
                    switch(neighbors) {
                        case 3:
                            outChunk.cellData[j,i] = true;
                            break;
                        default:
                            outChunk.cellData[j,i] = false;
                            break;
                    }   
                }
            }
        }

        upLeftChunk.UpdateNewChunk();
        leftChunk.UpdateNewChunk();
        downLeftChunk.UpdateNewChunk();
        upChunk.UpdateNewChunk();
        downChunk.UpdateNewChunk();
        upRightChunk.UpdateNewChunk();
        rightChunk.UpdateNewChunk();
        downRightChunk.UpdateNewChunk();

        return output;
    }

    public Chunk UpdateNewChunk() {

    }
}
