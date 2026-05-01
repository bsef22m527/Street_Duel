using UnityEngine;
using DG.Tweening;

public class PlantSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float rotationAmount = 5f;   // how far it bends
    public float duration = 2.5f;       // speed of sway
    public float randomness = 0.5f;     // variation per plant

    void Start()
    {
        float randomRot = rotationAmount + Random.Range(-randomness, randomness);
        float randomDur = duration + Random.Range(-randomness, randomness);

        transform
            .DORotate(new Vector3(0, 0, randomRot), randomDur)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

            Vector3 startPos = transform.localPosition;

transform
    .DOLocalMoveX(startPos.x + 0.05f, duration)
    .SetEase(Ease.InOutSine)
    .SetLoops(-1, LoopType.Yoyo);
    }
}