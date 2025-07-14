using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mfram.Map
{
public class GridMapManger : MonoBehaviour
{
   [Header("地圖資訊")]
   public List<MapData_SO> mapDataList;

   private void Start()
   {
      foreach (var mapData in mapDataList)
      {
         InitTileDetailsDict(mapData);
      }
   }

   //場景名字+座標和對應的網格資料
   private Dictionary<string,TileDetails>tileDetailsDict = new Dictionary<string, TileDetails>();

   private void InitTileDetailsDict(MapData_SO mapData)
   {
      foreach (TileProperty tileProperty in mapData.tileproperties)
      {
         TileDetails tileDetails = new TileDetails
         {
            gridX = tileProperty.tileCoordinates.x,
            gridY = tileProperty.tileCoordinates.y
         };
      //字典的key
      string key = tileDetails.gridX+"X"+tileDetails.gridY+"Y"+mapData.sceneName;
      if (GetTileDetails(key)!=null)
      {
         tileDetails=GetTileDetails(key);
      }

      switch (tileProperty.gridType)
      {
         case GridType.Diggable:
            tileDetails.canDig = true;
            break;
         case GridType.DropItem:
            tileDetails.canDropItem = true;
            break;
         case GridType.PlaceFurniture:
            tileDetails.canPlaceFurniture = true;
            break;
         case GridType.NPCObastacle:
            tileDetails.isNPCObstacle = true;
            break;
      }

      if (GetTileDetails(key)!=null)
      {
         tileDetails = GetTileDetails(key);
        
      }
      else
      {
         tileDetailsDict.Add(key, tileDetails);
      }
      }
      
   }
/// <summary>
/// 根據key返回網格資料
/// </summary>
/// <param name="key">x+y+地圖名字</param>
/// <returns></returns>
   private TileDetails GetTileDetails(string key)
   {
      if (tileDetailsDict.ContainsKey(key))
      {
         return tileDetailsDict[key];
      }
      return null;
   }

}
    
}
