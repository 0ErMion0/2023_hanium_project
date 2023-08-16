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

// �޴�ȭ�� ��ư
partial class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainTitle;
    [SerializeField] GameObject EssentialEdu;
    [SerializeField] GameObject SelectiveEdu;
    [SerializeField] GameObject preferenceCanvas;

    [SerializeField] TMP_Dropdown dropdown_localization;
    [SerializeField] List<Sprite> language_sprites;

    private void Start()
    {
        StartCoroutine(InitDropdown());
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
    }


    // ���� �������� ��ư Ŭ����
    public void MenuButton_SeletiveEdu_OnMouseClick()
    {
        mainTitle.SetActive(false);
        SelectiveEdu.SetActive(true);
    }

    // �ڷ� ��ư
    public void MenuButton_Back_OnMouseClick()
    {
        mainTitle.SetActive(true);
        EssentialEdu.SetActive(false); // é�� ����
        SelectiveEdu.SetActive(false);
        preferenceCanvas.SetActive(false);
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
            // �ʼ� ��������
            case 0: // Ʃ�丮��
                SceneManager.LoadScene("Tutorial");
                break;
            case 1: // ������Ʈ
                SceneManager.LoadScene("EduSafetyBelt");
                break;
            //case 2:
            //    break;
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

            default:
                break;
        }
    }

    // ���� ��ư
    public void MenuButton_Preference_OnMouseClick()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("EduLifeJacket"));
        // ��ư Ŭ���� ������ �Լ�
        mainTitle.SetActive(false);
        preferenceCanvas.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
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
            options.Add(new TMP_Dropdown.OptionData(locale.name));
        }
        dropdown_localization.options = options;

        dropdown_localization.value = selected;
        dropdown_localization.onValueChanged.AddListener(LocaleSelected);
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}