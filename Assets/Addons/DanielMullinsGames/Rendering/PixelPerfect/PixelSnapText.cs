using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Text))]
public class PixelSnapText : PixelSnapElement
{
    private const int PPU = 100;

    private void OnGUI()
    {
        TryAdjustPositionsForHierarchy();
    }

    protected override void SnapToPixelGrid()
    {
        if (transform.parent != null)
        {
            shouldRestorePosition = true;
            cachedLocalPos = transform.localPosition;

            Vector3 newLocalPosition = Vector3.zero;

            newLocalPosition.x = (Mathf.Round(transform.parent.position.x * PPU) / PPU) - transform.parent.position.x;
            newLocalPosition.y = (Mathf.Round(transform.parent.position.y * PPU) / PPU) - transform.parent.position.y;
            newLocalPosition.z = transform.localPosition.z;

            newLocalPosition.x /= 0.01f;
            newLocalPosition.y /= 0.01f;

            transform.localPosition = newLocalPosition;
        }
    }
}