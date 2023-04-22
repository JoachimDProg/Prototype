using System.Collections;
using UnityEngine;

public class Logo : MonoBehaviour
{
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private AudioClip mainMenuTheme;
    Vector3 startPointPos;
    Vector3 endPointPos;
    float themeLength = 0;

    void Start()
    {
        transform.position = startPoint.transform.position;
        startPointPos = startPoint.transform.position;
        endPointPos = endPoint.transform.position;
        themeLength = mainMenuTheme.length - 1;

        StartCoroutine(LerpLogo());
    }

    private IEnumerator LerpLogo()
    {
        float t = 0.0f;

        while (t < themeLength)
        {
            float transition = Mathf.Lerp(startPointPos.y, endPointPos.y, t / themeLength);
            transform.position = new Vector3(0, transition);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
