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
        public ClickableTextMeshProUGUILink websiteText = null!;
        public TextMeshProUGUI dependenciesText = null!;

        private void Start()
        {
            Transform content = transform.Find("Viewport/Content");

            IEnumerable<ModListPlugin> plugins = PluginInfoFinder.GetInfo();

            foreach (ModListPlugin plugin in plugins)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, content);
                var buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                string pluginName = plugin.Name;

                buttonText.text = pluginName;

                var button = buttonObj.GetComponent<Button>();

                string websiteUrl = plugin.WebsiteUrl;
                string dependencies = plugin.Dependencies;
                
                button.onClick.AddListener(() =>
                {
                    nameText.text = pluginName;
                    versionText.text = "v" + plugin.Version;
                    websiteText.text = websiteUrl.Length == 0
                        ? "(none)"
                        : "Website (" + new Uri(websiteUrl).Host + ")";
                    websiteText.Url = websiteUrl;
                    
                    dependenciesText.text = dependencies.Length == 0 ? "(none)" : dependencies;
                });
            }
        }
    }
}
