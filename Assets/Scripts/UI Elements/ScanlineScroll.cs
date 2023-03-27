using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanlineScroll : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private float speed;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}