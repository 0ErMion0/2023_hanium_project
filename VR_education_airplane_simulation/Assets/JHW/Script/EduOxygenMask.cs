using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

partial class EduOxygenMask : MonoBehaviour
{
    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

    // �¹���
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
        string key = "EduOxygenMask_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduOxygenMask_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue);
        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
        stewardess.GetComponent<Animator>().SetBool("Talk", true);

        switch (scriptIndex)
        {
            case 2: // �ǽ� - ������ ��Ҹ���ũ�� ��� �տ� �ִ� �°����� �����ּ���.
                npc.SetActive(true);
                mask.SetActive(true);
                stewardess.GetComponent<Stewardess>().MaskEquipAnim();
                mask.transform.DOLocalMoveY(0f, 4.5f);
                break;
            case 7:
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

partial class EduOxygenMask : MonoBehaviour
{
    // �ǽ� ������Ʈ��
    [SerializeField] GameObject npc; // ��Ҹ���ũ �� npc
    [SerializeField] GameObject npc_head; // npc �� ��ġ
    [SerializeField] GameObject mask; // ��Ҹ���ũ Object
    [SerializeField] GameObject maskStartPos; // ��Ҹ���ũ ���� ��ġ ������Ʈ
    [SerializeField] GameObject maskEndPos; // ��Ҹ���ũ �� ��ġ ������Ʈ
    [SerializeField] GameObject maskDropPos; // ��Ҹ���ũ ��� ��ġ ������Ʈ

    [SerializeField] GameObject maskStrap; // ��Ҹ���ũ ��Ʈ��
    [SerializeField] GameObject maskStrap_indicator; // ��Ҹ���ũ ��Ʈ�� - ���� ȭ��ǥ
    [SerializeField] Material maskStrap_highlightMat; // ��Ҹ���ũ ��Ʈ�� - ���� ���͸���(����)
    [SerializeField] Material maskStrap_highlightMat_origin; // ��Ҹ���ũ ��Ʈ�� - ���� ���͸���
    [SerializeField] GameObject maskStrap_highlightBone; // ��Ҹ���ũ ��Ʈ�� ���϶� bone(amature)

    public void MaskSelectEntered()
    {
        // UX ����
        maskEndPos.transform.GetChild(0).gameObject.SetActive(false);
        
        maskDropPos.SetActive(true);
    }

    public void MaskSelectExited()
    {
        maskDropPos.SetActive(false);
        if (Vector3.Distance(maskEndPos.transform.position, maskDropPos.transform.position) < 0.4f)
        {
            maskEndPos.GetComponent<XRGrabInteractable>().enabled = false;
            maskEndPos.transform.SetParent(npc_head.transform);
            //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":-2.4155960083007814,"y":6.399694442749023,"z":16.067296981811525},"rotation":{"x":-0.40882986783981326,"y":-0.5377156138420105,"z":-0.320950984954834,"w":0.6638607382774353},"scale":{"x":7.499998569488525,"y":7.500002384185791,"z":7.500000476837158}}
            maskEndPos.transform.DOMove(new Vector3(-2.4155960083007814f, 6.399694442749023f, 16.067296981811525f), 2f).OnComplete(() => { maskStrap_indicator.SetActive(true); StartCoroutine(MaskStrapUX()); }); // ����ũ �̵� ���ϸ� ��Ʈ�� ux ����
            maskEndPos.transform.DOLocalRotate(new Vector3(265f, 0f, 10f), 2f);

            // ���� ��ũ��Ʈ��
            scriptIndex++;
            StartCoroutine(NextScript());
        }
        else // ��Ҹ���ũ npc�� �� ���� ��� (��ġ�� �� ������ ���)
        {
            // UX �ٽ� Ű��
            maskEndPos.transform.GetChild(0).gameObject.SetActive(true);
            // ����ũ ���� ��ġ��
            maskEndPos.transform.position = maskStartPos.transform.position;
        }

    }

    IEnumerator MaskStrapUX(int cnt=0)
    {
        if (cnt == 8)
        {
            maskStrap_indicator.SetActive(false);
            maskStrap_highlightBone.transform.DOScale(new Vector3(.1f, .1f, .1f),3f);
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            if(cnt%2==0) maskStrap.GetComponent<SkinnedMeshRenderer>().material = maskStrap_highlightMat;
            else maskStrap.GetComponent<SkinnedMeshRenderer>().material = maskStrap_highlightMat_origin;
            StartCoroutine("MaskStrapUX", cnt + 1);
        }
    }
}
