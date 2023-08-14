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
            get => _url;
            set => _url = value;
        }

        private new void Start()
        {
            base.Start();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}
