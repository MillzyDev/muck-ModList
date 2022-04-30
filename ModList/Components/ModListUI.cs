using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModList.Components
{
    public class ModListUI : MonoBehaviour
    {
        public GameObject ModListView
        {
            get;
            private set;
        }

        public Button MainBackButton
        {
            get;
            private set;
        }

        public Button ErrorsButton
        {
            get;
            private set;
        }

        public Button ErrorsBackButton
        {
            get;
            private set;
        }

        public TextMeshProUGUI Title
        {
            get;
            private set;
        }

        public GameObject PluginListContent
        {
            get;
            private set;
        }

        public GameObject PluginButtonPrefab
        {
            get;
            private set;
        }

        public TextMeshProUGUI PluginName
        {
            get;
            private set;
        }

        public TextMeshProUGUI PluginVersion
        {
            get;
            private set;
        }

        public RawImage WebIcon
        {
            get;
            private set;
        }

        public TextMeshProUGUI WebLink
        {
            get;
            private set;
        }

        public TextMeshProUGUI Description
        {
            get;
            private set;
        }

        public TextMeshProUGUI Dependencies
        {
            get;
            private set;
        }

        public GameObject ErrorsView
        {
            get;
            private set;
        }

        public GameObject ErrorListContent
        {
            get;
            private set;
        }

        public GameObject ErrorPrefab
        {
            get;
            private set;
        }

#nullable enable
        public void OutputIfNull(object? o, string name)
        {
            if (o == null) Plugin.GetLogger().LogInfo($"{name} is null.");
        }
#nullable disable

        private void Start()
        {
            ModListView = transform.Find("ModListView").gameObject;

            MainBackButton = ModListView.transform.Find("BackButton").GetComponent<Button>();
            MainBackButton.gameObject.AddComponent<ButtonSfx>();
            ErrorsButton = ModListView.transform.Find("ErrorsButton").GetComponent<Button>();
            ErrorsButton.gameObject.AddComponent<ButtonSfx>();

            Transform viewBackground = ModListView.transform.Find("ModListViewBackground");

            Title = viewBackground.Find("ModListTitle").GetComponent<TextMeshProUGUI>();

            Transform pluginList = viewBackground.Find("PluginList");
            pluginList.GetComponent<ScrollRect>().scrollSensitivity = 30f;

            Transform pluginViewport = pluginList.Find("Viewport");

            PluginListContent = pluginViewport.Find("Content").gameObject;

            PluginButtonPrefab = PluginListContent.transform.Find("PluginButton").gameObject;
            PluginButtonPrefab.AddComponent<ButtonSfx>();
            PluginButtonPrefab.SetActive(false);

            Transform infoContainer = viewBackground.Find("ModInfoContainer");

            Transform nameVersionContainer = infoContainer.Find("NameVersionContainer");
            PluginName = nameVersionContainer.Find("PluginName").GetComponent<TextMeshProUGUI>();
            PluginVersion = nameVersionContainer.Find("PluginVersion").GetComponent<TextMeshProUGUI>();

            Transform webContainer = infoContainer.Find("WebsiteContainer");
            WebIcon = webContainer.Find("WebsiteIcon").GetComponent<RawImage>();
            WebLink = webContainer.Find("WebsiteLink").GetComponent<TextMeshProUGUI>();
            WebLink.gameObject.AddComponent<Hyperlink>();

            Transform descContainer = infoContainer.Find("DescriptionContainer");
            Description = descContainer.Find("Description").GetComponent<TextMeshProUGUI>();

            Transform depsContainer = infoContainer.Find("DependencyContainer");
            Dependencies = depsContainer.Find("Dependencies").GetComponent<TextMeshProUGUI>();

            ModListView.SetActive(false);

            ErrorsView = transform.Find("ErrorView").gameObject;

            Transform errorList = ErrorsView.transform.Find("ErrorList");
            errorList.GetComponent<ScrollRect>().scrollSensitivity = 30f;

            Transform errorViewport = errorList.Find("Viewport");

            ErrorListContent = errorViewport.Find("Content").gameObject;
            ErrorPrefab = ErrorListContent.transform.Find("Error").gameObject;
            ErrorPrefab.SetActive(false);

            ErrorsBackButton = ErrorsView.transform.Find("BackButton").GetComponent<Button>();
            ErrorsBackButton.gameObject.AddComponent<ButtonSfx>();

            ErrorsView.SetActive(false);

            Texture2D texture = new(256, 256);
            Stream sourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ModList.web-icon.jpg");
            MemoryStream memoryStream = new();
            sourceStream.CopyTo(memoryStream);
            texture.LoadImage(memoryStream.ToArray());
            WebIcon.texture = texture;

            PluginName.text = "???";
            PluginVersion.text = "???";
            WebLink.text = "???";
            Description.text = "???";
            Dependencies.text = "???";

            AddPlugins(PluginManager.Instance.pluginData);
            AddErrors(PluginManager.Instance.errors);
        }

        public void AddPlugins(List<Tuple<string, PluginManifest>> plugins)
        {
            foreach (var plugin in plugins)
            {
                Plugin.GetLogger().LogInfo($"Found plugin {plugin.Item1}");
                AddPlugin(plugin);
            }
        }

        public void AddPlugin(Tuple<string, PluginManifest> plugin)
        {
            string name = plugin.Item1;
            string version = null;
            string website = null;
            string description = null;
            string dependencies = null;

            if (plugin.Item2 != null)
            {
                name = string.IsNullOrEmpty(plugin.Item2.Name) ? plugin.Item1 : plugin.Item2.Name;
                version = string.IsNullOrEmpty(plugin.Item2.Version) ? "???" : plugin.Item2.Version;
                website = plugin.Item2.Website;
                description = string.IsNullOrEmpty(plugin.Item2.Description) ? "???" : plugin.Item2.Description;
                var deps = string.Join(", ", plugin.Item2.Dependencies);
                dependencies = string.IsNullOrEmpty(deps) ? "(None)" : deps;
            }

            GameObject button = Instantiate(PluginButtonPrefab);
            button.transform.SetParent(PluginListContent.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = name;

            var link = WebLink.GetComponent<Hyperlink>();

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                PluginName.text = name;
                PluginVersion.text = version == null ? "???" : version;
                Description.text = description == null ? "???" : description;
                Dependencies.text = dependencies == null ? "???" : dependencies;
                link.URL = website;
            });
            button.SetActive(true);
        }

        public void AddErrors(List<string> errors)
        {
            foreach (var error in errors)
                AddError(error);
        }

        public void AddError(string error)
        {
            GameObject message = Instantiate(ErrorPrefab);
            message.transform.SetParent(ErrorListContent.transform);
            message.GetComponentInChildren<TextMeshProUGUI>().text = error;
            message.SetActive(true);

            ErrorsButton.GetComponentInChildren<TextMeshProUGUI>().text = $"CL Errors ({ErrorListContent.transform.childCount - 1})";
        }
    }
}