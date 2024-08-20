using System.Numerics;
using System.Runtime.Serialization;
using OpenTK.Graphics.ES20;
using static Structs;

public static class World {

    static int chunkFidelity = 16;

    public static List<Chunk> chunks = new List<Chunk> {
        new Chunk(new Vector2(0, 0), chunkFidelity, ChunkData.chunk00), 
        new Chunk(new Vector2(1, 0), chunkFidelity, ChunkData.chunk00),
        new Chunk(new Vector2(0, 1), chunkFidelity, ChunkData.chunk00), 
        new Chunk(new Vector2(1, 1), chunkFidelity, ChunkData.chunk00)
    };

    public static shaderData[] SetVerts(float camX, float camY, Vector4 cellColor) {
        List<Chunk> tempData = new List<Chunk>();
        foreach (Chunk c in chunks) {
            if (c.position.X - camX <= 1.6f && c.position.X - camX >= -2.6f && c.position.Y - camY <= 2f && c.position.Y - camY >= -1f) {
                tempData.Add(c);
            }
        }

        Chunk[] chunkData = tempData.ToArray();

        List<shaderData> output = new List<shaderData>();
        foreach(Chunk c in chunkData) {
            output.AddRange(c.ToShaderData(cellColor));
        }

        return output.ToArray();
    }

    public static void NextFrame() {
        List<Chunk> newChunks = new List<Chunk>();
        foreach(Chunk c in chunks) {
            newChunks.AddRange(c.UpdateChunk(chunks));
        }
        chunks = newChunks;

        //Remove unnecisary chunks
    }
}