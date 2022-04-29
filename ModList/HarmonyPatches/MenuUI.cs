using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using ModList.Components;
using static ModList.UIUtils;

namespace ModList.HarmonyPatches
{
    [HarmonyPatch(typeof(MenuUI))]
    [HarmonyPatch("Start")]
    class MenuUI_Start
    {
        private static void Postfix(MenuUI __instance)
        {
            Plugin.GetLogger().LogInfo("Getting main UI object");

            Plugin.GetLogger().LogInfo("Moving buttons");
            GameObject buttons = GameObject.Find("Buttons");
            buttons.transform.position += new Vector3(0f, 100f, 0f); // shift buttons to make room for our button

            Plugin.GetLogger().LogInfo("Adding ModList button");
            Button modListButton = CreateButton(buttons.transform, "Mods");
            modListButton.gameObject.name = "ModListOpenButton";
            modListButton.transform.SetSiblingIndex(2); // put it in the second position in the relative hierarchy

            Plugin.GetLogger().LogInfo("Instantaiting UI");
            GameObject modListUI = Object.Instantiate(Plugin.ModListUI);
            modListUI.AddComponent<ModListUI>();
            modListUI.AddComponent<ModListButtons>();
            modListUI.AddComponent<ErrorsButtons>();
            GameObject modView = modListUI.transform.Find("ModListView").gameObject;

            modListButton.onClick.AddListener(() =>
            {
                Plugin.GetLogger().LogInfo("ModListButton was pressed");
                modView.SetActive(true);
                __instance.gameObject.SetActive(false);
            });
        }
    }
}
