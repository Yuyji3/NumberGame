using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    public List<Tile> tiles; // 에디터에서 할당
    public int spawnCount = 2;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnRandomTiles(1); // 게임 시작 시 1 숫자 2개 생성
    }

    public void SpawnRandomTiles(int number)
    {
        List<Tile> emptyTiles = tiles.FindAll(t => t.IsEmpty());

        for (int i = 0; i < spawnCount && emptyTiles.Count > 0; i++)
        {
            int index = Random.Range(0, emptyTiles.Count);
            emptyTiles[index].SetNumber(number);
            emptyTiles.RemoveAt(index);
        }
    }

    public void MergeTiles(Tile from, Tile to)
    {
        if (from == to || from.IsEmpty() || to.IsEmpty() || from.number != to.number)
            return;

        int newNumber = from.number + to.number;
        to.SetNumber(newNumber);
        from.Clear();

        SpawnRandomTiles(newNumber / 2);
    }
}
