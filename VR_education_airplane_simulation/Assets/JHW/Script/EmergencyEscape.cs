using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class EmergencyEscape : MonoBehaviour
{
    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    // �� ��ġ üũ
    [SerializeField] GameObject leftHand, rightHand;
    [SerializeField] GameObject leftHandPos, rightHandPos;

    // �ǽ� ������Ʈ��
    [SerializeField] GameObject baggage; // �� ������Ʈ
    [SerializeField] GameObject dropBaggage; // �ٴڿ� �������� �� ���� �� ������Ʈ
    [SerializeField] GameObject originBaggagePos; // �ٴڿ� �������� �� ���� ��ġ
    [SerializeField] GameObject curtain;
    [SerializeField] GameObject pathUX;

    // ������ȯ - �÷��̾� ��Ʈ�ѷ� ����
    [SerializeField] GameObject playerController1;
    [SerializeField] GameObject playerController2;
    [SerializeField] GameObject scriptReference; // ��ũ��Ʈ ���۷���

    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

    // �¹���
    [SerializeField] GameObject stewardess;

    int scriptIndex = 0;

    TextMeshProUGUI originText;
    LocalizeStringEvent originLocalize;

    private void Start()
    {
        // �ڸ� ����
        NextButtonOnClick();
    }

    IEnumerator NextScript()
    {
        string key = "EduEmergencyEscape_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduEmergencyEscape_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        if (scriptText.text.Length < 50) scriptText.fontSize = 20; else scriptText.fontSize = 15; // ��Ʈ ���̿� ���� ũ������
        TextToSpeach.Instance.SpeechText(scriptValue);
        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
        stewardess.GetComponent<Animator>().SetBool("Talk", true);

        switch (scriptIndex)
        {
            case 9:
                baggage.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(false);
                break;
            case 10:
                // �÷��̾� ���� ����
                playerController1.SetActive(false);
                playerController2.SetActive(true);

                // �ڸ� ��ġ ���Ҵ� �� �缳��
                originText = scriptText;
                originLocalize = localizeStringEvent;
                scriptText = scriptReference.GetComponent<TextMeshProUGUI>();
                localizeStringEvent = scriptReference.GetComponent<LocalizeStringEvent>();
                scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
                // Ŀư
                curtain.GetComponent<XRGrabInteractable>().enabled=true;
                curtain.transform.GetChild(0).gameObject.SetActive(true);
                // �� UX ON
                pathUX.SetActive(true);

                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 11:
                // �÷��̾� ���� ����
                playerController1.SetActive(true);
                playerController2.SetActive(false);

                // �ڸ� ��ġ ���Ҵ� �� �缳��
                scriptText = originText;
                localizeStringEvent = originLocalize;
                scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");

                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 12:
                leftHandPos.SetActive(true);
                rightHandPos.SetActive(true);
                StartCoroutine(CheckHandPos());
                break;
            case 20:
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

    public void baggageSelectEntered()
    {
        baggage.transform.GetChild(0).gameObject.SetActive(false); // Grab UX off

        dropBaggage.SetActive(true); // drop pos object on
        
    }

    public void baggageSelectExited()
    {
        // �ٴڿ� �� ���� ���Ҵٸ� (�Ÿ��� �ִٸ�) ������ġ��
        if (Vector3.Distance(baggage.transform.position, dropBaggage.transform.position) > 0.4f)
        {
            baggage.transform.GetChild(0).gameObject.SetActive(false); // Grab UX on
            baggage.transform.position = originBaggagePos.transform.position; // ���� ������ġ��
            dropBaggage.SetActive(false);
        }
        else // �ٴڿ� ���� ���Ҵٸ�
        {
            baggage.transform.position = dropBaggage.transform.position; // �� ��ġ ����������ġ�� ����
            baggage.GetComponent<XRGrabInteractable>().enabled = false; // ��ȣ�ۿ� �Ұ����ϰ�

            baggage.transform.GetChild(0).gameObject.SetActive(false); // ux off
            dropBaggage.SetActive(false); // ux off

            // ���� ��ũ��Ʈ��
            scriptIndex++;
            StartCoroutine(NextScript());
        }
    }

    // Ŀư ��ȣ�ۿ�
    public void CurtainSelectEntered(GameObject _obj)
    {
        Debug.Log(Vector3.Distance(playerController2.transform.position, _obj.transform.position));
        if (Vector3.Distance(playerController2.transform.position, _obj.transform.position) > 10f) return; // �Ÿ� �ʹ� �ָ� ���� X
        _obj.transform.DOScaleX(0.1f, 1f); // Ŀư ������������
        _obj.transform.GetChild(0).gameObject.SetActive(false); // ux off
        _obj.transform.GetChild(1).gameObject.SetActive(false); // Collider off
        _obj.GetComponent<XRGrabInteractable>().enabled = false; // Ŀư�� ���̻� ��ȣ�ۿ� ���ϰ�
    }

    // ��� grab
    public void EmergencyExitGateSelectEntered(GameObject _obj)
    {
        Destroy(_obj);
        playerController1.transform.position = _obj.transform.position;
        pathUX.SetActive(false);
        playerController1.transform.localRotation = Quaternion.Euler(new Vector3(0, -90f, 0)); // ī�޶� �����̴��� ���ϵ���
        
        scriptIndex++;
        StartCoroutine(NextScript());
    }

    IEnumerator CheckHandPos()
    {
        Debug.Log(Vector3.Distance(leftHand.transform.position, leftHandPos.transform.position) + ", " + Vector3.Distance(rightHand.transform.position, rightHandPos.transform.position));

        // �� �������� �ֱ������� �˻�
        if(Vector3.Distance(leftHand.transform.position, leftHandPos.transform.position)<.15f && Vector3.Distance(rightHand.transform.position, rightHandPos.transform.position)<.15f)
        {
            leftHandPos.SetActive(false);
            rightHandPos.SetActive(false);
            scriptIndex++;
            StartCoroutine (NextScript());
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(CheckHandPos());
        }

    }
}
