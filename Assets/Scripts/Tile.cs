using UniRx;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public ReactiveProperty<bool> isOpened;

    public Vector3 leftTopCorner;
    public Vector3 rightTopCorner;
    public Vector3 leftBottomCorner;
    public Vector3 rightBottomCorner;

    public void SetupCorners(Vector3 tilePosition)
    {
        leftTopCorner = new Vector3(tilePosition.x - 0.5f, tilePosition.y + 0.5f, 0);
        rightTopCorner = new Vector3(tilePosition.x + 0.5f, tilePosition.y + 0.5f, 0);
        leftBottomCorner = new Vector3(tilePosition.x - 0.5f, tilePosition.y - 0.5f, 0);
        rightBottomCorner = new Vector3(tilePosition.x + 0.5f, tilePosition.y - 0.5f, 0);
    }

    public Vector3 GetRandomCorner()
    {
        int randomNumber = Random.Range(0, 4);
        switch (randomNumber)
        {
            case 0: return leftTopCorner;
            case 1: return rightTopCorner;
            case 2: return leftBottomCorner;
            case 3: return rightBottomCorner;
        }

        return leftBottomCorner;
    }
}