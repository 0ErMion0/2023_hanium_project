using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

partial class lifeJacket : MonoBehaviour
{
    [SerializeField] GameObject equipedJacketModel;
    [SerializeField] GameObject targetObj;

    [SerializeField] GameObject equipedJacket;
    [SerializeField] List<GameObject> beltPos;

    [SerializeField] GameObject BeltGrabUX;
    [SerializeField] GameObject BeltDropUX;
    [SerializeField] GameObject lifeJacketDropPos; // �������� ���ƾ� �� ��ġ
    [SerializeField] GameObject BeltStartPos; // ��Ʈ �տ� ��� ����
    [SerializeField] GameObject BeltEndPos; // ��Ʈ ���� ����
    [SerializeField] GameObject CharacterCenter; // ĳ���� �߽���

    [SerializeField] TextMeshProUGUI txt; // �ڸ�

    // �¹���
    [SerializeField] GameObject stewardess;

    private Vector3 originLifeJacketPos;
    private Vector3 originBeltStartPos;
    private Vector3 originBeltEndPos;



    public void JacketSelectEntered()
    {
        lifeJacketDropPos.SetActive(true);
        jacketGrabUI.SetActive(false);
    }
    public void JacketSelectExited()
    {
        Debug.Log(originLifeJacketPos); // ���� �������� ��ġ
        Debug.Log(jacket_lifeJacketObj.transform.position); // ������ ������ �� ��ǥ

        Debug.Log("�Ÿ� = " + Vector3.Distance(lifeJacketDropPos.transform.position, jacket_lifeJacketObj.transform.position));
        if (Vector3.Distance(lifeJacketDropPos.transform.position, jacket_lifeJacketObj.transform.position) >= 0.17f) // ���ƾ� �� ���� �� ���Ҵٸ� ���� ��ġ��
        {
            jacket_lifeJacketObj.transform.position = originLifeJacketPos;
            jacket_lifeJacketObj.transform.rotation = Quaternion.Euler(0, 90, 0);
            jacketGrabUI.SetActive(true);
            return;
        }
        lifeJacketDropPos.SetActive(false);
        jacket_lifeJacketObj.gameObject.SetActive(false);
        equipedJacket.SetActive(true);
        // �ڸ� ����
        scriptIndex++;
        StartCoroutine(NextScript());
    }
    public void JacketBeltSelectEntered()
    {

        // UX OFF/ON
        BeltGrabUX.SetActive(false);
        BeltDropUX.SetActive(true);

        for (int i = 0; i < CharacterCenter.transform.childCount-1; i++)
        {
            //beltPos[i].transform.position = new Vector3(beltPos[i].transform.position.x, BeltEndPos.transform.position.y, beltPos[i].transform.position.z);
            beltPos[i].transform.DOMove(CharacterCenter.transform.GetChild(i).position, .5f);
        }
        // ������ ���� ��ġ �ľ��ؼ� ��Ʈ ������
        //Vector3 startPos = beltPos[0].transform.position;
        //Vector3 destPos = beltPos[beltPos.Count-1].transform.position;
        //Vector3 diff = startPos - destPos;
    }

    public void JacketBeltSelectExited()
    {
        Debug.Log("�Ÿ� = " + Vector3.Distance(BeltEndPos.transform.position, BeltStartPos.transform.position));
        if (Vector3.Distance(BeltEndPos.transform.position, BeltStartPos.transform.position) >= 0.15f) // ���ƾ� �� ���� �� ���Ҵٸ� ���� ��ġ��
        {
            BeltStartPos.transform.position = originBeltStartPos;
            BeltStartPos.transform.rotation = Quaternion.Euler(180, 0, 90);
            BeltGrabUX.SetActive(true); 
            BeltDropUX.SetActive(false);
            return;
        }
        BeltGrabUX.SetActive(true);
        BeltDropUX.SetActive(false);

        // ��ȣ�ۿ� �Ұ����ϰ� ����
        BeltStartPos.transform.GetComponent<XRGrabInteractable>().enabled = false;
        BeltStartPos.transform.GetChild(1).gameObject.SetActive(false);

        for (int i = 0; i < CharacterCenter.transform.childCount; i++)
        {
            //beltPos[i].transform.position = new Vector3(beltPos[i].transform.position.x, BeltEndPos.transform.position.y, beltPos[i].transform.position.z);
            beltPos[i].transform.DOMove(CharacterCenter.transform.GetChild(i).position, .5f);
        }
        beltPos[CharacterCenter.transform.childCount - 1].transform.rotation = Quaternion.Euler(180, 0, 90);

        // ���� �ڸ�����
        scriptIndex=5;
        StartCoroutine(NextScript());
    }
}

partial class lifeJacket
{

    [Header("=== ��� �������� ===")]
    [SerializeField] GameObject npcObj;
    [SerializeField] GameObject xrOriginObj;
    [SerializeField] Transform npcMovePos; // npc �̵���ġ
    [SerializeField] Transform xrOriginMovePos; // xr �̵���ġ

