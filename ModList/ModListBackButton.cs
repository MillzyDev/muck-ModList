using UnityEngine;
using UnityEngine.UI;

namespace ModList
{
    public class ModListBackButton : MonoBehaviour
    {
        public Button[] backButtons = null!;

        private void Start()
        {
            var menuUi = GetComponentInParent<MenuUI>();
            GameObject mainUi = menuUi.mainUi;
            
            foreach (Button button in backButtons)
            {
                button.onClick = new Button.ButtonClickedEvent();
                button.onClick.AddListener(() =>
                {
                    gameObject.SetActive(false);
                    mainUi.SetActive(true);
                });
            }
        }
    }
}
