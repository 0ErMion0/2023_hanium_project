using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using Amazon.Polly;
using TMPro;
using UnityEngine.Localization.Components;

// �޴�ȭ�� ��ư
partial class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainTitle;
    [SerializeField] GameObject EssentialEdu;
    [SerializeField] GameObject SelectiveEdu;
    [SerializeField] GameObject SimulationEdu;
    [SerializeField] GameObject OXQuizEdu;
    [SerializeField] GameObject preferenceCanvas;

    [SerializeField] TMP_Dropdown dropdown_localization;
    [SerializeField] List<Sprite> language_sprites;

    [SerializeField] GameObject discription_edu;

    // é�� Ŭ�����ߴ��� ���� �˻�
    [Header("== é�� Ŭ���� ���� �˻� ==")]
    [SerializeField] List<Image> ChapterImageList;

    [SerializeField] Sprite isClearedChapter;

    private void Start()
    {
        StartCoroutine(InitDropdown());

        PlayerDataLoad();
    }

    // Ʃ�丮�� ��ư Ŭ����
    public void MenuButton_Tutorial_OnMouseClick()
    {
        // ��ư Ŭ���� ������ �Լ�

    }

    // �ʼ� �������� ��ư Ŭ����
    public void MenuButton_AssentialEdu_OnMouseClick()
    {
        mainTitle.SetActive(false);
        EssentialEdu.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }


    // ���� �������� ��ư Ŭ����
    public void MenuButton_SeletiveEdu_OnMouseClick()
    {
        mainTitle.SetActive(false);
        SelectiveEdu.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // ���� �ó����� ���� ��ư Ŭ����
    public void MenuButton_SimulationEdu_OnMouseClick()
    {
        mainTitle.SetActive(false);
        SimulationEdu.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // OX ���� ���� ��ư Ŭ����
    public void MenuButton_OXQuizEdu_OnMouseClick()
    {
        mainTitle.SetActive(false);
        OXQuizEdu.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // �ڷ� ��ư
    public void MenuButton_Back_OnMouseClick(GameObject CurrentCanvas)
    {
        mainTitle.SetActive(true);
        CurrentCanvas.SetActive(false);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // ���� ��ư
    public void MenuButton_Preference_OnMouseClick()
    {
        mainTitle.SetActive(false);
        preferenceCanvas.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    public void MenuButton_Quit_OnMouseClick()
    {
        Application.Quit();
    }

    public void MenuButton_Chapter_OnMouseClick(int chap_num)
    {
        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);

        // é�� �ѹ��� ���� �����ϴ� ��� �ٸ�
        // ��ư Ŭ���� ������ �Լ�
        switch (chap_num)
        {
            // ��� ����
            case 0: 
                SceneManager.LoadScene("Tutorial");
                break;

            // �ʼ� ��������
            case 1: // ������Ʈ
                SceneManager.LoadScene("EduSafetyBelt");
                break;
            case 2:
                SceneManager.LoadScene("EduBackrestAngle");
                break;
            case 3:
                SceneManager.LoadScene("EduGuideSafetyRules");
                break;
            case 4: // ��Ҹ���ũ
                SceneManager.LoadScene("EduOxygenMask");
                break;
            case 5: // ��������
                SceneManager.LoadScene("EduLifeJacket");
                break;
            case 6: // ��ݹ����ڼ�
                SceneManager.LoadScene("EduAntiShockPosture");
                break;
            case 7: // ���Ż��
                SceneManager.LoadScene("EduEmergencyEscape");
                break;

            // ���� ��������
            case 8:
                SceneManager.LoadScene("EduTerror");
                break;
            case 9:
                SceneManager.LoadScene("EduPersonalProblem");
                break;

            // ���� �ó����� ����
            case 10:
                SceneManager.LoadScene("SimulationEdu");
                break;

            // OX �����
            //case 11:
            //    break;

            default:
                break;
        }
    }



    // ��� �����
    public void OnChangeLanguage(TMP_Dropdown _dropdown)
    {
        //string currentLanguage = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.DisplayName; // ���� ���
        string languageName = _dropdown.options[_dropdown.value].text;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(languageName);
    }

    IEnumerator InitDropdown()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;

            if (locale.name== "Korean (South Korea) (ko-KR)") options.Add(new TMP_Dropdown.OptionData("�ѱ���"));
            if (locale.name == "English (en)") options.Add(new TMP_Dropdown.OptionData("English"));
            //options.Add(new TMP_Dropdown.OptionData(locale.name));
        }

        dropdown_localization.options = options;

        dropdown_localization.value = selected;
        dropdown_localization.onValueChanged.AddListener(LocaleSelected);
    }

    static void LocaleSelected(int index)
    {
        if (index == 0) LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        if (index == 1) LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[6];
        //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    // ������ ������ �ø��� ���� ����
    public void Button_OnMouseEnter(int num)
    {
        string key = "Menu_start_dis_" + num;
        discription_edu.GetComponent<LocalizeStringEvent>().StringReference.SetReference("Menu_StringTable",key);
    }

    // ���� Ŭ�����ߴ��� ���� �˻�
    private void PlayerDataLoad()
    {
        for (int i = 0; i < 7; i++)
        {
            // ��ư Ȱ��ȭ �� �ؽ�Ʈ ������ ���� + �̹��� ������ �������
            ChapterImageList[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
            ChapterImageList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            ChapterImageList[i].transform.GetChild(1).GetComponent<Image>().color = Color.white;

            // Ŭ���� �� ���
            if (PlayerPrefs.GetInt("Chapter" + (i + 1).ToString()) != 0)
            {
                ChapterImageList[i].sprite = isClearedChapter;
            }
            // Ŭ���� ���� ���
            else
            {
                break;
            }
        }
        
    }
}