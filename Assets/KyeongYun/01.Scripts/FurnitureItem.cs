using UnityEngine;
using UnityEngine.Tilemaps;

public class FurnitureItem : MonoBehaviour
{
    [SerializeField] private Tilemap allowedTilemap; // ��ġ ������ Ÿ�ϸ�

    private bool isOnAllowedTilemap;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Tilemap>() == allowedTilemap)
        {
            isOnAllowedTilemap = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Tilemap>() == allowedTilemap)
        {
            isOnAllowedTilemap = false;
        }
    }

    public bool CanPlace()
    {
        return isOnAllowedTilemap;
    }
}
