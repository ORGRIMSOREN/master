using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapData;
    public GridType gridType;
    private Tilemap currentTilemap;

    private void OnEnable()
    {
        //初始化Tilemap資料
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();

            if (mapData !=null)mapData.tileproperties.Clear();
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            UpdateTilemapProperties();
        #if UNITY_EDITOR
            if (mapData != null)EditorUtility.SetDirty(this);
        #endif
        }
    }

    private void UpdateTilemapProperties()
    {
        currentTilemap.CompressBounds();

        if (!Application.IsPlaying(this))
        {
            if (mapData != null)
            {
                //取得已繪製範圍的左下角為最小值座標
                Vector3Int startPos =currentTilemap.cellBounds.min;
                //取得已繪製範圍的右上角為最大值座標
                Vector3Int endPos = currentTilemap.cellBounds.max;
            
                //循環座標範圍獲得地圖網格資料並加入Map_SO中
                for (int x = startPos.x ; x < endPos.x ; x++)
            {
                for (int y = startPos.y ; y < endPos.y ; y++)
                {
                    TileBase tile = currentTilemap.GetTile(new Vector3Int(x,y,0)); 
                    
                    if (tile !=null )
                    {
                        TileProperty tileProperty = new TileProperty
                        {
                          tileCoordinates  = new Vector2Int(x, y),
                          gridType = this.gridType,
                          boolTypeValue = true
                        };
                        mapData.tileproperties.Add(tileProperty);
                    }
                }
            }
            }

        }
        
    }
}
