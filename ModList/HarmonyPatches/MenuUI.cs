using BepInEx.Bootstrap;
using HarmonyLib;
using ModList.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static ModList.Plugin;

namespace ModList.HarmonyPatches
{
    [HarmonyPatch(typeof(global::MenuUI))]
    [HarmonyPatch("Start")]
    class MenuUI
    {
        static GameObject view;
        static GameObject main;
        static TextMeshProUGUI modsText;
        static int pageTotal;
        static Button nextButton;
        static Button prevButton;
        static TextMeshProUGUI pageCount;

        private static void Postfix(global::MenuUI __instance)
        {
            Plugin.logger.LogInfo("Getting main UI object");
            main = GameObject.Find("UI/Main");

            Plugin.logger.LogInfo("Moving buttons");
            GameObject buttons = GameObject.Find("Buttons");
            buttons.transform.position += new Vector3(0f, 100f, 0f); // shift buttons to make room for our button

            Plugin.logger.LogInfo("Adding ModList button");
            Button modListButton = UIUtils.CreateButton(buttons.transform, "Mods", new UnityAction(OnModsClick));
            modListButton.transform.SetSiblingIndex(2); // put it in the second position in the relative hierarchy

            // create a new canvas for our ui (the other ui object removes ours fsr)
            Plugin.logger.LogInfo("Creating canvas");
            GameObject canvas = UIUtils.CreateCanvas(RenderMode.ScreenSpaceCamera);
            //Object.DontDestroyOnLoad(canvas);

            GameObject container = UIUtils.CreateView(canvas.transform, "ModList - Loaded Plugins");
            view = container.transform.parent.parent.gameObject;

            GameObject mods = new GameObject("ModListLoadedPlugins");
            mods.AddComponent<VerticalLayoutGroup>();
            mods.transform.SetParent(container.transform);
            mods.GetComponent<RectTransform>().anchoredPosition = new Vector2(360f, -250f);

            List<string> pluginNames = new List<string>();
            List<string> pluginIds;
            foreach (var info in Chainloader.PluginInfos)
            {
                string key = info.Key;
                string[] nameSpaces = key.Split('.');
                string name = nameSpaces[nameSpaces.Length - 1];

                pluginNames.Add($"<color=green>{name}</color>");
            }
            string text = pluginNames.Join(delimiter: "\n");
            logger.LogInfo($"Found a total of {pluginNames.Count} plugins!");
            pageTotal = (int)Math.Ceiling(pluginNames.Count / 13d);

            modsText = UIUtils.CreateText(mods.transform, text);
            modsText.overflowMode = TextOverflowModes.Page;

            GameObject pages = new GameObject("ModListPageSelection");
            var pagesLayout = pages.AddComponent<HorizontalLayoutGroup>();
            var pagesLayoutElement = pages.AddComponent<LayoutElement>();
            pages.transform.SetParent(container.transform);
            pagesLayout.GetComponent<RectTransform>();
            pagesLayoutElement.minHeight = 65f;

            prevButton = UIUtils.CreateButton(pages.transform, "< Prev", PrevPage);
            prevButton.interactable = false;
            pageCount = UIUtils.CreateText(pages.transform, $"{modsText.pageToDisplay}/{pageTotal}");
            pageCount.alignment = TextAlignmentOptions.Midline;
            pageCount.fontSize = 35f;
            nextButton = UIUtils.CreateButton(pages.transform, "Next >", NextPage);
            nextButton.interactable = pageTotal > 1;
        }

        private static void OnModsClick()
        {
            view?.SetActive(true);
            main?.SetActive(false);
        }

        private static void NextPage()
        {
            modsText.pageToDisplay++;
            if (modsText.pageToDisplay == pageTotal)
            {
                nextButton.interactable = false;
                prevButton.interactable = true;
            }
            else
            {
                nextButton.interactable = true;
                prevButton.interactable = true;
            }
            UpdatePageCount();
        }

        private static void PrevPage()
        {
            modsText.pageToDisplay--;
            if (modsText.pageToDisplay == 1)
            {
                prevButton.interactable = false;
                nextButton.interactable = true;
            }
            else
            {
                prevButton.interactable = true;
                nextButton.interactable = true;
            }
            UpdatePageCount();
        }

        private static void UpdatePageCount()
        {
            pageCount.text = $"{modsText.pageToDisplay}/{pageTotal}";
        }
    }
}
