using System.Collections;
using UnityEngine;

namespace ModList.Components
{
    [RequireComponent(typeof(ModListUI))]
    public class ErrorsButtons : MonoBehaviour
    {
        ModListUI modListUI;

        void Start()
        {
            modListUI = GetComponent<ModListUI>();
            modListUI.ErrorsBackButton.onClick.AddListener(BackWasPressed);
        }

        void BackWasPressed()
        {
            modListUI.ErrorsView.SetActive(false);
            modListUI.ModListView.SetActive(true);
        }
    }
}