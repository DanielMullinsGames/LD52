using UnityEngine;

public class SetMaterialColorOnStart : MonoBehaviour
{
    [SerializeField]
    private Renderer coloredRenderer = default;

    [SerializeField]
    private Color color = default;

    [SerializeField]
    private string materialColorId = default;

    void Start()
    {
        coloredRenderer.material.SetColor(materialColorId, color);
    }
}
