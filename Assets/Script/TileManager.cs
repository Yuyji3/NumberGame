using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    private List<Tile> tiles = new List<Tile>(); // 자동으로 채움
    public int spawnCount = 1;

    void Awake()
    {
        instance = this;

        // 태그가 "Tile"인 오브젝트들 찾아서 tiles에 추가
        GameObject[] tileObjects = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject obj in tileObjects)
        {
            Tile tile = obj.GetComponent<Tile>();
            if (tile != null)
                tiles.Add(tile);
        }
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
