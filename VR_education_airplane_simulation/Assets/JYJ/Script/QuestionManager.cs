using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

partial class QuestionManager : MonoBehaviour
{
    // ���� ox����
    static char[] quizAnswer = { '-', 'O', 'O', 'X', 'X', 'O', 'X', 'X', 'O', 'X', 'X', 'X', 'O', 'X', 'X', 'O' };

    // ������ �ڸ� ��ũ��Ʈ �ε��� ��ȣ
    static private int MAX_SCRIPT_INDEX = quizAnswer.Length;

    // ���൵
    [SerializeField] TextMeshProUGUI progressText;

    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    // OX ��ư
    [SerializeField] GameObject btn_O;
    [SerializeField] GameObject btn_X;

    // �¹���
    [SerializeField] GameObject stewardess;

    int quizIndex = 0;

    int answerCnt;
    int wrongCnt;

    private void Start()
    {
        // �ڸ� ����
        StartCoroutine(NextScript());
    }

    IEnumerator NextScript()
    {
        progressText.text = quizIndex +" / " + quizAnswer.Length; // ���൵

        string key = "Quiz_script" + quizIndex;

        localizeStringEvent.StringReference.SetReference("Quiz_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(quizIndex == MAX_SCRIPT_INDEX); // Talk �ִϸ��̼� ��������, ���� ���� ������(scriptIndex=MAX_SCRIPT_INDEX �̸�) ����ϴ� ���

        switch (quizIndex)
        {
            case 0:
                quizIndex++;
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                StartCoroutine(NextScript());
                break;
            case 16:
                progressText.text = "- Results -\nanswer : " + answerCnt + '\n' + "wrong : " + wrongCnt;
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                SceneManager.LoadScene("MainTitle");
                break;
            default:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                // ��ư Ȱ��ȭ
                btn_O.SetActive(true);
                btn_X.SetActive(true);
                break;
        }
    }

    public void NextButton_O()
    {
        btn_O.SetActive(false);
        btn_X.SetActive(false);


        // �����̸�
        if (quizAnswer[quizIndex] == 'O')
        {
            // ���� ��
            answerCnt++;

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.answer);
        }
        else // �����̸�
        {
            // ���� ��
            wrongCnt++;

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.wrong);
        }

        // ���� ��ũ��Ʈ��
        quizIndex++;
        StartCoroutine(NextScript());
    }

    public void NextButton_X()
    {
        btn_O.SetActive(false);
        btn_X.SetActive(false);

        // �����̸�
        if (quizAnswer[quizIndex] == 'X')
        {
            // ���� ��
            answerCnt++;

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.answer);
        }
        else // �����̸�
        {
            // ���� ��
            wrongCnt++;

            // ����
            SoundManager.Instance.PlaySFX(SoundManager.SFX_list.wrong);
        }

        // ���� ��ũ��Ʈ��
        quizIndex++;
        StartCoroutine(NextScript());
    }
}

partial class QuestionManager
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