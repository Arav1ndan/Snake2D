using System.Collections.Generic;
using UnityEngine;

public class SnakeGame : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(20, 20); // Grid size: 20x20
    public GameObject snakeSegmentPrefab; // Prefab for snake segments
    public GameObject foodPrefab; // Prefab for food

    private List<Vector2Int> snakeSegments = new List<Vector2Int>();
    private List<GameObject> snakeSegmentObjects = new List<GameObject>(); // Visual representation of the snake
    private Vector2Int snakeDirection = Vector2Int.right;
    private Vector2Int foodPosition;
    private GameObject foodObject; // Visual representation of food

    private float moveTimer = 0f;
    public float moveDelay = 0.2f; // Snake moves every 0.2 seconds

    private void Start()
    {
        // Initialize snake at center of grid
        Vector2Int startPosition = new Vector2Int(gridSize.x / 2, gridSize.y / 2);
        snakeSegments.Add(startPosition);

        // Create the visual snake segment
        GameObject initialSegment = Instantiate(snakeSegmentPrefab, GridToWorldPosition(startPosition), Quaternion.identity);
        snakeSegmentObjects.Add(initialSegment);

        // Spawn food
        SpawnFood();
    }

    private void Update()
    {
        HandleInput();

        // Move the snake at regular intervals
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveDelay)
        {
            moveTimer = 0f;
            MoveSnake();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && snakeDirection != Vector2Int.down) snakeDirection = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) && snakeDirection != Vector2Int.up) snakeDirection = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) && snakeDirection != Vector2Int.right) snakeDirection = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) && snakeDirection != Vector2Int.left) snakeDirection = Vector2Int.right;
    }

    private void MoveSnake()
    {
        // Calculate the new head position
        Vector2Int newHead = snakeSegments[0] + snakeDirection;

        // Screen wrapping logic
        if (newHead.x < 0) newHead.x = gridSize.x - 1;
        else if (newHead.x >= gridSize.x) newHead.x = 0;

        if (newHead.y < 0) newHead.y = gridSize.y - 1;
        else if (newHead.y >= gridSize.y) newHead.y = 0;

        // Check if the snake eats food
        if (newHead == foodPosition)
        {
            snakeSegments.Insert(0, newHead); // Add a new segment
            GameObject newSegment = Instantiate(snakeSegmentPrefab, GridToWorldPosition(newHead), Quaternion.identity);
            snakeSegmentObjects.Insert(0, newSegment);

            SpawnFood();
        }
        else
        {
            // Move the snake: Add new head and remove tail
            snakeSegments.Insert(0, newHead);
            snakeSegments.RemoveAt(snakeSegments.Count - 1);

            // Update visual representation
            GameObject newSegment = Instantiate(snakeSegmentPrefab, GridToWorldPosition(newHead), Quaternion.identity);
            snakeSegmentObjects.Insert(0, newSegment);

            Destroy(snakeSegmentObjects[snakeSegmentObjects.Count - 1]);
            snakeSegmentObjects.RemoveAt(snakeSegmentObjects.Count - 1);
        }
    }

    private void SpawnFood()
    {
        // Spawn food at a random position in the grid
        do
        {
            foodPosition = new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        } while (snakeSegments.Contains(foodPosition)); // Ensure food doesn't spawn on the snake

        // Create or move the visual food
        if (foodObject == null)
        {
            foodObject = Instantiate(foodPrefab, GridToWorldPosition(foodPosition), Quaternion.identity);
        }
        else
        {
            foodObject.transform.position = GridToWorldPosition(foodPosition);
        }
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, gridPosition.y, 0);
    }
}
