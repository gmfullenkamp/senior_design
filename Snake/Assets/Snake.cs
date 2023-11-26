using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class Snake : Agent
{
    // Current Movement Direction (by default it moves to the right)
    Vector2 dir = Vector2.right;

    // Keep track of tail
    List<Transform> tail = new List<Transform>();

    // Did the snake eat something?
    bool ate = false;

    // Tail prefab
    public GameObject tailPrefab;

    // Food prefab
    public Transform foodPrefab;

    // Walls
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Movement interval in seconds
    public float moveInterval = 0.01f;
    private float timeSinceLastMove;

    // Begin a training session
    public override void OnEpisodeBegin()
    {
        // Reset snake position, tail, and food
        transform.localPosition = Vector2.zero;
        ResetTail();
        ate = false;
        ClearFood();
        SpawnFood();
    }

    // Reset tail
    void ResetTail()
    {
        foreach (Transform segment in tail)
        {
            Destroy(segment.gameObject);
        }
        tail.Clear();
    }

    void SpawnFood()
    {
        // Ensure any existing food is cleared first
        ClearFood();

        // x position between left and right border
        int x = (int)Random.Range(borderLeft.localPosition.x, borderRight.localPosition.x);

        // y position between top and bottom border
        int y = (int)Random.Range(borderBottom.localPosition.y, borderTop.localPosition.y);

        // Instantiate the food at (x, y)
        Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
    }

    void ClearFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foods)
        {
            Destroy(food);
        }
    }

    // ML observations
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(foodPrefab.localPosition);
        sensor.AddObservation(borderTop.localPosition);
        sensor.AddObservation(borderBottom.localPosition);
        sensor.AddObservation(borderLeft.localPosition);
        sensor.AddObservation(borderRight.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Map actions to movement directions
        int moveDirection = actions.DiscreteActions[0];
        switch (moveDirection)
        {
            case 0: dir = Vector2.right; break;
            case 1: dir = Vector2.up; break;
            case 2: dir = -Vector2.right; break;
            case 3: dir = -Vector2.up; break;
        }

        // Update the timer
        timeSinceLastMove += Time.deltaTime;

        // Move if the interval has passed
        if (timeSinceLastMove >= moveInterval)
        {
            Move();
            timeSinceLastMove = 0;
        }
    }

    // Player control
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            discreteActions[0] = 0;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActions[0] = 3;
        }
    }

    void Move()
    {
        // Save current position
        Vector2 v = transform.localPosition;

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
        Debug.Log(coll.name);
        // Food?
        if (coll.name.StartsWith("FoodPrefab"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the food
            Destroy(coll.gameObject);

            // Reward!
            SetReward(+100f);

            // Spawn new food after eating
            SpawnFood();
        }
        // Collided with tail or border
        else if (coll.CompareTag("Wall") || coll.CompareTag("Tail"))
        {
            // Wall hit :(
            SetReward(-10000f);
            EndEpisode();
        }
        // Didn't find anything
        else
        {
            SetReward(-1f);
        }
    }
}