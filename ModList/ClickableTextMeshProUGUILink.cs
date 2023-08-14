using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ModList
{
    public class ClickableTextMeshProUGUILink : TextMeshProUGUI, IPointerClickHandler
    {
        private string _url = string.Empty;

        public string Url
        {
            set
            {
                if (value == _url)
                    return;
                
                var uri = new Uri(value);
                string tld = uri.Host;

                text = "Website (" + tld + ")";
                
                _url = value;
            }
        }

        protected override void Start()
        {
            base.Start();

            text = "";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(_url);
        }
    }
}
