using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    public float waitTime = 13.5f;

    private void Awake()
    {
          StartCoroutine(WaitToFinish());
    }
    IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
