using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameover = false;

    public GameObject Player;
    public GameObject Map;
    public GameObject RestartUI;
    public GameObject SpecialCoinUI;
    public VideoPlayer EndVideo;
    public VideoPlayer EndVideo2;
    public GameObject EndVideoRaw;
    public GameObject EndVideoRaw2;
    public GameObject EndingCard;

    public GameObject EndBlackUI;
    public GameObject SpeedUPUI;


    public GameObject HanOK;

    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI SpecialTextUI;
    public int score = 0;

    public AudioSource MainAudioSource;
    public AudioSource SpecAudioSource;
    public AudioSource DieAudioSource;

    public List<Sprite> sprites = new List<Sprite>();
    public List<AudioClip> audioSources = new List<AudioClip>();

    public Image ille;

    public float TimeSpeed = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("씬에 두개 이상의 게임매니저가 있습니다.");
            Destroy(gameObject);
        }
    }


    public void Updatescore(int AddScore)
    {
        score += AddScore;
        scoreUI.text = score.ToString();
    }


    public void GameOver()
    {
        if (!isGameover)
        {
            isGameover = true;
            if(MainAudioSource.isPlaying)
            {
                MainAudioSource.Pause();
            }
            DieAudioSource.Play();
            Map.GetComponent<ScrollingMap>().isScroll = false;
            StartCoroutine(GAMEOVERUI());
        }
    }
    IEnumerator GAMEOVERUI()
    {
        yield return new WaitForSeconds(1.5f);
        RestartUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void SetChildCollidersActive(bool isActive)
    {
        // 현재 게임 오브젝트의 모든 자식 게임 오브젝트에서 BoxCollider2D 컴포넌트를 가져옴
        BoxCollider2D[] colliders = HanOK.GetComponentsInChildren<BoxCollider2D>();

        // 각 BoxCollider2D의 활성화 상태를 변경
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = isActive;
        }
    }


    public void GameEnding()
    {
        Map.GetComponent<ScrollingMap>().isScroll = false;
        MainAudioSource.Stop();

        for (int i = 0; i < 9; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 1);
        }
        PlayerPrefs.SetInt("GameClear", 1);
        PlayerPrefs.Save();

        EndVideo.gameObject.SetActive(true);
        EndVideoRaw.gameObject.SetActive(true);
        EndVideo.loopPointReached += OnVideoEnd;
        EndVideo.Play();
    }
    void OnVideoEnd(VideoPlayer vp)
    {
        EndVideo.gameObject.SetActive(false);
        EndVideoRaw.gameObject.SetActive(false);
        EndingCard.SetActive(true);
    }
    public void OnClickEnding()
    {
        EndVideo2.gameObject.SetActive(true);
        EndVideoRaw2.gameObject.SetActive(true);
        EndVideo2.loopPointReached += OnVideoEnd2;
        EndVideo2.Play();
    }
    void OnVideoEnd2(VideoPlayer vp)
    {
        // 콜백 함수 해제
        EndVideo.loopPointReached -= OnVideoEnd;

        // 다음 씬으로 이동
        SceneManager.LoadScene(0);
    }

    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs have been reset.");
    }

    public void SpeedUP()
    {
        SpeedUPUI.SetActive(true);
        TimeSpeed += 0.1f;
        if(TimeSpeed >= 1.3f)
        {

        }
        Time.timeScale = TimeSpeed;
        StartCoroutine(IESpeedUPUI());
    }

    IEnumerator IESpeedUPUI()
    {
        yield return new WaitForSeconds(1);
        SpeedUPUI.SetActive(false);
    }
    // 특정 AudioClip을 재생하는 메서드
    public void PlaySpecialBGM(int index)
    {
        if (index >= 0 && index < audioSources.Count)
        {
            // Main BGM을 일시정지
            MainAudioSource.Pause();
            // 스페셜 BGM 재생
            SpecAudioSource.clip = audioSources[index];
            SpecAudioSource.volume = 1;
            switch (index)
            {
                case 5:
                    SpecAudioSource.volume = 0.8f; break;
                case 7:
                    SpecAudioSource.volume = 0.7f; break;
            }
            SpecAudioSource.Play();
        }
        else
        {
            Debug.LogError("Invalid index for special BGM.");
        }
    }
    public void ResizeImage()
    {
        // Sprite의 실제 크기를 가져옵니다.
        Sprite sprite = ille.sprite;
        if (sprite == null)
            return;

        Rect spriteRect = sprite.rect;
        Vector2 spriteSize = new Vector2(spriteRect.width, spriteRect.height);

        // RectTransform의 크기를 조정합니다.
        RectTransform rectTransform = ille.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 이미지의 비율을 유지하면서 크기를 조정합니다.
            float imageRatio = spriteSize.x / spriteSize.y;
            float parentHeight = rectTransform.rect.height;

            // 부모의 높이에 맞추어 너비를 조정합니다.
            float newWidth = parentHeight * imageRatio;
            rectTransform.sizeDelta = new Vector2(newWidth, parentHeight);
        }
    }
    // Main BGM을 다시 재생하는 메서드
    public void ResumeMainBGM()
    {
        // Main BGM 재생
        MainAudioSource.UnPause();
        // 스페셜 BGM 정지
        SpecAudioSource.Stop();
    }
public void GetSpecialCoin(int coinname, string CoinText)
    {
        //Map.GetComponent<ScrollingMap>().isScroll = false;
        Time.timeScale = 0;
        SpecialCoinUI.SetActive(true);
        SpecialTextUI.text = CoinText;
        ille.sprite = sprites[coinname];
        ResizeImage();
        PlayerPrefs.SetInt(coinname.ToString(), 1);
        PlaySpecialBGM(coinname);
    }
    public void OnClick_Close_UI_Special()
    {
        SpecialCoinUI.GetComponent<Animator>().Play("UI_UnScrillCoin");
        StartCoroutine(ReStart());
    }
    IEnumerator ReStart()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        SpecialCoinUI.SetActive(false);
        ResumeMainBGM();
        Time.timeScale = TimeSpeed;
    }
    public void OnClickLoby()
    {

        SceneManager.LoadScene(0);
    }

    public void OnClickRestart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Init()
    {
        Time.timeScale = TimeSpeed;
        if (Player == null)
        {
            Player = GameObject.Find("Player");
        }
        if (Map == null)
        {
            Map = GameObject.Find("Map");
        }
        if (RestartUI == null)
        {
            RestartUI = GameObject.Find("UI_GameOver");
        }
        scoreUI.text = "0";
        SpecAudioSource.Stop();


    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
        {
            ResetPlayerPrefs();
        }
        
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
        {
            ResetPlayerPrefs();
        }
    }
}
