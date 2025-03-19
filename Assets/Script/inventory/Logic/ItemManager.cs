using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mfram.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item ItemPrefab;

        private Transform itemParent;

        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        }

        private void start()
        {
            itemParent = GameObject.FindGameObjectWithTag("ItemParent").transform;
        }

        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(ItemPrefab, pos, Quaternion.identity,itemParent);
            item.itemID = ID;
        }

    }

}
