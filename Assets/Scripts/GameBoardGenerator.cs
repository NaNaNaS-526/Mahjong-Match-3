using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoardGenerator : MonoBehaviour
{
    [SerializeField] private Tile[] tilesPrefabs;
    [SerializeField] private SpriteRenderer gameBoard;

    [SerializeField] private int layersAmount;
    [SerializeField] private int bottomLayerHeight;
    [SerializeField] private int bottomLayerWidth;

    private List<Tile[,]> _createdTiles;

    private float _leftBoardBound;
    private float _bottomBoardBound;

    private void Awake()
    {
        InitializeGameBoardBounds();
        InitializeCreatedTileArray();
        GenerateGameBoard();
    }

    private void GenerateGameBoard()
    {
        for (int i = 0; i < _createdTiles.Count; i++)
        {
            int rows = _createdTiles[i].GetUpperBound(0) + 1;
            int columns = _createdTiles[i].Length / rows;
            float offset = i * 0.5f;
            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; k++)
                {
                    float leftPoint = _leftBoardBound + j + 0.5f + offset;
                    float bottomPoint = _bottomBoardBound + k + 0.5f + offset;
                    Vector3 newTilePosition = new Vector3(leftPoint, bottomPoint, i * -0.1f);

                    CreateTile(newTilePosition, i, j, k);
                }
            }
        }
    }

    private void CreateTile(Vector3 spawnPosition, int layer, int x, int y)
    {
        Tile randomTile = tilesPrefabs[Random.Range(0, tilesPrefabs.Length)];
        var newTile = Instantiate(randomTile, spawnPosition, Quaternion.identity);
        _createdTiles[layer][x, y] = newTile;
        if (layer != 0)
        {
            CloseTiles(newTile, layer, x, y);
        }
    }

    private void CloseTiles(Tile tile, int layer, int x, int y)
    {
        List<Tile> newClosedTiles = new List<Tile>
        {
            _createdTiles[layer - 1][x, y],
            _createdTiles[layer - 1][x + 1, y],
            _createdTiles[layer - 1][x, y + 1],
            _createdTiles[layer - 1][x + 1, y + 1]
        };
        foreach (var closedTile in newClosedTiles.Where(closedTile => closedTile != null))
        {
            closedTile.AddClosingTile(tile);
        }
    }

    private void InitializeCreatedTileArray()
    {
        _createdTiles = new List<Tile[,]>();
        for (int i = 0; i < layersAmount; i++)
        {
            _createdTiles.Add(new Tile[bottomLayerWidth - i, bottomLayerHeight - i]);
        }
    }

    private void InitializeGameBoardBounds()
    {
        var bounds = gameBoard.bounds;
        _leftBoardBound = bounds.min.x;
        _bottomBoardBound = bounds.min.y;
    }
}