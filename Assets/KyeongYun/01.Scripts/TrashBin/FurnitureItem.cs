using UnityEngine;
using UnityEngine.Tilemaps;

public class FurnitureItem : MonoBehaviour
{
    public string allowedTilemapName; // 배치 가능한 타일맵 이름

    private bool isOnAllowedTilemap;
    private Tilemap allowedTilemap;
    private TilemapCollider2D allowedTilemapCollider;

    void Start()
    {
        // Start에서 Initialize를 호출하지 않음. 스포너에서 호출하도록 함.
    }

    public void Initialize()
    {
        // 이름으로 Tilemap과 TilemapCollider 찾기
        GameObject gridObject = GameObject.Find("Grid");
        if (gridObject != null)
        {
            Transform allowedTilemapTransform = gridObject.transform.Find(allowedTilemapName);
            if (allowedTilemapTransform != null)
            {
                allowedTilemap = allowedTilemapTransform.GetComponent<Tilemap>();
                allowedTilemapCollider = allowedTilemapTransform.GetComponent<TilemapCollider2D>();

                if (allowedTilemap == null || allowedTilemapCollider == null)
                {
                    Debug.LogError("Tilemap or TilemapCollider not found on " + allowedTilemapName);
                }
            }
            else
            {
                Debug.LogError("Transform with name " + allowedTilemapName + " not found under Grid");
            }
        }
        else
        {
            Debug.LogError("Grid object not found!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<TilemapCollider2D>() == allowedTilemapCollider)
        {
            isOnAllowedTilemap = IsAllColliderVerticesInsideTilemapCollider2D(GetComponent<BoxCollider2D>(), allowedTilemap, allowedTilemapCollider);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<TilemapCollider2D>() == allowedTilemapCollider)
        {
            isOnAllowedTilemap = false;
        }
    }

    public bool CanPlace()
    {
        return isOnAllowedTilemap;
    }

    private bool IsAllColliderVerticesInsideTilemapCollider2D(BoxCollider2D boxCollider, Tilemap tilemap, TilemapCollider2D tilemapCollider)
    {
        Vector2[] colliderVertices = GetColliderVertices(boxCollider);

        foreach (Vector2 vertex in colliderVertices)
        {
            Vector3Int tilePosition = tilemap.WorldToCell(vertex);
            if (!tilemapCollider.bounds.Contains(vertex) || tilemap.GetTile(tilePosition) == null)
            {
                return false;
            }
        }

        return true;
    }

    private Vector2[] GetColliderVertices(BoxCollider2D boxCollider)
    {
        Vector2[] vertices = new Vector2[4];
        Vector2 center = boxCollider.transform.TransformPoint(boxCollider.offset);

        float xExtent = boxCollider.size.x * Mathf.Abs(boxCollider.transform.lossyScale.x) / 2f;
        float yExtent = boxCollider.size.y * Mathf.Abs(boxCollider.transform.lossyScale.y) / 2f;

        vertices[0] = new Vector2(center.x - xExtent, center.y - yExtent);
        vertices[1] = new Vector2(center.x + xExtent, center.y - yExtent);
        vertices[2] = new Vector2(center.x - xExtent, center.y + yExtent);
        vertices[3] = new Vector2(center.x + xExtent, center.y + yExtent);

        return vertices;
    }
}
