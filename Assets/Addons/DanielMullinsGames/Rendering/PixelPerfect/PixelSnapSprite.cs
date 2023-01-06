using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class PixelSnapSprite : PixelSnapElement
{
    private Sprite sprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnWillRenderObject()
    {
        TryAdjustPositionsForHierarchy();
    }

    protected override void SnapToPixelGrid()
    {
        if (spriteRenderer != null)
        {
            sprite = spriteRenderer.sprite;
        }
        else
        {
            sprite = null;
        }

        Camera cam = Camera.main;
        if (cam != null && sprite != null)
        {
            shouldRestorePosition = true;
            cachedLocalPos = transform.localPosition;

            float cameraPPU = (float)cam.pixelHeight / (2f * cam.orthographicSize);

            Vector3 initialPosition = transform.position;
            Vector2 camPos = cam.transform.position;
            Vector2 pos = initialPosition;
            Vector2 relPos = pos - camPos;

            Vector2 offset = new Vector2(0, 0);

            // offset for screen pixel edge if screen size is odd
            offset.x = (cam.pixelWidth % 2 == 0) ? 0 : 0.5f;
            offset.y = (cam.pixelHeight % 2 == 0) ? 0 : 0.5f;

            // The vector from the nearest texel edge to the pivot towards the pivot in texture pixels 
            Vector2 pivotOffset = sprite.pivot - new Vector2(Mathf.Round(sprite.pivot.x), Mathf.Round(sprite.pivot.y)); // the fractional part in texture pixels

            float assetPPU = 100f;
            float assetUPP = 1.0f / assetPPU;
            float camPixelsPerAssetPixel = cameraPPU / assetPPU;

            offset.x /= camPixelsPerAssetPixel; // zero or half a screen pixel in texture pixels
            offset.y /= camPixelsPerAssetPixel;

            // Convert from units to asset pixels, round them, convert back to units.
            relPos.x = (Mathf.Round(relPos.x / assetUPP - offset.x) + offset.x + pivotOffset.x) * assetUPP;
            relPos.y = (Mathf.Round(relPos.y / assetUPP - offset.y) + offset.y + pivotOffset.y) * assetUPP;

            // The offsets make sure that the distance we round is from screen pixel (fragment) edges to texel edges.
            // We don't take into account the pivot offset when rounding to avoid the artifact where 2 sprites appear to move at different times 
            // because of the fractional part of their pivots. We also consider the subtexel part of the pivot as an error which we want to correct.
            pos = relPos + camPos;

            transform.position = new Vector3(pos.x, pos.y, initialPosition.z);
        }
    }
}
