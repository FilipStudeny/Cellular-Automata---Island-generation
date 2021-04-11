using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsLand;

    // What you have you have ;)
    public Vector2Int GridPosition;
    
    public Tile neighbor_UP;
    public Tile neighbor_RIGHT;
    public Tile neighbor_LEFT;
    public Tile neighbor_DOWN;

    public GameObject ground_Tile;

    private void Start() 
    {
        if (IsLand)
        {
            GameObject tile = Instantiate(ground_Tile, Vector3.zero, Quaternion.identity);
            tile.transform.SetParent(transform, false);
        }
        else { Destroy(gameObject); }
    }

}
