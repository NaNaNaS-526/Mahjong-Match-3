using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class MergePanel : MonoBehaviour
{
    [SerializeField] private GameObject levelFailPanel;
    [SerializeField] private GameObject levelCompletePanel;

    private GameBoardGenerator _gameBoardGenerator;

    public List<Tile> tilesOnPanel;

    private SpriteRenderer _spriteRenderer;
    private float _leftBound;
    private const float Offset = 0.6f;

    private int _maxTilesAmount;

    [Inject]
    private void Construct(GameBoardGenerator gameBoardGenerator)
    {
        _gameBoardGenerator = gameBoardGenerator;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        levelFailPanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        InitializeLeftBound();
        SignUpForTileUpdates();
        InitializeTileList();
    }

    private void SignUpForTileUpdates()
    {
        var allTiles = _gameBoardGenerator.GetCreatedTiles();
        foreach (var array in allTiles)
        {
            foreach (var tile in array)
            {
                tile.OnTileClicked += DetermineDestinationPoint;
            }
        }
    }

    private void DetermineDestinationPoint(Tile tile)
    {
        float y = transform.position.y;
        float globalOffset = _leftBound + Offset;
        var destinationPoint = new Vector3(globalOffset + tilesOnPanel.Count, y, 0.01f);
        foreach (var trialTile in tilesOnPanel.Where(t => tile.type == t.type))
        {
            int index = tilesOnPanel.IndexOf(trialTile);
            RearrangeTiles(index, tile);
            return;
        }

        MoveTileToDestinationPoint(tile, destinationPoint);
    }

    private void RearrangeTiles(int index, Tile tile)
    {
        float y = transform.position.y;
        float globalOffset = _leftBound + Offset;
        List<Tile> rightTiles = new List<Tile>();
        for (int i = index + 1; i < tilesOnPanel.Count;)
        {
            rightTiles.Add(tilesOnPanel[i]);
            tilesOnPanel.RemoveAt(i);
        }

        var destinationPoint = new Vector3(globalOffset + tilesOnPanel.Count, y, 0.01f);
        if (tile)
        {
            MoveTileToDestinationPoint(tile, destinationPoint);
        }

        foreach (var a in rightTiles)
        {
            destinationPoint = new Vector3(globalOffset + tilesOnPanel.Count, y, 0.01f);
            MoveTileToDestinationPoint(a, destinationPoint);
        }
    }

    private void MoveTileToDestinationPoint(Tile tile, Vector3 destinationPoint)
    {
        var position = tile.transform.position;
        tile.transform.DOMove(new Vector3(position.x, position.y, -1.0f), 0.01f);
        tile.transform.DOMove(destinationPoint, 1.0f).SetEase(Ease.InOutElastic).OnComplete(() =>
        {
            tilesOnPanel.Add(tile);
            CheckTileListFullness();
        });
    }

    private void CheckTileListFullness()
    {
        CheckTilesForMatches();
        if (tilesOnPanel.Count >= _maxTilesAmount) EndGame();
    }

    private void CheckTilesForMatches()
    {
        TileType currentType = TileType.None;
        List<Tile> matchingTiles = new List<Tile>();
        foreach (var verifiableTile in tilesOnPanel)
        {
            if (verifiableTile.type == currentType)
            {
                matchingTiles.Add(verifiableTile);
            }
            else
            {
                currentType = verifiableTile.type;
                matchingTiles.Clear();
                matchingTiles.Add(verifiableTile);
            }

            if (matchingTiles.Count >= 3)
            {
                RemoveSomeTiles(matchingTiles);
                break;
            }
        }
    }

    private void RemoveSomeTiles(List<Tile> matchingTiles)
    {
        int index = tilesOnPanel.IndexOf(matchingTiles[0]);
        foreach (var tile in matchingTiles)
        {
            tilesOnPanel.Remove(tile);
            Destroy(tile.gameObject, 0.1f);
        }

        RearrangeTiles(index, null);
    }

    private void EndGame()
    {
        levelFailPanel.SetActive(true);
    }

    private void InitializeLeftBound()
    {
        _leftBound = _spriteRenderer.bounds.min.x;
    }

    private void InitializeTileList()
    {
        (int, int) gameBoardSizes = _gameBoardGenerator.GetGameBoardSizes();
        int capacity = gameBoardSizes.Item1 > gameBoardSizes.Item2 ? gameBoardSizes.Item1 : gameBoardSizes.Item2;
        tilesOnPanel = new List<Tile>(capacity);
        _maxTilesAmount = capacity;
        transform.localScale = new Vector3(capacity, 1.2f);
    }
}