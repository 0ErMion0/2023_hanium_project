using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

partial class BeltManager : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        NextButtonOnClick();
    }

    // ���� �ڸ� �о����
    int scriptIndex = 0;
    IEnumerator NextScript()
    {
        string key = "EduSafetyBelt_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("EduSafetyBelt_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(scriptIndex == MAX_SCRIPT_INDEX); // Talk �ִϸ��̼� ��������

        switch (scriptIndex)
        {
            case 2: // ��� ��Ʈ ���� ���� ������ ��Ʈ�� �������ּ���
                beltImage.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 3: // �¹��� ���� �� �ϸ� ��Ʈ ������Ʈ Ȱ��ȭ, ���� ��ư ��Ȱ��ȭ
                beltImage.SetActive(false);
                belt1.SetActive(true);
                belt2.SetActive(true);

                // ��Ʃ��� ��Ʈ�Ŵ� �ִϳ��̼�
                stewardess.GetComponent<Animator>().SetBool("Talk", false);
                stewardess.GetComponent<Stewardess>().BeltEquipAnim();

                // ��Ʈ UX
                BeltUX_object.SetActive(true);
                BeltUX_object.GetComponent<BeltUX>().BeltOn_UX();
                break;
            case 6: // ux �� ��ȣ�ۿ� Ȱ��ȭ
                beltEndPos2.GetComponent<XRGrabInteractable>().enabled = true;
                beltEndPos2.transform.GetChild(1).gameObject.SetActive(true);
                BeltUX_object.GetComponent<BeltUX>().BeltOff_UX();
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

partial class BeltManager : MonoBehaviour
{
    [SerializeField] GameObject belt1;
    [SerializeField] GameObject belt2;
    [SerializeField] GameObject beltEndPos1;
    [SerializeField] GameObject beltEndPos2;
    [SerializeField] GameObject beltLinekdPos1;
    [SerializeField] GameObject beltLinekdPos2;
    [SerializeField] GameObject beltLinekdPos3;
    [SerializeField] GameObject BeltUX_object;
    [SerializeField] GameObject beltImage;

    //[SerializeField] GameObject cam;
    bool isLinked;

    public void beltSelectEntered()
    {
        beltLinekdPos3.transform.GetChild(0).gameObject.SetActive(true);

        // ��Ʈ1 ������
        if (beltEndPos1.transform.parent==null)
        {
            beltEndPos1.transform.GetChild(0).gameObject.SetActive(false);
        }
        // ��Ʈ2 ������
        if(beltEndPos2.transform.parent == null)
        {
            beltEndPos2.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void beltSelectExited()
    {
        beltLinekdPos3.transform.GetChild(0).gameObject.SetActive(false);
        if (isLinked) return;
        //Debug.Log(Vector3.Distance(beltEndPos1.transform.position, beltLinekdPos1.transform.position) + " " + Vector3.Distance(beltEndPos2.transform.position, beltLinekdPos2.transform.position));
        if (Vector3.Distance(beltEndPos1.transform.position, beltLinekdPos1.transform.position) > 0.07f) return;
        if (Vector3.Distance(beltEndPos2.transform.position, beltLinekdPos2.transform.position) > 0.07f) return;

        isLinked = true;
        BeltUX_object.GetComponent<BeltUX>().BeltOnSucces();
        beltEndPos1.GetComponent<XRGrabInteractable>().enabled = false;
        beltEndPos2.GetComponent<XRGrabInteractable>().enabled = false;
        beltEndPos1.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        beltEndPos2.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        beltEndPos1.transform.DOMove(beltLinekdPos1.transform.position, 0.5f);
        beltEndPos2.transform.DOMove(beltLinekdPos2.transform.position, 0.5f);
        scriptIndex++;
        StartCoroutine(NextScript());
    }

    // ��Ʈ Ǯ��
    public void beltLinkedSelectEntered()
    {
        //��Ʈ ���� �ȵ������� ����X
        if (!isLinked) return;
        BeltUX_object.GetComponent<BeltUX>().BeltOffSucces();
        beltEndPos2.GetComponent<XRGrabInteractable>().enabled = false;
        beltEndPos1.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), .5f); //.localRotation = Quaternion.Euler(0f, 0f, 0f);
        beltEndPos2.transform.DOLocalRotate(new Vector3(0f, 0f, 0f), .5f);
        beltEndPos2.transform.GetChild(1).gameObject.SetActive(false);
        scriptIndex++;
        StartCoroutine(NextScript());
    }
}