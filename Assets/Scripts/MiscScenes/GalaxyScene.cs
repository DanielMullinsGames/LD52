using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class GalaxyScene : TimedBehaviour
{
    [Header("Galaxy")]
    [SerializeField]
    private Transform straw;

    [SerializeField]
    private List<GameObject> allBobas;

    [SerializeField]
    private List<GameObject> bobaGroups;

    [SerializeField]
    private JitterPosition shaker;

    [SerializeField]
    private Transform strawOpening;

    private int boba;

    private void Start()
    {
        bobaGroups.ForEach(x => x.SetActive(false));
    }

    protected override void OnTimerReached()
    {
        CheckForNewBoba();

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.G))
        {
            AddBoba();
            AddBoba();
            AddBoba();
        }
#endif
    }

    private void CheckForNewBoba()
    {
        if (PlayerPrefs.GetInt("boba") > 0)
        {
            AddBoba();
            PlayerPrefs.SetInt("boba", 0);
        }
    }

    private void AddBoba()
    {
        if (boba < bobaGroups.Count)
        {
            bobaGroups[boba].SetActive(true);
        }

        boba++;

        if (boba == 3)
        {
            StartCoroutine(Finale());
        }
    }

    private IEnumerator Finale()
    {
        StartCoroutine(IncreasingShake(12f));
        yield return new WaitForSeconds(5f);

        straw.gameObject.SetActive(true);
        var pos = straw.position;
        straw.position = straw.position + Vector3.up * 3f;

        Tween.Position(straw, pos, 5f, 0f, Tween.EaseLinear);
        yield return new WaitForSeconds(5f);

        //play sound
        foreach (var boba in allBobas)
        {
            Vector2 dir = (strawOpening.position - boba.transform.position).normalized * 0.25f;
            Tween.Position(boba.transform, (Vector2)boba.transform.position + dir, 2f, 0f, Tween.EaseInStrong);
        }

        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Thanks");
    }

    private IEnumerator IncreasingShake(float duration)
    {
        float startTime = Time.time;
        float shakeAmount = 0f;
        while (Time.time < startTime + duration)
        {
            shakeAmount += Time.deltaTime * 1f;
            shaker.amount = shakeAmount;
            yield return new WaitForEndOfFrame();
        }
        shaker.amount = 0f;
    }
}
