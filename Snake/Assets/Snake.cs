using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // Reference to the Snake's body prefab
    public GameObject snakeBodyPrefab;

    // Speed at which the snake moves
    public float moveSpeed = 5f;

    // Initial direction of the snake
    private Vector2 moveDirection = Vector2.right;

    // List to store the body segments of the snake
    private List<Transform> bodySegments = new List<Transform>();

    // Flag to check if the snake ate food
    private bool ateFood = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the snake with a few body segments
        InitializeSnake();
        
        // Call Move function to start moving the snake
        InvokeRepeating("Move", 0.1f, 1.0f / moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // Handle user input to change the snake's direction
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2.down)
        {
            moveDirection = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2.up)
        {
            moveDirection = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2.right)
        {
            moveDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2.left)
        {
            moveDirection = Vector2.right;
        }
    }

    // Initialize the snake with a few body segments
    void InitializeSnake()
    {
        // Add a few initial body segments
        for (int i = 0; i < 3; i++)
        {
            AddBodySegment();
        }
    }

    // Move the snake
    void Move()
    {
        // Save the current position of the head
        Vector2 previousPosition = transform.position;

        // Move the head in the specified direction
        transform.Translate(moveDirection);

        // Check if the snake ate food
        if (ateFood)
        {
            // Add a new body segment to the snake
            AddBodySegment();

            // Reset the ateFood flag
            ateFood = false;
        }

        // Move the body segments
        for (int i = 0; i < bodySegments.Count; i++)
        {
            Vector2 currentPos = bodySegments[i].position;
            bodySegments[i].position = previousPosition;
            previousPosition = currentPos;
        }
    }

    // Add a body segment to the snake
    void AddBodySegment()
    {
        // Instantiate a new body segment prefab
        GameObject bodySegment = Instantiate(snakeBodyPrefab, new Vector2(-10, -10), Quaternion.identity);

        // Add it to the bodySegments list
        bodySegments.Add(bodySegment.transform);
    }

    // Handle collisions
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the snake collided with food
        if (other.CompareTag("Food"))
        {
            // Destroy the food
            Destroy(other.gameObject);

            // Set the ateFood flag to true
            ateFood = true;
        }
        else
        {
            // The snake collided with something else (e.g., wall or itself)
            // Handle game over logic here
        }
    }
}
