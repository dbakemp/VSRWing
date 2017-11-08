using System.Threading;
using UnityEngine;

namespace TerrainGenerator
{
    public class TerrainChunk
    {
        public Vector2i Position { get; private set; }

        public Terrain Terrain { get; set; }

        private TerrainData Data { get; set; }

        private TerrainChunkSettings Settings { get; set; }

        private NoiseProvider NoiseProvider { get; set; }

        private float[,] Heightmap { get; set; }

        private object HeightmapThreadLockObject { get; set; }

        public TerrainChunk(TerrainChunkSettings settings, NoiseProvider noiseProvider, int x, int z)
        {
            HeightmapThreadLockObject = new object();

            Settings = settings;
            NoiseProvider = noiseProvider;

            Position = new Vector2i(x, z);
        }

        #region Heightmap stuff

        public void GenerateHeightmap()
        {
            var thread = new Thread(GenerateHeightmapThread);
            thread.Start();
        }

        private void GenerateHeightmapThread()
        {
            lock (HeightmapThreadLockObject)
            {
                var heightmap = new float[Settings.HeightmapResolution, Settings.HeightmapResolution];

                for (var zRes = 0; zRes < Settings.HeightmapResolution; zRes++)
                {
                    for (var xRes = 0; xRes < Settings.HeightmapResolution; xRes++)
                    {
                        var xCoordinate = Position.X + (float)xRes / (Settings.HeightmapResolution - 1);
                        var zCoordinate = Position.Z + (float)zRes / (Settings.HeightmapResolution - 1);

                        heightmap[zRes, xRes] = NoiseProvider.GetValue(xCoordinate, zCoordinate);
                    }
                }

                Heightmap = heightmap;
            }
        }

        public bool IsHeightmapReady()
        {
            return Terrain == null && Heightmap != null;
        }

        public float GetTerrainHeight(Vector3 worldPosition)
        {
            return Terrain.SampleHeight(worldPosition);
        }

        #endregion

        #region Main terrain generation

        public void CreateTerrain()
        {
            Data = new TerrainData();
            Data.heightmapResolution = Settings.HeightmapResolution;
            Data.alphamapResolution = Settings.AlphamapResolution;
            Data.SetHeights(0, 0, Heightmap);
            ApplyTextures(Data);

            Data.size = new Vector3(Settings.Length, Settings.Height, Settings.Length);
            var newTerrainGameObject = Terrain.CreateTerrainGameObject(Data);
            newTerrainGameObject.transform.position = new Vector3((Position.X * Settings.Length) - (Settings.Length/2), -30, Position.Z * Settings.Length);

            Terrain = newTerrainGameObject.GetComponent<Terrain>();
            Terrain.heightmapPixelError = 8;
            Terrain.materialType = UnityEngine.Terrain.MaterialType.Custom;
            Terrain.materialTemplate = Settings.TerrainMaterial;
            Terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            Terrain.heightmapPixelError = 1000;
            Terrain.Flush();
        }

        private void ApplyTextures(TerrainData terrainData)
        {
            var flatSplat = new SplatPrototype();
            var steepSplat = new SplatPrototype();

            flatSplat.texture = Settings.FlatTexture;
            steepSplat.texture = Settings.SteepTexture;

            terrainData.splatPrototypes = new SplatPrototype[]
            {
                flatSplat,
                steepSplat
            };

            terrainData.RefreshPrototypes();

            var splatMap = new float[terrainData.alphamapResolution, terrainData.alphamapResolution, 2];

            for (var zRes = 0; zRes < terrainData.alphamapHeight; zRes++)
            {
                for (var xRes = 0; xRes < terrainData.alphamapWidth; xRes++)
                {
                    var normalizedX = (float)xRes / (terrainData.alphamapWidth - 1);
                    var normalizedZ = (float)zRes / (terrainData.alphamapHeight - 1);

                    var steepness = terrainData.GetSteepness(normalizedX, normalizedZ);
                    var steepnessNormalized = Mathf.Clamp(steepness / 1.5f, 0, 1f);

                    splatMap[zRes, xRes, 0] = 1f - steepnessNormalized;
                    splatMap[zRes, xRes, 1] = steepnessNormalized;
                }
            }

            terrainData.SetAlphamaps(0, 0, splatMap);
        }

        #endregion

        #region Distinction

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as TerrainChunk;
            if (other == null)
                return false;

            return this.Position.Equals(other.Position);
        }

        #endregion
        

        public void Remove()
        {
            Heightmap = null;
            Settings = null;
            
            if (Terrain != null)
                GameObject.Destroy(Terrain.gameObject);
        }
    }
}