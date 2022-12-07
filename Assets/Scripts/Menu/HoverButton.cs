using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.2f,1.2f, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1f,1f, 1f);
        }
    }
}
