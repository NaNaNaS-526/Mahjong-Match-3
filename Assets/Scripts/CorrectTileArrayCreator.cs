using System.Collections.Generic;

public static class CorrectTileArrayCreator
{
    private static List<Tile> newTileList = new();
    private static int exceeding;

    public static Tile[] CreateCorrectTileArray(Tile[] tilesPrefabs, int tilesOnBoardAmount)
    {
        newTileList.Clear();
        for (int i = 0; i < tilesOnBoardAmount / 3; i++)
        {
            AddTilesToNewTileList(newTileList, tilesPrefabs, i);
        }

        ShuffleNewTileList();
        return newTileList.ToArray();
    }

    private static void AddTilesToNewTileList(List<Tile> newTilesList, Tile[] tilesPrefabs, int index)
    {
        if (index >= tilesPrefabs.Length)
        {
            exceeding = index / tilesPrefabs.Length;
            index -= tilesPrefabs.Length * exceeding;
        }

        for (int j = 0; j < 3; j++)
        {
            newTilesList.Add(tilesPrefabs[index]);
        }
    }

    private static void ShuffleNewTileList()
    {
        System.Random rnd = new System.Random();
        for (int i = newTileList.Count - 1; i > 0; i--)
        {
            int index = rnd.Next(i + 1);
            (newTileList[i], newTileList[index]) = (newTileList[index], newTileList[i]);
        }
    }
}