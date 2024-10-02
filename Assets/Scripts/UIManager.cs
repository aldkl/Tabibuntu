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

    public List<Image> images;  // UI �̹��� ����Ʈ
    public float scrollSensitivity = 0.1f;  // ��ũ�� ���� ���� ����
    private int currentIndex = 0;  // ���� ǥ�� ���� �̹��� �ε���
    private float scrollDelta = 0; // ��ũ�� ��ȭ�� ����

    private bool OnTabiWiki = false;
    public void OnclickLeftIll()
    {
        if (Illu > 0)//���� �Ϸ��� 1�̻��̸�
        {
            Illu--;//���̳ʽ� ���ְ�
            Real_Illustration.color = Color.white;
            Real_Illustration.sprite = sprites[Illu];//�Ϸ� �������ֱ�
            IllustrationText.text = IllustText[Illu];
            int coinStatus = PlayerPrefs.GetInt(Illu.ToString(), 0);
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "���� ã�� ���߽��ϴ�!";
            }
            ResizeImage();
        }
    }

    public void OnclickRightIll()
    {
        if (Illu < sprites.Count - 1)//���� �Ϸ��� Count���� ������
        {
            Illu++;//���̳ʽ� ���ְ�
            Real_Illustration.color = Color.white;
            Real_Illustration.sprite = sprites[Illu];//�Ϸ� �������ֱ�
            IllustrationText.text = IllustText[Illu];
            int coinStatus = PlayerPrefs.GetInt(Illu.ToString(), 0);
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "���� ã�� ���߽��ϴ�!";
            }
            ResizeImage();
        }
    }

    public void ResizeImage()
    {
        // Sprite�� ���� ũ�⸦ �����ɴϴ�.
        Sprite sprite = Real_Illustration.sprite;
        if (sprite == null)
            return;

        Rect spriteRect = sprite.rect;
        Vector2 spriteSize = new Vector2(spriteRect.width, spriteRect.height);

        // RectTransform�� ũ�⸦ �����մϴ�.
        RectTransform rectTransform = Real_Illustration.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // �̹����� ������ �����ϸ鼭 ũ�⸦ �����մϴ�.
            float imageRatio = spriteSize.x / spriteSize.y;
            float parentHeight = rectTransform.rect.height;

            // �θ��� ���̿� ���߾� �ʺ� �����մϴ�.
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
            int direction = scrollDelta > 0 ? -1 : 1;  // ��ũ�� ����

            currentIndex = Mathf.Clamp(currentIndex + direction, 0, images.Count);  // �ε��� ���� ����
            if (currentIndex == images.Count)
            {
                MainAudio.UnPause();
                OnTabiWiki = false;
                TabiWiki.SetActive(false);
                currentIndex = 1;
                // ��� �̹����� ��Ȱ��ȭ�ϰ� ���� �ε����� �̹����� Ȱ��ȭ
                for (int i = 0; i < images.Count; i++)
                {
                    images[i].gameObject.SetActive(i == currentIndex);
                }
                return;
            }
            // ��� �̹����� ��Ȱ��ȭ�ϰ� ���� �ε����� �̹����� Ȱ��ȭ
            for (int i = 0; i < images.Count; i++)
            {
                images[i].gameObject.SetActive(i == currentIndex);
            }
            scrollDelta = 0; // ��ũ�� ��ȭ�� �ʱ�ȭ
        }
    }//�׹���Ű��ũ��


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
        // ������ �ϳ��� ���� ����(1)��� �Ϸ���Ʈ��ư Ȱ��ȭ
        if (coinStatus >= 1)
        {
            if (checkAni == 0)
            {
                Debug.Log("checkAni = " + checkAni);
                PlayerPrefs.SetInt("AniIllur", 1);
                IllustrationShowCheck.gameObject.SetActive(true);
                // ù ��° �̹��� �ִϸ��̼� ���
                yield return new WaitForSeconds(GetAnimationClipLength(IllustrationShowCheck.GetComponent<Animator>(), "UI_OpenIller"));
                // ù ��° ��ư Ȱ��ȭ
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
        // ������ Ŭ���� �� ����(1)��� �Ϸ���Ʈ��ư Ȱ��ȭ
        if (coinStatus == 1)
        {
            if (checkAni == 0)
            {

                PlayerPrefs.SetInt("AniWiki", 1);
                TabiWikiShowCheck.gameObject.SetActive(true);
                // �� ��° �̹��� �ִϸ��̼� ���
                yield return new WaitForSeconds(GetAnimationClipLength(TabiWikiShowCheck.GetComponent<Animator>(), "UI_OpenIWiki"));
                // �� ��° ��ư Ȱ��ȭ
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
                // ��� �̹����� ��Ȱ��ȭ�ϰ� ���� �ε����� �̹����� Ȱ��ȭ
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
                // 7�� �̹����� ��� ���� �ð����� �̹����� ����
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
            Real_Illustration.sprite = sprites[6]; // ù ��° 7�� �̹���
            IllustrationText.text = IllustText[6];
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "���� ã�� ���߽��ϴ�!";
            }
            yield return new WaitForSeconds(2); // 2�� ���
            Real_Illustration.sprite = Illur7_2; // �� ��° 7�� �̹���
            IllustrationText.text = Illur7_2text;
            if (coinStatus == 0)
            {
                Real_Illustration.color = new Color(0, 0, 0, 1);
                IllustrationText.text = "���� ã�� ���߽��ϴ�!";
            }
            yield return new WaitForSeconds(2); // 2�� ���
        }
    }

    /// <summary>
    /// �ػ� ���� �Լ�
    /// </summary>
    public void SetResolution()
    {
        int setWidth = 1920; // ȭ�� �ʺ�
        int setHeight = 1080; // ȭ�� ����

        //�ػ󵵸� �������� ���� ����
        //3��° �Ķ���ʹ� Ǯ��ũ�� ��带 ���� > true : Ǯ��ũ��, false : â���
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
