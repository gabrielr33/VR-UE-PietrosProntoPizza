using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class FloatingReviewText : MonoBehaviour
    {
        [SerializeField] private Color _goodReviewColor;
        [SerializeField] private Color _mediumReviewColor;
        [SerializeField] private Color _badReviewColor;

        private Transform _cameraTransform;
        private TMP_Text _reviewText;
        private Vector3 _startPos;
        private Vector3 _endPos;

        private void Awake()
        {
            _startPos = transform.localPosition;
            _endPos = new Vector3(_startPos.x, _startPos.y + 120f, _startPos.z);
            Destroy(gameObject, 7f);
        }

        private void Update()
        {
            transform.LookAt(_cameraTransform.position);
            transform.Rotate(0f, 180f, 0f);

            transform.localPosition = Vector3.Lerp(transform.localPosition, _endPos, Time.deltaTime * 0.3f);
        }

        public void SetUp(decimal review)
        {
            _cameraTransform = Camera.main.transform;
            _reviewText = GetComponent<TMP_Text>();
            _reviewText.text = $"{review}";
            SetColorAccordingToReview(review);
        }

        private void SetColorAccordingToReview(decimal review)
        {
            _reviewText.color = (float)review switch
            {
                > 3.5f and <= 5 => _goodReviewColor,
                > 2 and <= 3.5f => _mediumReviewColor,
                >= 0 and <= 2 => _badReviewColor,
                _ => Color.black
            };
        }
    }
}