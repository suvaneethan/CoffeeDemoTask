using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI scoreText;
    public RectTransform coinPrefab;
    public RectTransform canvasRect;
    public RectTransform scoreIcon;

    [Header("Coin fly settings")]
    public float coinDuration = 0.85f;
    public AnimationCurve coinEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float arcHeight = 40f;
    public float popScale = 1.25f;
    public float popDuration = 0.12f;

    Coroutine coinRoutine;

    public void UpdateScore(int value)
    {
        if (scoreText != null) scoreText.text = value.ToString();
    }

    public void SpawnCoinFly(Vector3 worldPos)
    {
        if (coinPrefab == null || canvasRect == null || scoreIcon == null) return;
        if (coinRoutine != null) StopCoroutine(coinRoutine);
        coinRoutine = StartCoroutine(DoCoinFly(worldPos));
    }

    IEnumerator DoCoinFly(Vector3 worldPos)
    {
        Vector2 startScreen = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        RectTransform coin = Instantiate(coinPrefab, canvasRect);
        coin.position = startScreen;
        coin.localScale = Vector3.one;

        Vector3 start = coin.position;
        Vector3 target = scoreIcon.position;
        float t = 0f;

        while (t < coinDuration)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / coinDuration);
            float eased = coinEase.Evaluate(u);
            float arc = Mathf.Sin(u * Mathf.PI) * arcHeight;
            coin.position = Vector3.Lerp(start, target, eased) + Vector3.up * arc;
            yield return null;
        }

        // pop animation
        float pt = 0f;
        while (pt < popDuration)
        {
            pt += Time.deltaTime;
            float u = Mathf.Clamp01(pt / popDuration);
            float s = Mathf.Lerp(1f, popScale, Mathf.Sin(u * Mathf.PI));
            coin.localScale = Vector3.one * s;
            yield return null;
        }

        Destroy(coin.gameObject);
        coinRoutine = null;
    }
}
