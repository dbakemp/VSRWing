using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    public float scrollSpeed = 0.5F;
    public Renderer rend;
    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<MeshRenderer>();
    }
	
    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
