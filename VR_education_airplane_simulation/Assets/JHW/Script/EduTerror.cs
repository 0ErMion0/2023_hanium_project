using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

partial class EduTerror : MonoBehaviour
{

    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject stewardess;

    // Start is called before the first frame update
    void Start()
    {
        NextButtonOnClick();
    }

    // ���� �ڸ� �о����
    int scriptIndex = 0;
    IEnumerator NextScript()
    {
        string key = "EduTerror_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduTerror_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];
        
        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
        stewardess.GetComponent<Animator>().SetBool("Talk", false); // �̹� true�� �����Ǿ��ִ� ��찡 �־ false�� ���� ���� true�� ����
        stewardess.GetComponent<Animator>().SetBool("Talk", true);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(); // Talk �ִϸ��̼� ��������

        switch (scriptIndex)
        {
            case 12: // �¹��� ���� �� �ϸ� �ǽ�, ������Ʈ Ȱ��ȭ, next ��ư ��Ȱ��ȭ
                strange_bag.SetActive(true);
                break;
            case 13: // �¹��� ȣ�� ��ư
                callButton.SetActive(true);
                break;
            case 14:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                SceneManager.LoadScene("MainTitle");
                break;
            default:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
        }
    }
    public void NextButtonOnClick()
    {
        scriptIndex++;

        StartCoroutine(NextScript());
        nextButton.SetActive(false);

        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button1);
    }
}

partial class EduTerror
{

    // �ǽ� ������Ʈ
    [SerializeField] GameObject strange_bag; // ������ ����
    [SerializeField] GameObject callButton; // �¹��� ȣ�� ��ư
    bool isButtonClicked = false;

    // ���� ������
    public void StrangeBagelectEntered()
    {
        strange_bag.GetComponent<XRGrabInteractable>().enabled = false;
        strange_bag.transform.GetChild(0).gameObject.SetActive(false);
        strange_bag.transform.GetChild(1).gameObject.SetActive(true);

        // ���� ��ũ��Ʈ��
        scriptIndex++;
        StartCoroutine(NextScript());
    }

    // �¹��� ȣ�� ��ư Ŭ��
    public void StewardessCallOnButton()
    {
        // �̹� ��ư �������¸� ����X
        if (isButtonClicked) return;
        isButtonClicked = true;

        // UX
        strange_bag.SetActive(false);
        callButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0.8f, 0.5f);
        callButton.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

        // ���� ��ũ��Ʈ��
        NextButtonOnClick();
    }

}