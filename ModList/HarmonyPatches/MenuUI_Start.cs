using System.Text;
using BepInEx.Bootstrap;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModList.HarmonyPatches
{
    [HarmonyPatch(typeof(MenuUI))]
    [HarmonyPatch(nameof(MenuUI.Start))]
    // ReSharper disable once InconsistentNaming
    public static class MenuUI_Start
    {
        [HarmonyPostfix]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static void Postfix(MenuUI __instance)
        {
            Transform mainUi = __instance.mainUi.transform;
            Transform buttons = mainUi.Find("Buttons");
            
            // This is added to ensure that all buttons remain on screen and it doesnt look scuffed
            var contentSizeFitter = buttons.gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Transform buttonTemplate = buttons.Find("MultiplayerButton");
            GameObject modListButton = Object.Instantiate(buttonTemplate.gameObject, buttons, true);
            modListButton.name = "ModsButton";
            modListButton.transform.SetSiblingIndex(3); // Below "Setting", above "Achievements"

            // Change the text to not be "New Lobby"
            var buttonText = modListButton.GetComponentInChildren<TextMeshProUGUI>();
            int pluginCount = Chainloader.PluginInfos.Count;
            buttonText.text = "Mods (" + pluginCount + ")";
            
            // Load UI
            GameObject uiPrefab = UiLoader.Instance.LoadUiAsset();
            GameObject ui = Object.Instantiate(uiPrefab, __instance.transform);
            ui.SetActive(false);

            var button = modListButton.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(() =>
            {
                __instance.mainUi.SetActive(false);
                ui.SetActive(true);
            });
        }
    }
}
