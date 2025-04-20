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
            EventHandler.AfterTransitionEvent   += OnAfterTransitionEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
            EventHandler.AfterTransitionEvent   -= OnAfterTransitionEvent;
        }

        private void OnAfterTransitionEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            
        }


        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(ItemPrefab, pos, Quaternion.identity,itemParent);
            item.itemID = ID;
        }

    }

}
