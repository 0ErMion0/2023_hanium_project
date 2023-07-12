using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

// �޴�ȭ�� ��ư
partial class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainTitle;
    [SerializeField] GameObject chapterSelect;

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
        chapterSelect.SetActive(false);
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

}