using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    SpriteRenderer sprite;
    public float colorLooseRate = 2;
    private void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
        colorLooseRate = 2;
        sprite.color = new Color(1, 1, 1, 1);
    }
    private void Update()
    {
        float newAlpha = sprite.color.a - colorLooseRate * Time.deltaTime;
        sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,newAlpha);
        if (newAlpha <= 0)
            gameObject.SetActive(false);
    }
}
