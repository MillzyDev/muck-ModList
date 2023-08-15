using System;
using System.Collections.Generic;
using BepInEx.Bootstrap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModList
{
    public class ErrorScrollView : MonoBehaviour
    {
        public GameObject errorPrefab = null!;
        public TextMeshProUGUI modsButton = null!;

        private void Start()
        {
            RectTransform content = GetComponent<ScrollRect>().content;
            
            List<string> errors = Chainloader.DependencyErrors;

            if (errors.Count != 0)
            {
                foreach (string error in errors)
                {
                    GameObject errorDisplay = Instantiate(errorPrefab, content);
                    var errorText = errorDisplay.GetComponentInChildren<TextMeshProUGUI>();
                    errorText.text = error;
                }
            } else
            {
                GameObject errorDisplay = Instantiate(errorPrefab, content);
                var errorText = errorDisplay.GetComponentInChildren<TextMeshProUGUI>();
                errorText.color = Color.white;
                errorText.text = "There are no errors... why are you here?";
            }

            modsButton.text = "Mods (" + Chainloader.PluginInfos.Count + ")";
        }
    }
}
