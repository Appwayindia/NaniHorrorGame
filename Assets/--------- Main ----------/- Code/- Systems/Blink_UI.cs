using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Henmova
{
    public class Blink_UI : MonoBehaviour
    {
        [SerializeField] private Image _target;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;


        void Start()
        {
            _target.DOFade(0f, _duration).From(1f).SetLoops(-1, LoopType.Yoyo).SetEase(_ease);
        }
    }
}
