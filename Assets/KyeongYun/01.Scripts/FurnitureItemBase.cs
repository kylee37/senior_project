using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class FurnitureItemBase : MonoBehaviour
{
    protected bool isOnAllowedTilemap;
    protected TilemapCollider2D allowedTilemapCollider;
    protected BoxCollider2D boxCollider;

    protected abstract string AllowedTilemapName { get; }

    void Start()
    {
        GameObject grid = GameObject.Find("Grid");
        if (grid != null)
        {
            Transform tilemapTransform = grid.transform.Find(AllowedTilemapName);
            if (tilemapTransform != null)
            {
                allowedTilemapCollider = tilemapTransform.GetComponent<TilemapCollider2D>();
            }
            else
            {
                Debug.LogError("Tilemap not found: " + AllowedTilemapName);
            }
        }
        else
        {
            Debug.LogError("Grid object not found!");
        }

        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CheckIfInsideAllowedTilemap();
    }

    private void CheckIfInsideAllowedTilemap()
    {
        if (allowedTilemapCollider == null || boxCollider == null)
        {
            isOnAllowedTilemap = false;
            return;
        }

        Vector2[] boxCorners = GetBoxColliderCorners(boxCollider);
        isOnAllowedTilemap = true;

        foreach (Vector2 corner in boxCorners)
        {
            if (!allowedTilemapCollider.OverlapPoint(corner))
            {
                isOnAllowedTilemap = false;
                break;
            }
        }
    }

    private Vector2[] GetBoxColliderCorners(BoxCollider2D boxCollider)
    {
        Vector2[] corners = new Vector2[4];
        Vector2 size = boxCollider.size;
        Vector2 offset = boxCollider.offset;
        Vector2 position = boxCollider.transform.position;

        corners[0] = position + offset + new Vector2(-size.x, -size.y) * 0.5f;
        corners[1] = position + offset + new Vector2(size.x, -size.y) * 0.5f;
        corners[2] = position + offset + new Vector2(size.x, size.y) * 0.5f;
        corners[3] = position + offset + new Vector2(-size.x, size.y) * 0.5f;

        return corners;
    }

    public bool CanPlace()
    {
        return isOnAllowedTilemap && !IsOverlappingWithOtherFurniture();
    }

    private bool IsOverlappingWithOtherFurniture()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider != boxCollider && collider.gameObject.CompareTag("Furniture"))
            {
                return true;
            }
        }
        return false;
    }
}
