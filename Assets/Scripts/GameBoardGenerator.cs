using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoardGenerator : MonoBehaviour
{
    [SerializeField] private Tile[] tilesPrefabs;
    [SerializeField] private int layersAmount;
    [SerializeField] private int tilesOnLayerAmountMax;

    private Tile[][] _createdTiles;

    private void Awake()
    {
        SetupCreatedTileArray();
    }

    private void Start()
    {
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < layersAmount; i++)
        {
            for (int j = 0; j < _createdTiles[i].Length; j++)
            {
                if (i == 0)
                {
                    Vector3 randomPosition = new Vector3(Random.Range(i, j), Random.Range(i, j), 0.0f);
                    CreateTile(i, j, randomPosition);
                }
                else
                {
                    CreateTile(i, j, _createdTiles[i - 1][Random.Range(0, j)].GetRandomCorner());
                }
            }
        }
    }

    private void CreateTile(int layer, int tilesAmount, Vector3 spawnPosition)
    {
        Tile randomTile = tilesPrefabs[Random.Range(0, tilesPrefabs.Length)];
        Tile newTile = Instantiate(randomTile, spawnPosition, Quaternion.identity);
        var tilePosition = newTile.transform.position;
        newTile.SetupCorners(tilePosition);
        _createdTiles[layer][tilesAmount] = newTile;
    }

    private void SetupCreatedTileArray()
    {
        _createdTiles = new Tile[layersAmount][];
        for (int i = 0; i < layersAmount; i++)
        {
            _createdTiles[i] = new Tile[tilesOnLayerAmountMax - (i + 2)];
        }
    }
}