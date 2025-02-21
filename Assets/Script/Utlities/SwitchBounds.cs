
using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    private void Start()
    {
        SwitchConfinerShape();
    }
    private void SwitchConfinerShape() 
    {
        PolygonCollider2D confinershape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D= confinershape;
        //每次更換場景BOUNDS清除緩存
        confiner.InvalidatePathCache();
    }
}
