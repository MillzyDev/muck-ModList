using UnityEngine;

namespace ModList.Components
{
    [RequireComponent(typeof(ModListUI))]
    public class ModListButtons : MonoBehaviour
    {
        ModListUI modListUI;
        GameObject mainMenu;
        
        void Start()
        {
            modListUI = GetComponent<ModListUI>();
            mainMenu = Resources.FindObjectsOfTypeAll<MenuUI>()[0].gameObject;
            Plugin.GetLogger().LogDebug(mainMenu.ToString());

            modListUI.MainBackButton.onClick.AddListener(BackWasPressed);
            modListUI.ErrorsButton.onClick.AddListener(ErrorsWasPressed);
        }

        void BackWasPressed()
        {
            modListUI.ModListView.SetActive(false);
            mainMenu.SetActive(true);
        }

        void ErrorsWasPressed()
        {
            modListUI.ModListView.SetActive(false);
            modListUI.ErrorsView.SetActive(true);
        }
    }
}