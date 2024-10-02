using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinTilemapSpawner : MonoBehaviour
{
    public Tilemap tilemap; // 타일맵
    public GameObject GoldcoinPrefab; // 코인 프리팹
    public GameObject SilvercoinPrefab; // 코인 프리팹
    public TileBase GoldcoinTile; // 코인을 나타내는 타일
    public TileBase SilvercoinTile; // 코인을 나타내는 타일
    public GameObject Grid;

    public Transform CoinParent;

    void Start()
    {
        SpawnCoinsFromTilemap();
        Grid.SetActive(false);
    }

    void SpawnCoinsFromTilemap()
    {
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(position.x, position.y, position.z);
            if (tilemap.HasTile(localPlace))
            {
                TileBase tile = tilemap.GetTile(localPlace);
                if (tile == GoldcoinTile)
                {
                    Vector3 worldPosition = tilemap.CellToWorld(localPlace) + tilemap.tileAnchor;
                    Instantiate(GoldcoinPrefab, worldPosition, Quaternion.identity).transform.SetParent(CoinParent);
                }
                else if(tile == SilvercoinTile)
                {
                    Vector3 worldPosition = tilemap.CellToWorld(localPlace) + tilemap.tileAnchor;
                    Instantiate(SilvercoinPrefab, worldPosition, Quaternion.identity).transform.SetParent(CoinParent);
                }
            }
        }
    }
}