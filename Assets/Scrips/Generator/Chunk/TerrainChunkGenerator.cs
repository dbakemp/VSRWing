using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TerrainGenerator
{
    public class TerrainChunkGenerator : MonoBehaviour
    {
        public Material TerrainMaterial;

        public Texture2D FlatTexture;
        public Texture2D SteepTexture;

        private TerrainChunkSettings Settings;

        private NoiseProvider NoiseProvider;

        List<TerrainChunk> chunks = new List<TerrainChunk>();
        
        TerrainChunk newestChunk;

        int xScrollPosition = 0;
        int newScrollCounter = 0;

        private void Awake()
        {
            Settings = new TerrainChunkSettings(129, 129, 100, 30, FlatTexture, SteepTexture, TerrainMaterial);
            NoiseProvider = new NoiseProvider();

            addChunk(0, 0);
            addChunk(0, 1);
            addChunk(0, 2);
            addChunk(0, 3);
            addChunk(0, 4);
            addChunk(0, 5);
            addChunk(0, 6);
        }

        public void addChunk(int x, int y)
        {
            TerrainChunk chunk = new TerrainChunk(Settings, NoiseProvider, x, y);
            chunk.GenerateHeightmap();
            chunks.Add(chunk);
            newestChunk = chunk;
        }

        private void Update()
        {
            foreach (TerrainChunk chunk in chunks)
            {
                if (chunk.IsHeightmapReady())
                {
                    chunk.CreateTerrain();
                    chunk.Terrain.transform.position += new Vector3(0, 0, xScrollPosition);
                }
            }

            foreach (TerrainChunk chunk in chunks)
            {
                if (chunk.Terrain != null)
                {
                    chunk.Terrain.transform.position += new Vector3(0, 0, -4);
                }
            }

            xScrollPosition += -4;
            newScrollCounter += 4;

            if (newScrollCounter >= Settings.Length)
            {
                newScrollCounter = 0;
                addChunk(newestChunk.Position.X, newestChunk.Position.Z+1);
            }

            for(int i = chunks.Count-1; i >= 0; i--)
            {
                if (chunks[i].Terrain != null && chunks[i].Terrain.transform.position.z < -Settings.Length)
                {
                    chunks[i].Remove();
                    chunks.RemoveAt(i);
                }
            }
        }
    }
}