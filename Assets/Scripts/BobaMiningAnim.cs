using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaMiningAnim : ManagedBehaviour
{
    [SerializeField]
    private Civilization civ;

    [SerializeField]
    private SpriteRenderer bobaRenderer;

    public void StartHarvest()
    {
        GetComponent<Animator>().Play("harvest", 0, 0f);

        if (AudioController.Instance.BaseLoopSource.clip != null && AudioController.Instance.BaseLoopSource.clip.name == "nile")
        {
            AudioController.Instance.FadeOutLoop(2f);
        }
    }

    private void OnEnable()
    {
        if (civ.Technologies.Learned(TechnologyType.UnlockBoba))
        {
            GetComponent<Animator>().Play("harvest", 0, 0.5f + (Random.value * 0.5f));
        }
    }

    public void ShakeKeyframe()
    {
        ScreenShake.instance.AddIntensity(5f);
    }

    public void UnlockKeyframe()
    {
        CivilizationManager.Instance.UnlockAll();
    }

    public void ChangeSortLayerKeyframe()
    {
        bobaRenderer.sortingLayerName = "Tooltip";
    }

    public void ResetSortLayerKeyframe()
    {
        bobaRenderer.sortingLayerName = "Grid";
    }

    public void PlaySound()
    {
        AudioController.Instance.PlaySound2D("explosion");
    }

    public override void ManagedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H))
        {
            StartHarvest();
        }
    }
}
