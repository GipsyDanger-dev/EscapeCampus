using UnityEngine;
using UnityEngine.UI;

namespace EscapeCampus.UI
{
    public class CrosshairUI : MonoBehaviour
    {
        [SerializeField] private Image crosshairImage;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color interactColor = Color.green;

        private void Start()
        {
            if (crosshairImage != null)
            {
                crosshairImage.color = defaultColor;
            }
        }

        public void SetInteractState(bool canInteract)
        {
            if (crosshairImage != null)
            {
                crosshairImage.color = canInteract ? interactColor : defaultColor;
            }
        }
    }
}
