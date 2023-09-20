using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public partial class TutorialManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject tutorialInfo;

    // �¹���
    [SerializeField] GameObject stewardess;

    int curTextIndex = 0;
    int maxTextIndex = 7;

    List<string> scriptList = new List<string>();
    string currentLanguage;

    // Start is called before the first frame update
    void Start()
    {
        // �ؽ�Ʈ ���� �ҷ�����, ��ũ��Ʈ ��Ƴ��� ���߿� tts ��� ������ ��� ����
        //List<Dictionary<string, object>> list = CSVReader.Read("Localization/TutorialScript");
        //currentLanguage = LocalizationSettings.SelectedLocale.Identifier.CultureInfo.DisplayName;

        //for (int i = 0; i < list.Count; i++)
        //{
        //    switch (currentLanguage)
        //    {
        //        case "English":
        //            scriptList.Add(list[i]["English(en)"].ToString());
        //            break;
        //        case "Korean":
        //            scriptList.Add(list[i]["Korean(ko)"].ToString());
        //            break;
        //    }
        //    // Ʃ�丮�� ��ũ��Ʈ �ƴϸ� ����Ʈ�� �׸� ���
        //    if (!list[i]["Key"].ToString().Contains("tutorial_script")) break;
        //    maxTextIndex++;
        //}
        StartCoroutine(NextScript());
    }

    IEnumerator NextScript()
    {
        curTextIndex++;
        string key = "tutorial_script" + curTextIndex;

        // �ڸ��� ũ������ �� ��������Ʈ ����
        localizeStringEvent.StringReference.SetReference("TutorialScript", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        Debug.Log(scriptValue);

            RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
            scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

            stewardess.GetComponent<Animator>().SetBool("Talk", false); // �̹� true�� �����Ǿ��ִ� ��찡 �־ false�� ���� ���� true�� ����
            stewardess.GetComponent<Animator>().SetBool("Talk", true);
            stewardess.GetComponent<Stewardess>().RandomTalkAnimation(); // Talk �ִϸ��̼� ��������

            scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);

        // �۾� ũ�⿡ ���� �۾� �ڸ��� ����
        //if ((scriptList[curTextIndex]).Length >= 30) scriptText.transform.parent.GetComponent<RectTransform>().DOScaleY(1.5f, .5f);
        //else { scriptText.transform.parent.GetComponent<RectTransform>().DOScaleY(1f, .5f); }
        
        yield return new WaitForSeconds(scriptValue.Length * 0.1f);
        nextButton.SetActive(true);

    }

    public void NextButtonOnClick()
    {
        switch (curTextIndex)
        {
            case 2:
                OpenTutorialInfo();
                break;
            case 3:
                tutorialInfo.transform.GetChild(0).gameObject.SetActive(false);
                tutorialInfo.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 4:
                tutorialInfo.transform.GetChild(1).gameObject.SetActive(false);
                tutorialInfo.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 5:
                tutorialInfo.transform.GetChild(2).gameObject.SetActive(false);
                tutorialInfo.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 6:
                CloseTutorialInfo();
                break;
        }
        nextButton.SetActive(false);

        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button1);

        // Ʃ�丮�� �ؽ�Ʈ �ε����� �ִ�ġ�� ������Ʈ ��ȣ�ۿ� ��������, �ƴϸ� ���� �ؽ�Ʈ ��� �ڷ�ƾ ����
        if (curTextIndex >= maxTextIndex) StartPracticeTutorial();
        else StartCoroutine(NextScript());
    }

    void OpenTutorialInfo() { tutorialInfo.SetActive(true); }
    void CloseTutorialInfo() { tutorialInfo.SetActive(false); }
}


// Ʃ�丮�� - ������Ʈ ��ȣ�ۿ� ����
partial class TutorialManager
{
    [Header("== Ʃ�丮�� - ��ȣ�ۿ� ���� ==")]

    [SerializeField] LocalizeStringEvent localizeStringEvent;
    [SerializeField] GameObject tutorialObjects;
    [SerializeField] GameObject targetObj; // Ÿ�� ������Ʈ
    [SerializeField] GameObject originPosObj; // ��ü ���� ��ġ
    [SerializeField] GameObject dropPosObj; // ��ü ���ƾ� �� ��ġ
    [SerializeField] GameObject dropObj; // ���� ��ġ�� ��ü
    [SerializeField] GameObject book; // å 1
    [SerializeField] GameObject bookOpen; // å 2

    bool isSelected; // ��ü ����ִ��� ����
    bool isTriggered; // ��ȣ�ۿ� ����


