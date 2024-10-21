using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrFadeOutGameObject : MonoBehaviour
{
    public ScrGhostInteraction scrGhost;
    private bool StartedCoroutine = false;

    private void Start()
    {
        scrGhost = GetComponent<ScrGhostInteraction>();
    }

    private void Update()
    {
        if (scrGhost.FirstInteraction && !StartedCoroutine)
        {
            rf_FadeOutGameObject(1);
        }
    }

    public void rf_FadeOutGameObject(float _fadeSpeed)
    {
        StartCoroutine(rIE_FadeOutMaterial(_fadeSpeed));
    }

    public IEnumerator rIE_FadeOutMaterial(float _fadeSpeed)
    {
        // Grabs components
        Renderer rend = GetComponent<Renderer>();
        Color matColor = rend.material.color;
        float alpha = rend.material.color.a;

        while (rend.material.color.a > 0f)
        {
            // lowers alpha
            alpha -= Time.deltaTime / _fadeSpeed;
            rend.material.color = new Color(matColor.r, matColor.g, matColor.b, alpha);
            yield return null;
        }
        rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0f);
    }
}
