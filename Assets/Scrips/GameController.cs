using System.Collections;
using TerrainGenerator;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int Radius = 1;

    private Vector2i PreviousPlayerChunkPosition;

    public Transform Player;

    public TerrainChunkGenerator Generator;

    void Start()
    {
        Cursor.visible = false;
    }
    
}