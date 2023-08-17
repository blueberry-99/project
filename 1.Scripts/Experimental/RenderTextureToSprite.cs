using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureToSprite : MonoBehaviour
{
    public RenderTexture renderTexture;
    SpriteRenderer SpriteRenderer;

    Rect rect;

    Texture2D texture2D;

    // Start is called before the first frame update

    void Awake()
    {

    }
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

    }

    // Update is called once per frame
    void Update()
    {
        texture2D = toTexture2D(renderTexture);
        SpriteRenderer.sprite = Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1920, 1080, TextureFormat.RGB48, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

}
