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
    private Vector2 dir;

    // Keep track of tail
    List<Transform> tail = new List<Transform>();

    // Did the snake eat something?
    bool ate = false;

    // Distance from snake head to food
    private float lastDistanceToFood;

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
        bool isPositionOccupied;
        int x, y;
        do
        {
            isPositionOccupied = false;

            // x position between left and right border
            x = (int)Random.Range(borderLeft.localPosition.x + 1, borderRight.localPosition.x - 1);

            // y position between top and bottom border
            y = (int)Random.Range(borderBottom.localPosition.y + 1, borderTop.localPosition.y - 1);

            Vector2 potentialPosition = new Vector2(x, y);

            // Convert the snake head's position to Vector2 for comparison
            Vector2 snakeHeadPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);

            // Check if the potential position is occupied by the snake's head or any part of its tail
            if (snakeHeadPosition == potentialPosition || tail.Any(segment => new Vector2(segment.localPosition.x, segment.localPosition.y) == potentialPosition))
            {
                isPositionOccupied = true;
            }
        } while (isPositionOccupied); // Repeat until an unoccupied position is found

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
        // sensor.AddObservation(borderTop.localPosition);
        // sensor.AddObservation(borderBottom.localPosition);
        // sensor.AddObservation(borderLeft.localPosition);
        // sensor.AddObservation(borderRight.localPosition);
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
            case 4: dir = dir; break;
        }

        // Update the timer
        timeSinceLastMove += Time.deltaTime;

        // Move if the interval has passed
        if (timeSinceLastMove >= moveInterval)
        {
            Move();
            RewardBasedOnDistanceToFood();
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
        else
        {
            discreteActions[0] = 4;
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

    void RewardBasedOnDistanceToFood()
    {
        float currentDistanceToFood = Vector2.Distance(transform.localPosition, foodPrefab.localPosition);

        // Reward for moving closer, penalize for moving away
        if (currentDistanceToFood < lastDistanceToFood)
        {
            SetReward(0.04f); // Small reward for moving closer
        }
        else
        {
            SetReward(-0.05f); // Small penalty for moving away
        }

        // Update the last distance for the next frame
        lastDistanceToFood = currentDistanceToFood;
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
            SetReward(+1f);

            // Spawn new food after eating
            SpawnFood();
        }
        // Collided with tail or border
        else if (coll.CompareTag("Wall") || coll.CompareTag("Tail"))
        {
            // Wall hit :(
            SetReward(-10f);
            EndEpisode();
        }
    }
}