    void StartPracticeTutorial()
    {
        // ������Ʈ Ȱ��ȭ
        tutorialObjects.SetActive(true);

        // �ڸ� ����
        string key = "tutorial_grab";
        localizeStringEvent.StringReference.SetReference("TutorialScript", key);
        TextToSpeach.Instance.SpeechText(localizeStringEvent.StringReference.GetLocalizedString(key));


    }
    
    public void TutorialObject01Selected()
    {
        isSelected = true; // ��ü ����ִ��� ����
        isTriggered = false; // ��ȣ�ۿ뿩��

        // �ڸ� ���� �� tts
        string key = "tutorial_trigger";
        localizeStringEvent.StringReference.SetReference("TutorialScript", key);
        TextToSpeach.Instance.SpeechText(localizeStringEvent.StringReference.GetLocalizedString(key));

        // UX.. �ϵ��ڵ� �˼��մϴ�
        book.transform.GetChild(0).gameObject.SetActive(false); // Grab ux off
        book.transform.GetChild(1).gameObject.SetActive(true); // Trigger ux on
    }

    public void TutorialObject01Exited()
    {
        isSelected = false; // ��ü ����ִ��� ����
        dropObj.SetActive(false);



        // ��ü ������ ���� ��ġ�� ���ƾ� �� ��ġ ���� �Ÿ� üũ
        Debug.Log(Vector3.Distance(targetObj.transform.position, dropPosObj.transform.position));
        if (!(Vector3.Distance(targetObj.transform.position, dropPosObj.transform.position) < 0.5)) // ���ƾ� �� ��ġ�� ���� �ʾҴٸ� �ٽ� �����·� ����
        {
            targetObj.transform.position = originPosObj.transform.position;
            targetObj.transform.eulerAngles = new Vector3(0, 0, 0);

            // ���� ��ȣ�ۿ� �� ���¿��� ��ü ��������
            if (isTriggered)
            {
                isTriggered = false; // ��ȣ�ۿ� �ߴ��� ����

                // ��ü Ȱ��ȭ/��Ȱ��ȭ ���� �ʱ�ȭ
                book.SetActive(true);
                bookOpen.SetActive(false);
            }

            // �ڸ� ���� �� tts
            string key = "tutorial_grab";
            localizeStringEvent.StringReference.SetReference("TutorialScript", key);
            TextToSpeach.Instance.SpeechText(localizeStringEvent.StringReference.GetLocalizedString(key));

            // UX.. �ϵ��ڵ� �˼��մϴ�
            book.transform.GetChild(0).gameObject.SetActive(true); // Grab ux on
            book.transform.GetChild(1).gameObject.SetActive(false); // Trigger ux of
        }
        else
        {
            targetObj.SetActive(false);
            dropObj.transform.GetChild(0).gameObject.SetActive(false);
            dropObj.transform.GetChild(0).gameObject.SetActive(true);

            // �ڸ� ���� �� tts
            string key = "tutorial_clear";
            localizeStringEvent.StringReference.SetReference("TutorialScript", key);
            TextToSpeach.Instance.SpeechText(localizeStringEvent.StringReference.GetLocalizedString(key));


            // Ÿ��Ʋ �̵�
            Invoke("GoToMain", localizeStringEvent.StringReference.GetLocalizedString(key).Length*0.1f + 3f);

        }

    }

    private void GoToMain()
    {             // Ÿ��Ʋ�� �̵�
        SceneManager.LoadScene("MainTitle");
    }

    public void TutorialObject01Triggered() // ��ü ��������� ��ȣ�ۿ�
    {
        if (isTriggered) return; // �̹� ��ȣ�ۿ� ������ ����X


        if (isSelected) // ��ü ���� ���� �ƴϸ� ���� x
        {
            isTriggered = true;
            // ��ü ������ ���ƾ� �� ��ġ ����, å ����������� Ȱ��ȭ
            dropObj.SetActive(true);
            book.SetActive(false);
            bookOpen.SetActive(true);

            // �ڸ� ���� �� tts
            string key = "tutorial_drop";
            localizeStringEvent.StringReference.SetReference("TutorialScript", key);
            TextToSpeach.Instance.SpeechText(localizeStringEvent.StringReference.GetLocalizedString(key));
        }


    }
}



partial class TutorialManager
{
    public void popup_reStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void popup_toMainTitle()
    {
        SceneManager.LoadScene("MainTitle");
    }
    public void popup_exitPopup(GameObject popup)
    {
        popup.SetActive(false);
    }
}