using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CivType
{
    None,
    Nile,
    Yangtze,
    Zana,
    Planet,
    Robo,
    Lava,
}

public class CivilizationManager : Singleton<CivilizationManager>
{
    public List<Civilization> startingCivs = new List<Civilization>();
    public List<Civilization> unlockedCivs = new List<Civilization>();

    private List<Civilization> currentCivs = new List<Civilization>();

    [SerializeField]
    private GameObject singleCivParent;

    [SerializeField]
    private GameObject tabsParent;

    [SerializeField]
    private List<CivTab> tabs;

    [SerializeField]
    private GameObject viewBlocker;

    [SerializeField]
    private bool debugUnlockCivs;

    private CivTab currentTab;
    private bool didUnlock;

    private void Start()
    {
        currentCivs.AddRange(startingCivs);
        tabs.ForEach(x => x.CursorSelectStarted += (Interactable i) => OnTabClicked(i.GetComponent<CivTab>()));

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Earth")
        {
            OnTabClicked(tabs[0]);
        }

#if UNITY_EDITOR
        if (debugUnlockCivs)
        {
            UnlockAll();
        }
#endif
    }

    public void UnlockAll()
    {
        if (!didUnlock && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Earth")
        {
            AudioController.Instance.SetLoopAndPlay("earth");

            singleCivParent.gameObject.SetActive(false);
            tabsParent.gameObject.SetActive(true);

            currentCivs.AddRange(unlockedCivs);
            OnTabClicked(tabs[0]);

            didUnlock = true;
        }
    }

    public override void ManagedUpdate()
    {
        float timeStep = Time.deltaTime;
        currentCivs.ForEach(x => x.Tick(timeStep));
    }

    private void OnTabClicked(CivTab tab)
    {
        if (tab != currentTab)
        {
            currentCivs.ForEach(x => x.gameObject.SetActive(false));
            tab.civ.gameObject.SetActive(true);

            tabs.ForEach(x => x.text.SetColor(ColorUtils.GetColorWithAlpha(x.text.Color, 0.33f)));
            tab.text.SetColor(ColorUtils.GetColorWithAlpha(tab.text.Color, 1f));

            viewBlocker.gameObject.SetActive(true);
            CustomCoroutine.WaitThenExecute(0.175f, () => viewBlocker.gameObject.SetActive(false));
        }

        currentTab = tab;
    }
}
