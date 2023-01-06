using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ReferenceResolutionSetter : Singleton<ReferenceResolutionSetter>
{
    public override bool UpdateWhenPaused => true;

    public int ReferenceHeight { get; private set; }
    public int ReferenceWidth => Screen.width / ScaleFactor;
    public int ScaleFactor { get; private set; }

    [SerializeField]
    private int referenceHeightMin = 240;

    [SerializeField]
    private int referenceHeightMax = 280;

    [SerializeField]
    private float referenceHeightMultiplier = 1f;

    [SerializeField]
    private float minAspectRatio = 1.75f;

    [SerializeField]
    private RenderTextureSetter renderTextureSetter = default;

    protected override void ManagedInitialize()
    {
        AssignCameraReferenceHeight();
    }

    public override void ManagedUpdate()
    {
        AssignCameraReferenceHeight();
    }

    public float GetYExtentTop()
    {
        return renderTextureSetter.transform.position.y + (ReferenceHeight * 0.005f);
    }

    public float GetYExtentBottom()
    {
        return renderTextureSetter.transform.position.y + (ReferenceHeight * -0.005f);
    }

    private void AssignCameraReferenceHeight()
    {
        var heightAndScale = GetReferenceHeightAndScaleFactor();
        ReferenceHeight = heightAndScale[0];
        ScaleFactor = heightAndScale[1];
        int width = MakeEven(Screen.width / ScaleFactor);

        renderTextureSetter.FormatForResolution(width, ReferenceHeight, ScaleFactor);
    }

    private int[] GetReferenceHeightAndScaleFactor()
    {
        int min = Mathf.RoundToInt(referenceHeightMin * referenceHeightMultiplier);
        int max = Mathf.RoundToInt(referenceHeightMax * referenceHeightMultiplier);

        int height = Screen.height;

        // Limit height to prevent an aspect ratio smaller than the min
        float aspectRatio = Screen.width / (float)Screen.height;
        if (aspectRatio < minAspectRatio)
        {
            height = Mathf.RoundToInt(Screen.width / minAspectRatio);
        }

        // Find the highest multiple that resolution can be scaled by while staying within range
        int scale = 1;
        while (height / scale > max && height / (scale + 1) >= min)
        {
            scale++;
        }
        height = height / scale;

        // Clamp height between the min and max
        int clampedHeight = Mathf.Clamp(height, min, max);

        // Make height even
        int evenHeight = MakeEven(clampedHeight);

        return new int[] { evenHeight, scale };
    }

    private int MakeEven(int num)
    {
        return num - (num % 2 != 0 ? 1 : 0);
    }

    private void OnDrawGizmos()
    {
        DrawRenderableArea();
    }

    private void DrawRenderableArea()
    {
        DrawRectForResolution(referenceHeightMin, Color.cyan);
        DrawRectForResolution(referenceHeightMax, Color.blue);
    }

    private void DrawRectForResolution(int referenceHeight, Color c)
    {
        float width = referenceHeight * 1.777f;
        Vector2 topLeft = (Vector2)renderTextureSetter.transform.position + new Vector2(width * -0.005f, referenceHeight * 0.005f);
        Vector2 topRight = (Vector2)renderTextureSetter.transform.position + new Vector2(width * 0.005f, referenceHeight * 0.005f);
        Vector2 bottomLeft = (Vector2)renderTextureSetter.transform.position + new Vector2(width * -0.005f, referenceHeight * -0.005f);
        Vector2 bottomRight = (Vector2)renderTextureSetter.transform.position + new Vector2(width * 0.005f, referenceHeight * -0.005f);
        Debug.DrawLine(topLeft, topRight, c);
        Debug.DrawLine(topRight, bottomRight, c);
        Debug.DrawLine(bottomRight, bottomLeft, c);
        Debug.DrawLine(bottomLeft, topLeft, c);
    }
}
