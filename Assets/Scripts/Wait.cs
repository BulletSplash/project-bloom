using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Wait : MonoBehaviour
{
    GameObject video;
    VideoPlayer player;

    private void Awake()
    {
        video = GameObject.Find("Intro");
        player = video.GetComponent<VideoPlayer>();
    }
    private void Start()
    {
            StartCoroutine(WaitToFinish());       
    }

    IEnumerator WaitToFinish()
    {
        player.Play();
        yield return new WaitForSeconds(13f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    } 
}
