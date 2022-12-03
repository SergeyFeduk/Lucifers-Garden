using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSorted : MonoBehaviour
{
    private const float resolution = 10;
    [SerializeField] private int sortingOrderBase;
    [SerializeField] private float offset;
    [SerializeField] private bool isStatic = false;

    private Renderer myRenderer => GetComponent<Renderer>();

    private void Update()
    {
        myRenderer.sortingOrder = (int)(resolution * (sortingOrderBase - transform.position.y - offset));
        if (isStatic) {
            Destroy(this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - offset, 0), Vector3.one * 0.1f);
    }
}
