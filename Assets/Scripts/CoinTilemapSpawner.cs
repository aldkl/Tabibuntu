using UnityEngine;
using UnityEngine.Tilemaps;

public class CoinTilemapSpawner : MonoBehaviour
{
    public Tilemap tilemap; // Ÿ�ϸ�
    public GameObject GoldcoinPrefab; // ���� ������
    public GameObject SilvercoinPrefab; // ���� ������
    public TileBase GoldcoinTile; // ������ ��Ÿ���� Ÿ��
    public TileBase SilvercoinTile; // ������ ��Ÿ���� Ÿ��
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