    [SerializeField] GameObject leftHandle; // ���� ������
    [SerializeField] GameObject rightHandle; // ������ ������
    [SerializeField] GameObject jacketTube; // ��Ŷ Ʃ��

    bool isRightSelected;
    bool isLeftSelected;
    bool isAlreadyInflated = false;

    public void leftHandleSelected()
    {
        isLeftSelected = true;
        if (isRightSelected && !isAlreadyInflated)
        {
            isAlreadyInflated = true; InflateLifeJacket();
        }
        leftHandle.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void rightHandleSelected()
    {
        isRightSelected = true;
        if (isLeftSelected && !isAlreadyInflated)
        {
            isAlreadyInflated = true; InflateLifeJacket();
        }
        rightHandle.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void leftHandleExited()
    {
        isLeftSelected = false;
        leftHandle.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void rightHandleExited()
    {
        isRightSelected = false;
        rightHandle.transform.GetChild(0).gameObject.SetActive(true);
    }


    private void InflateLifeJacket()
    {
        leftHandle.transform.parent.DOLocalMoveY(1.2f, 0.5f);
        jacketTube.GetComponent<Transform>().DOScale(130f, .5f);
        leftHandle.SetActive(false);
        rightHandle.SetActive(false);
        

        scriptIndex++;
        StartCoroutine(NextScript()); // ���� ��ũ��Ʈ��
    }


}

public partial class lifeJacket {
    // ������ �ڸ� ��ũ��Ʈ �ε��� ��ȣ
    static private int MAX_SCRIPT_INDEX = 12;

    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

    // ���� �ڸ� �о����
    public int scriptIndex = 0;

    public IEnumerator NextScript()
    {
        string key = "lifeJacket_script" + scriptIndex;

        localizeStringEvent.StringReference.SetReference("LifeJacket_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(scriptIndex==MAX_SCRIPT_INDEX); // Talk �ִϸ��̼� ��������

        switch (scriptIndex)
        {
            case 2:
                jacketBag.SetActive(true);
                break;
            case 3: // �������� ���� - �����ڸ� ��ư �߸� �ȵ�
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                break;
            case 4: // �������� ��Ʈ�ű� - �����ڸ� ��ư �߸� �ȵ�
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                break;
            case 5: // ��� �̵�
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                // �÷��̾�, npc, �¹��� �̵�
                npcObj.transform.GetComponent<Transform>().position = npcMovePos.position;
                xrOriginObj.transform.GetComponent<Transform>().position = xrOriginMovePos.position;
                stewardess.transform.position = new Vector3(-1.476f, 5.451f, 21.03f);
                GameObject.Find("lifejacketManager").GetComponent<lifeJacket>().scriptIndex = 6; // �ϵ��ڵ� �˼��մϴ�... ������ lifejacket ������Ʈ�� ������Ʈ �ϳ��� �����ؾ� �Ǵµ� �Ǽ��� �ٸ������� �����ؼ� ������Ʈ�� �ΰ���..  GameObject.Find ����ϴ� �˼��մϴ�!!
                StartCoroutine(GameObject.Find("lifejacketManager").GetComponent<lifeJacket>().NextScript());
                break;
            case 7: // ���� ��Ǯ����
                leftHandle.SetActive(true);
                rightHandle.SetActive(true);
                break;
            case 8: // ��Ǯ���� �Ϸ�
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 12:
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

    private void Start()
    {
        originLifeJacketPos = this.transform.GetChild(0).position;
        originBeltStartPos = BeltStartPos.transform.position;
        originBeltEndPos = BeltEndPos.transform.position;

        // ���������� �������� ��������, �°����� ���� �������� �� ���� �ִµ�, �������� ���������� ���� ���� �ڸ� ���� X
        if(this.name != "equipedJacket")NextButtonOnClick();
    }
}

public partial class lifeJacket
{
    [SerializeField] GameObject jacket_model;
    [SerializeField] GameObject jacket_grabUI;
    [SerializeField] GameObject jacket_triggerUI;
    [SerializeField] GameObject jacket_originPosObj;
    [SerializeField] GameObject jacket_lifeJacketObj;
    [SerializeField] GameObject jacketBag;
    [SerializeField] GameObject jacketGrabUI;

    public void JacketBagSelected()
    {
        jacket_grabUI.SetActive(false);
        jacket_triggerUI.SetActive(true);
    }

    public void JacketBagExited()
    {
        jacket_grabUI.SetActive(true);
        jacket_triggerUI.SetActive(false);
        jacket_model.gameObject.transform.position = jacket_originPosObj.transform.position;
        //jacket_model.gameObject.transform.eulerAngles = Vector3.zero;
    }

    public void JacketBagTriggered()
    {
        jacket_model.SetActive(false);
        jacket_lifeJacketObj.SetActive(true);
        jacketBag.SetActive(false);

        // �ڸ� ����
        scriptIndex++;
        StartCoroutine(NextScript());
    }
}

