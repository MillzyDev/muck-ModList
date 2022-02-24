using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModList.UI
{
    static class UIUtils
    {
        public static Button CreateButton(Transform parent, string text, UnityAction callback, string template = "MultiplayerButton")
        {
            GameObject prefab = GameObject.Find(template);

            GameObject buttonObj = UnityEngine.Object.Instantiate(prefab);
            buttonObj.name = "ModListButton";
            buttonObj.transform.SetParent(parent);

            var modListText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            modListText.text = text;

            var button = buttonObj.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(callback);

            return button;
        }

        public static GameObject CreateCanvas(RenderMode renderMode)
        {
            Plugin.logger.LogInfo("Creating canvas");
            GameObject canvas = new GameObject("ModListCanvas", typeof(RectTransform));
            Canvas canv = canvas.AddComponent<Canvas>();
            canv.renderMode = renderMode;
            canvas.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        public static GameObject CreateView(Transform parent, string title)
        {
            // get the credits thing, cuz its easier
            Plugin.logger.LogInfo("Instantiating credits");
            GameObject view = Object.Instantiate(FindInactiveObject("Credits"));
            view.transform.SetParent(parent);
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
            text.fontSize = 45f;
            text.fontStyle |= FontStyles.Underline;
            text.text = title;

            var layout = thingyWeUse.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(30, 30, 30, 30);

            Plugin.logger.LogInfo("Moving back button");
            GameObject backButton = view.transform.Find("MenuButton").gameObject;
            backButton.transform.localPosition = new Vector3(-375f, 350f, 0f);

            return thingyWeUse.gameObject;
        }

        public static TextMeshProUGUI CreateText(Transform parent, string text)
        {
            TextMeshProUGUI t = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>()[0];
            GameObject obj = Object.Instantiate(t.gameObject);
            obj.transform.SetParent(parent);

            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 20f;
            tmp.color = Color.white;
            tmp.richText = true;

            return tmp;
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
                Object.DestroyImmediate(component as UnityEngine.Object, true);
            }
        }
    }
}
