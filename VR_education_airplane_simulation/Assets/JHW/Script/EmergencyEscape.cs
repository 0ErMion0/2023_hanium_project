using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public partial class EmergencyEscape : MonoBehaviour
{
    // ������ �ڸ� ��ũ��Ʈ �ε��� ��ȣ
    static private int MAX_SCRIPT_INDEX = 32;

    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;

    // �� ��ġ üũ
    [SerializeField] GameObject leftHand, rightHand;
    [SerializeField] GameObject leftHandPos, rightHandPos;

    // �ǽ� ������Ʈ��
    [SerializeField] GameObject baggage; // �� ������Ʈ
    [SerializeField] GameObject belt; // ��Ʈ ������Ʈ
    [SerializeField] GameObject beltEnd1; // ��Ʈ �� ������Ʈ
    [SerializeField] GameObject beltEnd2; // ��Ʈ �� ������Ʈ
    [SerializeField] GameObject dropBaggage; // �ٴڿ� �������� �� ���� �� ������Ʈ
    [SerializeField] GameObject originBaggagePos; // �ٴڿ� �������� �� ���� ��ġ
    [SerializeField] GameObject Door; // �� ����
    [SerializeField] GameObject curtain;
    [SerializeField] GameObject pathUX;
    [SerializeField] GameObject terrain_forest;
    [SerializeField] GameObject terrain_water;
    [SerializeField] GameObject slide;
    [SerializeField] GameObject slide_endPos;
    [SerializeField] GameObject boat;
    [SerializeField] GameObject redBox;
    [SerializeField] GameObject boat_destination;

    // ������ȯ - �÷��̾� ��Ʈ�ѷ� ����
    [SerializeField] GameObject playerController1;
    [SerializeField] GameObject playerController2;
    [SerializeField] GameObject scriptReference; // ��ũ��Ʈ ���۷���

    // �ڸ� ������ư
    [SerializeField] GameObject nextButton;

    // �¹���
    [SerializeField] GameObject stewardess;

    // �߰��߰��� ���� �̹�����
    [SerializeField] GameObject image1;
    [SerializeField] GameObject image2;
    [SerializeField] GameObject image3;
    [SerializeField] GameObject image4;
    [SerializeField] GameObject image5;
    [SerializeField] GameObject image6;
    [SerializeField] GameObject image7;

    int scriptIndex = 0;
    Vector3 tempPos;

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
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��� ũ������ �� ��������Ʈ ����
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear); 
        stewardess.GetComponent<Animator>().SetBool("Talk", false); // �̹� true�� �����Ǿ��ִ� ��찡 �־ false�� ���� ���� true�� ����
        stewardess.GetComponent<Animator>().SetBool("Talk", true);
        stewardess.GetComponent<Stewardess>().RandomTalkAnimation(scriptIndex == MAX_SCRIPT_INDEX); // Talk �ִϸ��̼� ��������


        switch (scriptIndex)
        {
            case 2:
                image1.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 3:
                image1.SetActive(false);
                image2.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 4:
                image2.SetActive(false);
                image3.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 6:
                image3.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 9: // �ǽ� - ��Ʈ ���� ����
                belt.SetActive(true);
                break;
            case 11: // �ǽ� - �� ������
                baggage.SetActive(true);
                belt.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(false);
                break;
            case 12: // �ǽ� - ��� �̵�
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
            case 13: // �������� - ��� �̵����� �� ���Ż�ⱸ ��������
                // �÷��̾� ���� ����
                playerController1.SetActive(true);
                playerController2.SetActive(false);

                // �ڸ� ��ġ ���Ҵ� �� �缳��
                scriptText = originText;
                localizeStringEvent = originLocalize;
                scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");

                // �¹����� ��� �� ����
                //DoorOpenAnim();
                
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 15:
                image5.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 16: // �ǽ� - ���� �׸��� ���� �ٸ��� ���� ������ �� ���� ������ �ǵ��� �ϰ� �����̵带 Ÿ�� �������ֽñ� �ٶ��ϴ�.
                image5.SetActive(false);
                image6.SetActive(true);
                leftHandPos.SetActive(true);
                rightHandPos.SetActive(true);
                StartCoroutine(CheckHandPos());
                break;
            case 17: // �����̵带 Ÿ�� �������ٸ� ���� �°��� Ż���� �� �� �ֵ��� �����̵忡�� ������ �ָ� �����ֽñ� �ٶ��ϴ�.
                image6.SetActive(false);
                tempPos = playerController1.transform.position; // �÷��̾� ���� ��ġ
                // �����̵� Ÿ�� ��������, �̵��Ϸ�� �����ڸ���ư Ȱ��ȭ
                playerController1.transform.DOMove(slide_endPos.transform.position, 6f).SetDelay(2f).OnComplete(() => { nextButton.SetActive(true); }).SetEase(Ease.InOutQuad);

                break;
            case 19: // �� ���� �����ϰ� �ȴٸ� �¹����� ������ ���� �ȳ��մϴ�. ������ ��Ǯ��!�� ����������!�� ���� ��������!�� ���ɾ�, �ڼ� ����!��

                // �ٽ� �÷��̾� ȭ�� ������ġ��
                playerController1.transform.position = tempPos;

                // --- ������Ʈ Ȱ��/��Ȱ��ȭ ---
               
                // ���Ż��
                terrain_water.SetActive(true);// �� ���� Ȱ��ȭ
                terrain_forest.SetActive(false);// ���� ���� ��Ȱ��ȭ
                // �������
                boat.SetActive(true);// ��Ʈ Ȱ��ȭ
                slide.SetActive(false);// �����̵� ��Ȱ��ȭ
                // �¹��� ��ġ
                stewardess.transform.position = new Vector3(boat_destination.transform.position.x-2f, boat_destination.transform.position.y-0.75f, boat_destination.transform.position.z+.19f);
                stewardess.transform.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));

                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 20:
                image7.SetActive(true);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 21:
                image7.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 22: // �ǽ� - ����� ���� ������ �Ӹ��� ���� �ʵ��� �� �����̵��� �������� �̵��� �ֽʽÿ�.

                redBox.SetActive(true);
                playerController1.transform.DOMove(boat_destination.transform.position, 7f).SetDelay(5f).OnComplete( () => { nextButton.SetActive(true); });
                break;

            case 23:

                redBox.SetActive(false);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f);
                nextButton.SetActive(true);
                break;
            case 32:// ����ȭ�� ���� - �����ϼ̽��ϴ�. ���� �޴��� �̵��ϰڽ��ϴ�.
                PlayerPrefs.SetInt("Chapter7", 1); // Ŭ���� ���� ����
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
        DoorOpenAnim(_obj);
    }

    // ��Ʈ grab -> ��Ʈ ����
    public void BeltSelectEntered()
    {
        beltEnd2.GetComponent<XRGrabInteractable>().enabled = false;

        // ��Ʈ UX
        beltEnd1.transform.DOLocalMove(new Vector3(0, beltEnd1.transform.localPosition.y, 0), .5f);
        beltEnd1.transform.DOLocalRotate(Vector3.zero, .5f);
        beltEnd2.transform.DOLocalMove(new Vector3(0, beltEnd1.transform.localPosition.y, 0), .5f);
        beltEnd2.transform.DOLocalRotate(Vector3.zero, .5f);

        // UX canvas off
        beltEnd2.transform.GetChild(0).gameObject.SetActive(false);

        // ���� ��ũ��Ʈ��
        scriptIndex++;
        StartCoroutine(NextScript());
    }

    // �� ���� �ִϸ��̼�
    private void DoorOpenAnim(GameObject _obj)
    {
        // �¹��� �̵�
        stewardess.transform.DOLocalRotate(new Vector3(0,270f,0),0f);
        _obj.SetActive(false);
        stewardess.transform.DOMove(new Vector3(_obj.transform.position.x-.3f,stewardess.transform.position.y,_obj.transform.position.z), 0f).OnComplete(() =>
        {
            // �� �ִϸ��̼�
            Vector3 DoorLocalPos = Door.transform.localPosition;
            stewardess.GetComponent<Animator>().SetBool("Talk", false);
            stewardess.GetComponent<Animator>().SetBool("Open", true);
            Door.transform.DOLocalMove(new Vector3(DoorLocalPos.x - 0.3f, DoorLocalPos.y, DoorLocalPos.z), 0.5f).SetDelay(2f).OnComplete(() =>
            {
                Door.transform.DOLocalRotate(new Vector3(0f,0f,-180f), 5f).OnComplete(() =>
                {
                    Destroy(_obj);
                    playerController1.transform.position = new Vector3(_obj.transform.position.x, _obj.transform.position.y, _obj.transform.position.z);
                    pathUX.SetActive(false);
                    playerController1.transform.localRotation = Quaternion.Euler(new Vector3(0, -90f, 0)); // ī�޶� �����̴��� ���ϵ���

                    // ��Ʃ�� ��ġ
                    stewardess.transform.position = new Vector3(slide_endPos.transform.position.x-2f, slide_endPos.transform.position.y-1.35f, slide_endPos.transform.position.z); 
                    stewardess.transform.localRotation = Quaternion.Euler(new Vector3(0, 90f, 0));

                    scriptIndex++;
                    StartCoroutine(NextScript());
                });
            });
        });
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

partial class EmergencyEscape
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