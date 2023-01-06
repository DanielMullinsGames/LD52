using UnityEngine;
using System.Collections.Generic;

public class RenderTextureSetter : MonoBehaviour
{
    [SerializeField]
    private List<Camera> pixelSceneCameras = default;

    [SerializeField]
    private List<UnityEngine.UI.RawImage> renderImages = default;

    private int prevScreenWidth = 0;
    private int prevScreenHeight = 0;
    private int prevScaleFactor = 0;

    public void FormatForResolution(int width, int height, int scaleFactor)
    {
        bool createRenderTexture = prevScreenWidth != width || prevScreenHeight != height || prevScaleFactor != scaleFactor;

        for (int i = 0; i < pixelSceneCameras.Count; i++)
        {
            var pixelSceneCamera = pixelSceneCameras[i];
            var renderImage = renderImages[i];

            pixelSceneCamera.orthographicSize = (height / 2) / 100f;

            if (createRenderTexture)
            {
                CleanUpCurrentRenderTexture(pixelSceneCamera);

                var renderTexture = CreateRenderTexture(width, height);
                pixelSceneCamera.targetTexture = renderTexture;
                renderImage.texture = renderTexture;
                renderImage.rectTransform.sizeDelta = new Vector2(width * scaleFactor, height * scaleFactor);
            }

            Rect rect = pixelSceneCamera.pixelRect;
            rect.width = width;
            rect.height = height;
            pixelSceneCamera.pixelRect = rect;
        }

        prevScreenWidth = width;
        prevScreenHeight = height;
        prevScaleFactor = scaleFactor;
    }

    private RenderTexture CreateRenderTexture(int width, int height)
    {
        var renderTexture = new RenderTexture(width, height,
            16, UnityEngine.Experimental.Rendering.DefaultFormat.HDR);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.useMipMap = false;
        renderTexture.Create();

        return renderTexture;
    }

    private void CleanUpCurrentRenderTexture(Camera cam)
    {
        if (cam.targetTexture != null)
        {
            var texture = cam.targetTexture;
            cam.targetTexture = null;
            Destroy(texture);
        }
    }
}
