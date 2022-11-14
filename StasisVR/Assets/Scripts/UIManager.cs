using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject navi;
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;

    private void Awake()
    {
        canvas.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(Canvas());
    }

    private IEnumerator Canvas()
    {
        yield return new WaitForSeconds(2);
        navi.SetActive(true);
        yield return new WaitForSeconds(2);
        text1.SetActive(true);
        yield return new WaitForSeconds(8);
        text1.SetActive(false);
        text2.SetActive(true);
        yield return new WaitForSeconds(15);
        canvas.SetActive(false);
    }
}
