﻿using System.Collections.Generic;
using System.Linq;

namespace TerrainGenerator
{
    public class ChunkCache
    {
        private readonly int MaxChunkThreads = 3;

        private Dictionary<Vector2i, TerrainChunk> RequestedChunks { get; set; }

        private Dictionary<Vector2i, TerrainChunk> ChunksBeingGenerated { get; set; }

        private Dictionary<Vector2i, TerrainChunk> LoadedChunks { get; set; }

        private HashSet<Vector2i> ChunksToRemove { get; set; }

        public OnChunkGeneratedDelegate OnChunkGenerated { get; set; }

        public ChunkCache()
        {
            RequestedChunks = new Dictionary<Vector2i, TerrainChunk>();
            ChunksBeingGenerated = new Dictionary<Vector2i, TerrainChunk>();
            LoadedChunks = new Dictionary<Vector2i, TerrainChunk>();
            ChunksToRemove = new HashSet<Vector2i>();
        }

        public void Update()
        {
            TryToDeleteQueuedChunks();

            GenerateHeightmapForAvailableChunks();
            CreateTerrainForReadyChunks();
        }

        public void AddNewChunk(TerrainChunk chunk)
        {
            RequestedChunks.Add(chunk.Position, chunk);
            GenerateHeightmapForAvailableChunks();
        }

        public void RemoveChunk(int x, int z)
        {
            ChunksToRemove.Add(new Vector2i(x, z));
            TryToDeleteQueuedChunks();
        }

        public bool ChunkCanBeAdded(int x, int z)
        {
            var key = new Vector2i(x, z);
            return
                !(RequestedChunks.ContainsKey(key)
                || ChunksBeingGenerated.ContainsKey(key)
                || LoadedChunks.ContainsKey(key));
        }

        public bool ChunkCanBeRemoved(int x, int z)
        {
            var key = new Vector2i(x, z);
            return
                RequestedChunks.ContainsKey(key)
                || ChunksBeingGenerated.ContainsKey(key)
                || LoadedChunks.ContainsKey(key);
        }

        public bool IsChunkGenerated(Vector2i chunkPosition)
        {
            return GetGeneratedChunk(chunkPosition) != null;
        }

        public TerrainChunk GetGeneratedChunk(Vector2i chunkPosition)
        {
            if (LoadedChunks.ContainsKey(chunkPosition))
                return LoadedChunks[chunkPosition];

            return null;
        }

        public List<Vector2i> GetGeneratedChunks()
        {
            return LoadedChunks.Keys.ToList();
        }

        private void GenerateHeightmapForAvailableChunks()
        {
            var requestedChunks = RequestedChunks.ToList();
            if (requestedChunks.Count > 0 && ChunksBeingGenerated.Count < MaxChunkThreads)
            {
                var chunksToAdd = requestedChunks.Take(MaxChunkThreads - ChunksBeingGenerated.Count);
                foreach (var chunkEntry in chunksToAdd)
                {
                    ChunksBeingGenerated.Add(chunkEntry.Key, chunkEntry.Value);
                    RequestedChunks.Remove(chunkEntry.Key);

                    chunkEntry.Value.GenerateHeightmap();
                }
            }
        }

        private void CreateTerrainForReadyChunks()
        {
            var anyTerrainCreated = false;

            var chunks = ChunksBeingGenerated.ToList();
            foreach (var chunk in chunks)
            {
                if (chunk.Value.IsHeightmapReady())
                {
                    ChunksBeingGenerated.Remove(chunk.Key);
                    LoadedChunks.Add(chunk.Key, chunk.Value);

                    chunk.Value.CreateTerrain();

                    anyTerrainCreated = true;
                    if (OnChunkGenerated != null)
                        OnChunkGenerated.Invoke(ChunksBeingGenerated.Count);
                    
                }
            }
            
        }

        private void TryToDeleteQueuedChunks()
        {
            var chunksToRemove = ChunksToRemove.ToList();
            foreach (var chunkPosition in chunksToRemove)
            {
                if (RequestedChunks.ContainsKey(chunkPosition))
                {
                    RequestedChunks.Remove(chunkPosition);
                    ChunksToRemove.Remove(chunkPosition);
                }
                else if (LoadedChunks.ContainsKey(chunkPosition))
                {
                    var chunk = LoadedChunks[chunkPosition];
                    chunk.Remove();

                    LoadedChunks.Remove(chunkPosition);
                    ChunksToRemove.Remove(chunkPosition);
                }
                else if (!ChunksBeingGenerated.ContainsKey(chunkPosition))
                    ChunksToRemove.Remove(chunkPosition);
            }
        }
        
    }
}