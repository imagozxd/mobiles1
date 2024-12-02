using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Transform titleStartPosition;
    [SerializeField] private Transform titleEndPosition;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float scaleIncrease = 1.2f;

    [SerializeField] private Ease easeValue;

    [SerializeField] private Button actionButton;
    [SerializeField] private float dropDuration = 2f;
    [SerializeField] private float bounceStrength = 0.5f;

    private void Start()
    {
        AnimateTitle();
    }

    private void AnimateTitle()
    {
        titleText.transform.position = titleStartPosition.position;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(titleText.transform.DOMove(titleEndPosition.position, moveDuration).SetEase(Ease.OutBounce));
        sequence.Append(titleText.transform.DOScale(scaleIncrease, 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(titleText.transform.DOScale(1f, 0.2f).SetEase(Ease.InQuad));

        sequence.Play();
    }

    public void AnimateButtonAndLoadScene()
    {
        Sequence buttonSequence = DOTween.Sequence();
        Vector3 originalPosition = actionButton.transform.position;

        buttonSequence.Append(actionButton.transform.DOMoveY(originalPosition.y - 500f,2).SetEase(easeValue));
        //buttonSequence.Append(actionButton.transform.DOMoveY(originalPosition.y, dropDuration / 2).SetEase(Ease.OutBounce));
        buttonSequence.OnComplete(() => SceneManager.LoadScene("mejora"));
    }
}
