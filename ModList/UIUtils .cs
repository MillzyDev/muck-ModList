using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModList
{
    static class UIUtils
    {
        public static Button CreateButton(Transform parent, string text, string template = "MultiplayerButton")
        {
            GameObject prefab = GameObject.Find(template);

            GameObject buttonObj = Object.Instantiate(prefab);
            buttonObj.name = "ModListButton";
            buttonObj.transform.SetParent(parent);

            //buttonObj.AddComponent<ButtonSfx>();

            var modListText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            modListText.text = text;

            var button = buttonObj.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();

            return button;
        }
    }
}
