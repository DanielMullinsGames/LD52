using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : ManagedBehaviour
{
    [SerializeField]
    private GameObject text1;

    [SerializeField]
    private GameObject text2;

    [SerializeField]
    private GameObject text3;

    [SerializeField]
    private GameObject text4;

    private void Start()
    {
        StartCoroutine(IntroSeq());
    }

    private IEnumerator IntroSeq()
    {
        yield return new WaitForSeconds(1f);
        text1.gameObject.SetActive(true);
        var clip = AudioController.Instance.PlaySound2D("milkyway_single");
        DontDestroyOnLoad(clip.gameObject);

        yield return new WaitForSeconds(5f);
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        text3.gameObject.SetActive(false);
        text4.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        text4.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Earth");
    }
}
