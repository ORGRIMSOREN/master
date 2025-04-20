
using System;
using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void OnEnable()
    {
        EventHandler.AfterTransitionEvent += SwitchConfinerShape;
    }

    private void OnDisable()
    {
        EventHandler.AfterTransitionEvent -= SwitchConfinerShape;
    }

    private void SwitchConfinerShape() 
    {
        PolygonCollider2D confinershape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D= confinershape;
        //ÿ�θ��Q����BOUNDS�������
        confiner.InvalidatePathCache();
    }
}
