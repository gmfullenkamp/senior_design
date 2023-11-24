using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    // Food Prefab
    public GameObject foodPrefab;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Use this for initialization
    void Start()
    {
        // Spawn food every 5 seconds
        InvokeRepeating("Spawn", 5, 5);
    }

    // Spawn one piece of food
    void Spawn()
    {
        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x,
                                  borderRight.position.x);

        // y position between top & bottom border
        int y = (int)Random.Range(borderBottom.position.y,
                                  borderTop.position.y);

        // Instantiate the food at (x, y)
        Instantiate(foodPrefab,
                    new Vector2(x, y),
                    Quaternion.identity); // default rotation
    }
}