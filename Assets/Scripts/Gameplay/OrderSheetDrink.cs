using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class OrderSheetDrink : MonoBehaviour
    {
        [SerializeField] private TMP_Text _orderText;
        [SerializeField] private Slider _maxWaitTimeSlider;
        [SerializeField] private GameObject _failedOverlayImage;
        
        public void SetUp(Order order)
        {
            if (order == null)
                return;
            
            _failedOverlayImage.SetActive(false);
            _orderText.text = $"No. {order.TableNumber}\n{order.CustomerName}\n{order.Drink.drinkName}";
            _maxWaitTimeSlider.minValue = 0;
            _maxWaitTimeSlider.maxValue = order.MaxWaitTimeInSec;
            _maxWaitTimeSlider.value = order.MaxWaitTimeInSec;

            StartCoroutine(CountDownSlider(order.MaxWaitTimeInSec));
        }

        private IEnumerator CountDownSlider(int seconds)
        {
            int counter = seconds;
            while (counter > 0) {
                yield return new WaitForSeconds(1);
                _maxWaitTimeSlider.value = --counter;
            }
            
            // TODO
            Debug.Log("Order expired!");
            _failedOverlayImage.SetActive(true);
        }
    }
}
