using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public bool isLoading { get; private set; } = false;

    public Animator transition;

    public static LevelManager Instance
    {
        get;
        private set;
    }

    [field: SerializeField]
    public string Id
    {
        get;
        private set;
    }

    private bool _lowerMusic;

    private void Awake()
    {
        this.Id = Guid.NewGuid().ToString();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        transition.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_lowerMusic)
        {
            LowerMusic();
        }
    }

    private void LowerMusic()
    {
        //SoundManager.Instance.music.volume = Mathf.Lerp(SoundManager.Instance.music.volume, 0, 0.05f);
    }

    public void NextLevel(int TargetScene)
    {
        StartCoroutine(LoadLevel(TargetScene));
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        isLoading = true;

        transition.SetTrigger("Start");

        _lowerMusic = true;

        yield return new WaitForSeconds(.7f);

        _lowerMusic = false;

        SceneManager.LoadScene(sceneIndex);

        isLoading = false;
    }
}

