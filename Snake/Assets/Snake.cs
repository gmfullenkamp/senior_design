using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snake : MonoBehaviour
{
    // Current Movement Direction (by default it moves to the right)
    Vector2 dir = Vector2.right;

    // Keep track of tail
    List<Transform> tail = new List<Transform>();

    // Did the snake eat something?
    bool ate = false;

    // Tail prefab
    public GameObject tailPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Move the Snake every 300ms
        InvokeRepeating("Move", 0.3f, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // Move in a new Direction
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;  // -up means down
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right;  // -right means left
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
    }

    void Move()
    {
        // Save current position
        Vector2 v = transform.position;

        // Move head into new direction
        transform.Translate(dir);

        // Ate something?
        if (ate)
        {
            // Load prefab into the world
            GameObject g = (GameObject)Instantiate(tailPrefab,
                                                   v,
                                                   Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a tail?
        else if (tail.Count > 0)
        {
            // Move last tail element to where the head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.name.StartsWith("FoodPrefab"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the food
            Destroy(coll.gameObject);
        }
        // Collided with tail or border
        else
        {
            // TODO: you lose screen
        }
    }
}
