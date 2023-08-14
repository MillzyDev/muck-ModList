using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModList
{
    [RequireComponent(typeof(ScrollRect))]
    public class PluginScrollView : MonoBehaviour
    {
        public GameObject buttonPrefab;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI versionText;
        public ClickableTextMeshProUGUILink websiteText;
        public TextMeshProUGUI dependenciesText;
    }
}
