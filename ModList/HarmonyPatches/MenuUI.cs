using BepInEx.Bootstrap;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModList.HarmonyPatches
{
    [HarmonyPatch(typeof(global::MenuUI))]
    [HarmonyPatch("Start")]
    class MenuUI
    {
        static GameObject view;
        static GameObject main;

        private static void Postfix(global::MenuUI __instance)
        {
            Plugin.logger.LogInfo("Getting loaded mods");
            // creating our text
            string mods = "\n     <size=42><u>Loaded Mods</u></size>\n";

            var pluginInfos = Chainloader.PluginInfos;
            foreach (var pluginInfo in pluginInfos)
            {
                mods += $"     {pluginInfo.Key}\n";
            }

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
            GameObject canvas = new GameObject("ModListCanvas", typeof(RectTransform));
            Canvas canv = canvas.AddComponent<Canvas>();
            canv.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.AddComponent<GraphicRaycaster>();

            // get the credits thing, cuz its easier
            Plugin.logger.LogInfo("Instantiating credits");
            view = Object.Instantiate(FindInactiveObject("Credits"));
            view.transform.SetParent(canvas.transform);
            RemoveComponent<Credits>(view); // remove this; we dont need it
            //view.SetActive(true);
            view.transform.localPosition = new Vector3(0f, 0f, 0f); // fix position

            // disable cringe stuff we dont need
            Plugin.logger.LogInfo("Removing credits");
            Transform credits = view.transform.Find("GameplaySettings/RawImage (1)");
            credits.gameObject.SetActive(false);

            Plugin.logger.LogInfo("Removing buttons");
            Transform thingyWeUse = view.transform.Find("GameplaySettings/RawImage");
            Transform button2 = thingyWeUse.Find("Button (2)");
            button2.gameObject.SetActive(false);
            Transform button3 = thingyWeUse.Find("Button (3)");
            button3.gameObject.SetActive(false);

            Plugin.logger.LogInfo("Setting text");
            TextMeshProUGUI text = thingyWeUse.GetComponentInChildren<TextMeshProUGUI>();
            text.alignment = TextAlignmentOptions.TopLeft;
            text.fontSize = 25f;
            text.text = mods;

            // fix button pos
            Plugin.logger.LogInfo("Moving back button");
            GameObject backButton = view.transform.Find("MenuButton").gameObject;
            backButton.transform.localPosition = new Vector3(-375f, 350f, 0f);
        }

        private static void OnModsClick()
        {
            view?.SetActive(true);
            main?.SetActive(false);
        }

        private static GameObject FindInactiveObject(string name)
        {
            Transform[] trs = Resources.FindObjectsOfTypeAll<Transform>();
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }
        private static void RemoveComponent<Component>(GameObject go)
        {
            Component component = go.GetComponent<Component>();

            if (component != null)
            {
                Object.DestroyImmediate(component as Object, true);

            }
        }

    }
}
