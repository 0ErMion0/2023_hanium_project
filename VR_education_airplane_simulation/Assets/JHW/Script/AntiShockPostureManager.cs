using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class AntiShockPostureManager : MonoBehaviour
{   
    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    // ���� ������Ʈ
    [SerializeField] GameObject playerLeg;
    [SerializeField] GameObject rightLegPos;
    [SerializeField] GameObject leftLegPos;
    [SerializeField] GameObject postureImage;
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;


    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

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
        TextToSpeach.Instance.SpeechText(scriptValue);
        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");

        switch (scriptIndex)
        {
            case 2:
                postureImage.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 7: // �ǽ�
                postureImage.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(false);
                playerLeg.SetActive(true);
                rightLegPos.SetActive(true);
                leftLegPos.SetActive(true);
                leftLegPos.GetComponent<XRGrabInteractable>().enabled = true;
                rightLegPos.GetComponent<XRGrabInteractable>().enabled = true;
                break;
            case 8: // �¹��� ���� �� -> ����ȭ�� ����
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

    public void grabLeg(GameObject leg)
    {
        // ��� �ٸ��� ��Ҵ����� ���� ���� ����, �Ÿ��� ũ��(�� ������ �ʾҴٸ�) ����X
        if (leg.name == "rightLegPos") {
            if (Vector3.Distance(leftHand.transform.position, leftLegPos.transform.position) > 0.4f) return;
            isRightLeg = true; 
        }
        else {
            if (Vector3.Distance(rightHand.transform.position, rightLegPos.transform.position) > 0.4f) return;
            isLeftLeg = true;
        }

        //Debug.Log(Vector3.Distance(leftHand.transform.position,leftLegPos.transform.position));
        //Debug.Log(Vector3.Distance(rightHand.transform.position, rightLegPos.transform.position));

        // UX OFF
        leg.transform.GetChild(0).gameObject.SetActive(false);

        if(isRightLeg&&isLeftLeg)
        {
            leftLegPos.SetActive(false);
            rightLegPos.SetActive(false);

            scriptIndex++;
            StartCoroutine(NextScript());
        }
    }

    public void dropLeg(GameObject leg)
    {
        // ��� �ٸ��� ��Ҵ����� ���� ���� ����
        if (leg.name == "rightLegPos") isRightLeg = false; else isLeftLeg = false;

        // UX ON
        leg.transform.GetChild(0).gameObject.SetActive(true);
    }
}
