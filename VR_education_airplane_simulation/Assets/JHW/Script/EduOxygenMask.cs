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
                mask.transform.DOLocalMoveY(0f, 4f);
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
            maskEndPos.transform.SetParent(npc_head.transform);
            //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":-2.487746000289917,"y":6.484857082366943,"z":15.653846740722657},"rotation":{ "x":0.44855767488479617,"y":0.5209577679634094,"z":0.40008866786956789,"w":-0.606076180934906},"scale":{ "x":8.571427345275879,"y":8.571430206298829,"z":8.571429252624512} }
            maskEndPos.transform.DOMove(new Vector3(-2.487746000289917f, 6.484857082366943f, 15.653846740722657f),4f);
            maskEndPos.transform.DOLocalRotate(new Vector3(265f, 0f, 10f), 4f);

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
}
