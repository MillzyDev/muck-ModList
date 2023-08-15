using System;
using System.Collections.Generic;
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
        public TextMeshProUGUI descriptionText = null!;
        public TextMeshProUGUI dependenciesText = null!;
        public TextMeshProUGUI errorsButton = null!;

        private void Start()
        {
            RectTransform content = GetComponent<ScrollRect>().content;

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
                    descriptionText.text = plugin.Description;
                    dependenciesText.text = dependencies.Length == 0 ? "(none)" : dependencies;
                });

                errorsButton.text = "Errors (" + Chainloader.DependencyErrors.Count + ")";
            }
        }
    }
}
