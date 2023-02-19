using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    public static Sprite skinToLoad;
    [SerializeField] private Sprite _defaultSkin;
    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = skinToLoad == null ? _defaultSkin : skinToLoad;
    }
}

