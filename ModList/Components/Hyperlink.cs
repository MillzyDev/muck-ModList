using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModList.Components
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    class Hyperlink : MonoBehaviour, IPointerClickHandler
    {
        TextMeshProUGUI text;

        private string url = string.Empty;
        public string URL
        {
            get => url;
            set
            {
                url = value == null ? "https://muck.thunderstore.io/" : value;

                try
                {
                    text.text = $"Website ({new Uri(url).Host})";
                }
                catch
                {
                    text.text = $"Website (???)";
                }
            }
        }

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(URL);
        }
    }
}
