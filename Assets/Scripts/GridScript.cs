using UnityEngine;

public class GridScript : MonoBehaviour
{
    // We can have a 15x15 grid

    private static GridScript instance;
    public static GridScript Instance { get { return instance; } }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private int gridWidth = 10;
    private int gridHeight = 10;
    private float cellSize = 1f;
    private Vector3 originPosition = Vector3.zero;

    public Vector2Int WorldToGridPosition(Vector2Int worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
        return new Vector2Int(x, y);
    }
    public Vector3 GridToWorldPostion(Vector2Int gridPosition)
    {
        float x = gridPosition.x * cellSize + originPosition.x + cellSize / 2f;
        float y = gridPosition.y * cellSize + originPosition.y + cellSize / 2f;
        return new Vector3(x, y, originPosition.z);
    }
    public bool IsWithinBounds(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < gridWidth &&
               gridPosition.y >= 0 && gridPosition.y < gridHeight;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPos = GridToWorldPostion(new Vector2Int(x, y));
                Gizmos.DrawWireCube(worldPos, Vector3.one * cellSize);
            }
        }
    }


}
