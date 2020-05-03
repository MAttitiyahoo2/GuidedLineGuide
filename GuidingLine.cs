using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuidingLine : MonoBehaviour
{
    public List<Transform> Positions;
    public float GuideSpeed;

    private LineRenderer lineRenderer;
    private int direction = 1;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();


        lineRenderer.positionCount = Positions.Count;
        lineRenderer.SetPositions(Positions.Select(x => x.transform.position).ToArray());

        StartCoroutine(GuideLine());
    }

    private IEnumerator GuideLine()
    {
        var colorGradient = lineRenderer.colorGradient;
        var alphaKeys = colorGradient.alphaKeys;

        for (int i = 0; i < alphaKeys.Length; i++)
        {
            var alphaKey = alphaKeys[i];

            while (Mathf.Abs(alphaKey.alpha) < 1)
            {
                var next = GuideSpeed * direction;
                alphaKey.alpha = direction == 1 ? Mathf.Min(1, alphaKey.alpha + next) : Mathf.Max(-1, alphaKey.alpha + next);
                alphaKeys[i] = alphaKey;
                colorGradient.alphaKeys = alphaKeys;
                lineRenderer.colorGradient = colorGradient;

                yield return null;
            }
        }

        for (int i = 0; i < alphaKeys.Length; i++)
        {
            alphaKeys[i].alpha = 0.999f * direction;
        }

        colorGradient.alphaKeys = alphaKeys;
        lineRenderer.colorGradient = colorGradient;
        direction *= -1;
        StartCoroutine(GuideLine());
    }
}
