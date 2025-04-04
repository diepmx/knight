using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemapGenerator : MonoBehaviour
{
    [Space(10)]
    // Tham chiếu đến thành phần Tilemap.
    public Tilemap tilemap;
    // Mảng các ô gạch (tile) ngẫu nhiên để lựa chọn.
    public TileBase[] randomTiles;

    [Space(10)]
    // Chiều rộng của tilemap.
    public int width = 1000;
    // Chiều cao của tilemap. 
    public int height = 1000;

    [Space(10)]
    // Tần suất xuất hiện của các ô gạch ngẫu nhiên.
    public float randomTileFrequency = 0.02f;

    void Start()
    {
        // Gọi hàm để tạo tilemap ngẫu nhiên khi bắt đầu.
        GenerateTilemap();
    }

    void GenerateTilemap()
    {
        // Lặp qua toàn bộ tilemap để đặt ô gạch.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Kiểm tra xem ô hiện tại có ô lân cận nào đã được đặt gạch chưa.
                bool occupiedNeighbor = CheckOccupiedNeighbors(x, y);

                // Nếu không có ô lân cận nào đã đặt gạch và giá trị ngẫu nhiên cho phép, đặt ô gạch ngẫu nhiên.
                if (!occupiedNeighbor && Random.value < randomTileFrequency)
                {
                    TileBase randomTile = randomTiles[Random.Range(0, randomTiles.Length)];
                    tilemap.SetTile(new Vector3Int(x, y, 0), randomTile);
                }
            }
        }
    }

    // Phương thức kiểm tra xem có ô lân cận nào đã đặt gạch hay không.
    bool CheckOccupiedNeighbors(int x, int y)
    {
        // Lặp qua 9 ô xung quanh ô hiện tại (bao gồm cả đường chéo).
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                Vector3Int neighbor = new Vector3Int(i, j, 0);

                // Nếu ô gạch đã được đặt, trả về true.
                if (tilemap.GetTile(neighbor) != null)
                {
                    return true;
                }
            }
        }

        // Nếu không có ô nào bị chiếm, trả về false.
        return false;
    }
}
