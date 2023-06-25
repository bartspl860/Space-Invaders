using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaking : MonoBehaviour
{
    [SerializeField]
    private float _duration;
    [SerializeField]
    private AnimationCurve _curve;

    public void startShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float strenght = _curve.Evaluate(elapsedTime / _duration);
            transform.position = startPosition + Random.insideUnitSphere * strenght;
            yield return null;
        }

        transform.position = startPosition;
    }
}
