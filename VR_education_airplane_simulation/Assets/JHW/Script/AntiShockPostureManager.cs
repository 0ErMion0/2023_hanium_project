using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public partial class AntiShockPostureManager : MonoBehaviour
{
    // ������ �ڸ� ��ũ��Ʈ �ε��� ��ȣ
    static private int MAX_SCRIPT_INDEX = 7;

    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

    // �¹���
    [SerializeField] GameObject stewardess;

    int scriptIndex = 0;

    bool isRightLeg;
    bool isLeftLeg;

    private void Start()
    {
        // �ڸ� ����
        NextButtonOnClick();
    }

    IEnumerator NextScript()
    {
        string key = "EduAntiShockPosture_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduAntiShockPosture_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(scriptIndex==MAX_SCRIPT_INDEX); // Talk �ִϸ��̼� ��������, ���� ���� ������(scriptIndex=MAX_SCRIPT_INDEX �̸�) ����ϴ� ���

        switch (scriptIndex)
        {
            case 2:
                postureImage.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 3:
                postureImage.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 6: // �ǽ�
                postureImage.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(false);
                playerLeg.SetActive(true);
                rightLegPos.SetActive(true);
                leftLegPos.SetActive(true);
                StartCoroutine(CheckHandPos());
                break;
            case 7: // �¹��� ���� �� -> ����ȭ�� ����
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

partial class AntiShockPostureManager {

    // ���� ������Ʈ
    [SerializeField] GameObject playerLeg;
    [SerializeField] GameObject rightLegPos;
    [SerializeField] GameObject leftLegPos;
    [SerializeField] GameObject postureImage;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;


    bool isHandOnLeg = false;

    //public void grabLeg(GameObject leg)
    //{
    //    // ��� �ٸ��� ��Ҵ����� ���� ���� ����, �Ÿ��� ũ��(�� ������ �ʾҴٸ�) ����X
    //    if (leg.name == "rightLegPos") {
    //        if (Vector3.Distance(leftHand.transform.position, leftLegPos.transform.position) > 0.4f) return;
    //        isRightLeg = true;
    //    }
    //    else {
    //        if (Vector3.Distance(rightHand.transform.position, rightLegPos.transform.position) > 0.4f) return;
    //        isLeftLeg = true;
    //    }

    //    //Debug.Log(Vector3.Distance(leftHand.transform.position,leftLegPos.transform.position));
    //    //Debug.Log(Vector3.Distance(rightHand.transform.position, rightLegPos.transform.position));

    //    // UX OFF
    //    leg.transform.GetChild(0).gameObject.SetActive(false);

    //    if (isRightLeg && isLeftLeg)
    //    {
    //        leftLegPos.SetActive(false);
    //        rightLegPos.SetActive(false);

    //        scriptIndex++;
    //        StartCoroutine(NextScript());
    //    }
    //}

    //public void dropLeg(GameObject leg)
    //{
    //    // ��� �ٸ��� ��Ҵ����� ���� ���� ����
    //    if (leg.name == "rightLegPos") isRightLeg = false; else isLeftLeg = false;

    //    // UX ON
    //    leg.transform.GetChild(0).gameObject.SetActive(true);
    //}

    public IEnumerator CheckHandPos(){
        if (!isHandOnLeg)
        {
            if (Vector3.Distance(leftHand.transform.position, leftLegPos.transform.position) < .2f &&
                Vector3.Distance(rightHand.transform.position, rightLegPos.transform.position) < .2f)
            {
                isHandOnLeg = true;
                StopCoroutine(CheckHandPos());

                leftLegPos.SetActive(false);
                rightLegPos.SetActive(false);

                scriptIndex++;
                StartCoroutine(NextScript());
            }
        }
        yield return new WaitForSeconds(.1f);
        StartCoroutine(CheckHandPos());
    }
}
