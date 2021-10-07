using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private AudioClip mainMenuTheme;
    Vector3 startPointPos;
    Vector3 endPointPos;
    float time = 0;
    float themeLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint.transform.position;
        startPointPos = startPoint.transform.position;
        endPointPos = endPoint.transform.position;
        themeLength = mainMenuTheme.length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < themeLength)
        {
            float t = time / themeLength;

            float transition = Mathf.Lerp(startPointPos.y, endPointPos.y, t);

            time += Time.deltaTime;

            transform.position = new Vector3(0, transition);
        }
        else
        {
            transform.position = new Vector3(0, endPointPos.y);
        }
    }
}
