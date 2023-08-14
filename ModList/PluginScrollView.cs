using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Bootstrap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModList
{
    public class PluginScrollView : MonoBehaviour
    {
        public GameObject buttonPrefab = null!;
        public TextMeshProUGUI nameText = null!;
        public TextMeshProUGUI versionText = null!;
        public ClickableTextMeshProUGUILink websiteText;
        public TextMeshProUGUI dependenciesText;

        private void Start()
        {
            Transform content = transform.Find("Viewport/Content");
            
            Dictionary<string, PluginInfo>? plugins = Chainloader.PluginInfos;

            foreach (KeyValuePair<string, PluginInfo> plugin in plugins)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, content);
                var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                string pluginName = plugin.Value.Metadata.Name;

                buttonText.text = plugin.Value.Metadata.Name;

                var button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    nameText.text = pluginName;
                    versionText.text = "v" + plugin.Value.Metadata.Version;
                });
            }
        }
    }
}
