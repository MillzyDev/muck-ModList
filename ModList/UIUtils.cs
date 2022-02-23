using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ModList
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
    }
}
