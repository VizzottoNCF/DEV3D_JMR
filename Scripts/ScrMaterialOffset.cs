using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrMaterialOffset : MonoBehaviour
{
    public Material myMaterial;

    public bool moveOffsetX = false;
    public bool moveOffsetY = false;

    public float offsetX = 0f;
    public float offsetY = 0f;

    public Vector2 newOffset;

    [Range(-3f,3f)] public float speedX = 1f;
    [Range(-3f,3f)] public float speedY = 1f;

    private void Start()
    {
        myMaterial = this.GetComponent<Material>();
    }

    void Update()
    {
        if (moveOffsetX)
        {
            newOffset.x += speedX * Time.deltaTime;

            if (newOffset.x > 1f)
            {
                newOffset.x -= 1f;
            }
            if (newOffset.x < -1f)
            {
                newOffset.x += 1f;
            }
        }
        
        if (moveOffsetY)
        {
            newOffset.y += speedY * Time.deltaTime;

            if (newOffset.y > 1f)
            {
                newOffset.y -= 1f;
            }
            if (newOffset.y < -1f)
            {
                newOffset.y += 1f;
            }
        }
        


        myMaterial.mainTextureOffset = newOffset;
    }
}
