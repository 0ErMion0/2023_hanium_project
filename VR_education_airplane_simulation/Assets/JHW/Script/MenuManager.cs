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
    // Ʃ�丮�� ��ư Ŭ����
    public void MenuButton_Tutorial_OnMouseClick()
    {
        // ��ư Ŭ���� ������ �Լ�
        SceneManager.LoadScene("Tutorial");
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button2);
    }
}