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
    [SerializeField] GameObject chapterSelect;
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
        SceneManager.LoadScene("Tutorial");
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // �ʼ� �������� ��ư Ŭ����
    public void MenuButton_AssentialEdu_OnMouseClick()
    {
        mainTitle.SetActive(false);
        chapterSelect.SetActive(true);
    }

    // �ڷ� ��ư
    public void MenuButton_Back_OnMouseClick()
    {
        mainTitle.SetActive(true);
        chapterSelect.SetActive(false); // é�� ����
        preferenceCanvas.SetActive(false);
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // Chapter 1
    public void MenuButton_Chapter1_OnMouseClick()
    {
        // ��ư Ŭ���� ������ �Լ�
        SceneManager.LoadScene("EduSafetyBelt");
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // Chapter 3
    public void MenuButton_Chapter3_OnMouseClick()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("EduGuideSafetyRules"));
        // ��ư Ŭ���� ������ �Լ�
        SceneManager.LoadScene("EduGuideSafetyRules");
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }

    // Chapter 5
    public void MenuButton_Chapter5_OnMouseClick()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("EduLifeJacket"));
        // ��ư Ŭ���� ������ �Լ�
        SceneManager.LoadScene("EduLifeJacket");
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
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