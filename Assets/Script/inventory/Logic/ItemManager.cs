using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mfram.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item ItemPrefab;

        private Transform itemParent;
        
        //紀錄場景物品
        private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string,List<SceneItem>>();
        
        

        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
            EventHandler.BeforeTransitionEvent  += OnBeforeTransitionEvent;
            EventHandler.AfterTransitionEvent   += OnAfterTransitionEvent;
        }


        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.BeforeTransitionEvent -= OnBeforeTransitionEvent;
            EventHandler.AfterTransitionEvent   -= OnAfterTransitionEvent;
        }
        private void OnBeforeTransitionEvent()
        {
            GetSceneItems();
        }

        private void OnAfterTransitionEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            ResetSceneItems();
        }


        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(ItemPrefab, pos, Quaternion.identity,itemParent);
            item.itemID = ID;
        }
        
        private void GetSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position  = new SerializableVector3(item.transform.position),
                };
                currentSceneItems.Add(sceneItem);
            }

            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //取得當啟用的場景中的物品並更新列表
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else
            {
                //如果是新場景加入則加入表
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }
        /// <summary>
        /// 重新載入場景物件
        /// </summary>
        private void ResetSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems!=null)
                {
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(ItemPrefab, item.position.ToVector3(), Quaternion.identity,itemParent);
                        newItem.Init(item.itemID);
                    }
                } 
            }
        }
    }

}
