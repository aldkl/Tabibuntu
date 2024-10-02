using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{

    public Image Illustration;
    public Image Real_Illustration;
    public Sprite Illur7_2;
    public string Illur7_2text;
    public Image IllustrationShowCheck;
    public Image TabiWikiShowCheck;
    public TextMeshProUGUI IllustrationText;
    public GameObject IllurBtn;
    public GameObject TabiWikiBtn;
    public GameObject GameStartBtn;

    public GameObject TabiWiki;

    public GameObject HowGameImage;

    public List<Sprite> sprites = new List<Sprite>();
    public List<string> IllustText = new List<string>();
    private int Illu = 0;

    public VideoPlayer OpeningVideo;
    public GameObject OpenVideoRaw;

    public AudioSource MainAudio;

    public List<Image> images;  // UI 이미지 리스트
    public float scrollSensitivity = 0.1f;  // 스크롤 감도 조절 변수
    private int currentIndex = 0;  // 현재 표시 중인 이미지 인덱스
    private float scrollDelta = 0; // 스크롤 변화량 누적

    private bool OnTabiWiki = false;
    public void OnclickLeftIll()
    {
        if (Illu > 0)//만약 일러가 1이상이면
        {
            Illu--;//마이너스 해주고
            Real_Illustration.color = Color.white;
            Real_Illustration.sprite = sprites[Illu];//일러 변경해주기
            IllustrationText.text = IllustText[Illu];
            int coinStatus = PlayerPrefs.GetInt(Illu.ToString(), 0);
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "아직 찾지 못했습니다!";
            }
            ResizeImage();
        }
    }

    public void OnclickRightIll()
    {
        if (Illu < sprites.Count - 1)//만약 일러가 Count보다 작으면
        {
            Illu++;//마이너스 해주고
            Real_Illustration.color = Color.white;
            Real_Illustration.sprite = sprites[Illu];//일러 변경해주기
            IllustrationText.text = IllustText[Illu];
            int coinStatus = PlayerPrefs.GetInt(Illu.ToString(), 0);
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "아직 찾지 못했습니다!";
            }
            ResizeImage();
        }
    }

    public void ResizeImage()
    {
        // Sprite의 실제 크기를 가져옵니다.
        Sprite sprite = Real_Illustration.sprite;
        if (sprite == null)
            return;

        Rect spriteRect = sprite.rect;
        Vector2 spriteSize = new Vector2(spriteRect.width, spriteRect.height);

        // RectTransform의 크기를 조정합니다.
        RectTransform rectTransform = Real_Illustration.GetComponent<RectTransform>();
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

    public void OnClickShutDown()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }



    public void OnClickOpenTabiWiki()
    {
        OnTabiWiki = true;
        TabiWiki.SetActive(true);
        MainAudio.Pause();
    }
    public void OnClickIllustrationOpen()
    {
        MainAudio.Pause();
        Illustration.gameObject.SetActive(true);
    }
    public void OnClickOpenHowGame()
    {
        HowGameImage.SetActive(true);
    }
    public void OnClickGameStart()
    {
        SceneManager.LoadScene(1);
    }
    public void IllurBtnClose()
    {
        MainAudio.UnPause();
        Illustration.gameObject.SetActive(false);
    }

    void ScrollImagesBy(float scroll)
    {
        scrollDelta += scroll * scrollSensitivity;

        if (Mathf.Abs(scrollDelta) >= 1.0f)
        {
            int direction = scrollDelta > 0 ? -1 : 1;  // 스크롤 방향

            currentIndex = Mathf.Clamp(currentIndex + direction, 0, images.Count);  // 인덱스 범위 조절
            if (currentIndex == images.Count)
            {
                MainAudio.UnPause();
                OnTabiWiki = false;
                TabiWiki.SetActive(false);
                currentIndex = 1;
                // 모든 이미지를 비활성화하고 현재 인덱스의 이미지만 활성화
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].gameObject.SetActive(i == currentIndex);
                }
                return;
            }
            // 모든 이미지를 비활성화하고 현재 인덱스의 이미지만 활성화
            for (int i = 0; i < images.Count; i++)
            {
                images[i].gameObject.SetActive(i == currentIndex);
            }
            scrollDelta = 0; // 스크롤 변화량 초기화
        }
    }//뿡무위키스크롤


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        SetResolution();
        //IllurBtnOPen();
        //TabiWikiBtnOPen();

        int IsOPVideo = PlayerPrefs.GetInt("IsOPVideo", 0);
        if (IsOPVideo == 0)
        {
            PlayerPrefs.SetInt("IsOPVideo", 1);

            OpeningVideo.gameObject.SetActive(true);
            OpenVideoRaw.gameObject.SetActive(true);
            OpeningVideo.loopPointReached += OnVideoEnd;
            OpeningVideo.Play();
        }
        else
        {
            OpeningVideo.gameObject.SetActive(false);
            OpenVideoRaw.gameObject.SetActive(false);
            MainAudio.Play();

            StartCoroutine(PlayAnimations());

        }
    }

    float GetAnimationClipLength(Animator animator, string clipName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0f;
    }
    IEnumerator PlayAnimations()
    {

        int coinStatus = 0;
        for (int i = 0; i < 9; i++)
        {
            coinStatus += PlayerPrefs.GetInt(i.ToString(), 0);
        }
        int checkAni = PlayerPrefs.GetInt("AniIllur", 0);
        // 코인이 하나라도 먹힌 상태(1)라면 일러스트버튼 활성화
        if (coinStatus >= 1)
        {
            if (checkAni == 0)
            {
                Debug.Log("checkAni = " + checkAni);
                PlayerPrefs.SetInt("AniIllur", 1);
                IllustrationShowCheck.gameObject.SetActive(true);
                // 첫 번째 이미지 애니메이션 재생
                yield return new WaitForSeconds(GetAnimationClipLength(IllustrationShowCheck.GetComponent<Animator>(), "UI_OpenIller"));
                // 첫 번째 버튼 활성화
                Debug.Log("checkAni = " + IllustrationShowCheck.gameObject.activeSelf);
            }
            IllurBtn.SetActive(true);
        }
        else
        {
            IllurBtn.SetActive(false);
        }

        coinStatus = PlayerPrefs.GetInt("GameClear", 0);

        checkAni = PlayerPrefs.GetInt("AniWiki", 0);
        // 게임을 클리어 한 상태(1)라면 일러스트버튼 활성화
        if (coinStatus == 1)
        {
            if (checkAni == 0)
            {

                PlayerPrefs.SetInt("AniWiki", 1);
                TabiWikiShowCheck.gameObject.SetActive(true);
                // 두 번째 이미지 애니메이션 재생
                yield return new WaitForSeconds(GetAnimationClipLength(TabiWikiShowCheck.GetComponent<Animator>(), "UI_OpenIWiki"));
                // 두 번째 버튼 활성화
            }
            TabiWikiBtn.SetActive(true);
        }
        else
        {

            TabiWikiBtn.SetActive(false);
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        OpeningVideo.gameObject.SetActive(false);
        OpenVideoRaw.gameObject.SetActive(false);
        MainAudio.Play();

        StartCoroutine(PlayAnimations());

    }
    void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs have been reset.");
    }
    void GetAllPlayerPrefs()
    {
        for (int i = 0; i < 9; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 1);
        }
        Debug.Log("All PlayerPrefs have been reset.");
        PlayerPrefs.SetInt("GameClear", 1);
        PlayerPrefs.Save();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B))
        {
            ResetPlayerPrefs();
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
        {
            GetAllPlayerPrefs();
        }

        if (OnTabiWiki)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll != 0)
            {
                ScrollImagesBy(scroll);
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                MainAudio.UnPause();
                OnTabiWiki = false;
                TabiWiki.SetActive(false);
                currentIndex = 1;
                // 모든 이미지를 비활성화하고 현재 인덱스의 이미지만 활성화
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].gameObject.SetActive(i == currentIndex);
                }
            }
        }
        if (Illustration.gameObject.activeSelf)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                MainAudio.UnPause();
                Illustration.gameObject.SetActive(false);
            }
            if (Illu == 6)
            {
                // 7번 이미지일 경우 일정 시간마다 이미지를 변경
                if (changeImageCoroutine == null)
                {
                    changeImageCoroutine = StartCoroutine(ChangeSeventhImage());
                }
            }
            else
            {
                StopChangeImageCoroutine();
            }
        }

    }

    private Coroutine changeImageCoroutine;

    private IEnumerator ChangeSeventhImage()
    {
        while (true)
        {
            int coinStatus = PlayerPrefs.GetInt(Illu.ToString(), 0);
            Real_Illustration.sprite = sprites[6]; // 첫 번째 7번 이미지
            IllustrationText.text = IllustText[6];
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "아직 찾지 못했습니다!";
            }
            yield return new WaitForSeconds(2); // 2초 대기
            Real_Illustration.sprite = Illur7_2; // 두 번째 7번 이미지
            IllustrationText.text = Illur7_2text;
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "아직 찾지 못했습니다!";
            }
            yield return new WaitForSeconds(2); // 2초 대기
        }
    }

    /// <summary>
    /// 해상도 고정 함수
    /// </summary>
    public void SetResolution()
    {
        int setWidth = 1920; // 화면 너비
        int setHeight = 1080; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, true);
    }
    private void StopChangeImageCoroutine()
    {
        if (changeImageCoroutine != null)
        {
            StopCoroutine(changeImageCoroutine);
            changeImageCoroutine = null;
        }
    }
}
