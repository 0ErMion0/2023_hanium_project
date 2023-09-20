using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

partial class SimulationEdu : MonoBehaviour
{    
    // �ڸ�
    [SerializeField] TextMeshProUGUI scriptText;
    [SerializeField] LocalizeStringEvent localizeStringEvent;
    // �ڸ� ������ư
    //[SerializeField] GameObject nextButton;

    // �¹���
    [SerializeField] GameObject stewardess;

    // ����Ʈ���μ���
    [SerializeField] Volume postProcess;


    bool isAirplaneMode = false;

    // Start is called before the first frame update
    void Start()
    {
        NextButtonOnClick();
    }

    // ���� �ڸ� �о����
    int scriptIndex = 0;
    IEnumerator NextScript()
    {
        // ������� ��ư ���������� ��ũ��Ʈ ���
        if (!isButtonClicked && scriptIndex<=9)
        {
            string key = "EduSimulation_script" + scriptIndex;

            localizeStringEvent.StringReference.SetReference("EduSimulation_StringTable", key);
            string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
            TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

            // �ڸ��ٴ� ���� �Ⱥ����ٰ�, ���̰� �� ���� 
            scriptText.transform.parent.gameObject.SetActive(true);

            // �ڸ��� ũ������
            RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
            scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

            scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").SetEase(Ease.Linear);
            stewardess.GetComponent<Stewardess>().RandomTalkAnimation(); // Talk �ִϸ��̼� ��������

            switch (scriptIndex)
            {
                //case 7: // ��Ʈ ����
                //    break;
                //case 8: // ����Ʈ�� �� ���ڱ��� ����� ���� ��ȯ�Ͻñ� �ٶ��ϴ�.
                //    break;

                case 9: // �ȳ� ��
                    yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                    scriptText.transform.parent.gameObject.SetActive(false); // �ڸ��� ��Ȱ��ȭ
                    break;
                default:
                    yield return new WaitForSeconds(scriptValue.Length * 0.1f + 3f);
                    //nextButton.SetActive(true);  // ����ùķ��̼� ������ ��ư ������
                    //scriptText.transform.parent.gameObject.SetActive(false); // �ڸ��� ��Ȱ��ȭ
                    scriptIndex++;
                    StartCoroutine(NextScript());
                    break;
            }
        }
        yield return null;
    }

    // ����ùķ��̼� ������ ��ư �Ⱦ�����
    public void NextButtonOnClick()
    {
        scriptIndex++;

        StartCoroutine(NextScript());
        //nextButton.SetActive(false);

        // ����
        SoundManager.Instance.PlaySFX(SoundManager.SFX_list.button1);
    }
}

partial class SimulationEdu : MonoBehaviour
{
    [Header("== Player ==")]
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PlayerLocomotionSystem;
    [SerializeField] GameObject Player_XR_Origin;
    [SerializeField] GameObject Move_VR_Origin;
    [SerializeField] GameObject Seat_Objects;

    [Header("== Belt ==")]
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

    public void SeatButtonOnClick()
    {
        // �¼��� ������ �÷��̾� �̵� ���߰� ��Ʈ Ȱ��ȭ
        Player.SetActive(false); // �÷��̾� ��� ��Ȱ��ȭ
        PlayerLocomotionSystem.SetActive(false); // ���̽�ƽ �̵� �ȵǰ�
        Seat_Objects.SetActive(false);
        Player_XR_Origin.transform.DOMove(Move_VR_Origin.transform.position,1.5f); // �÷��̾� ī�޶� �¼� ī�޶�� �̵�
        Player_XR_Origin.transform.DOLocalRotate(Vector3.zero, 1.5f);

        belt1.SetActive(true);
        belt2.SetActive(true);
        Destroy(Seat_Objects); // �¼� ������ ��ü���� ���صǴ� ��찡 �־.. �ı�
    }

    public void beltSelectEntered()
    {
        // ��Ʈ ���� ���¿��� ��Ʈ ������ Ǯ��
        if (isLinked)
        {
            beltEndPos2.GetComponent<XRGrabInteractable>().enabled = false;

            if (!isWaterLanding) PlayerMoveToExit_Landing(); // ���Ż���̸� �÷��̾� ��󱸷� �̵� 
            else lifeJacketObject.SetActive(true); // ���� �����ϰ� ���� ����

            beltEndPos2.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);
            beltEndPos1.transform.DOLocalRotate(new Vector3(0, 0, 0), 1f);

            isLinked = false;
        }
        else
        {
            beltLinekdPos3.transform.GetChild(0).gameObject.SetActive(true);

            // ��Ʈ1 ������
            if (beltEndPos1.transform.parent == null)
            {
                //beltEndPos1.transform.GetChild(0).gameObject.SetActive(false);
            }
            // ��Ʈ2 ������
            if (beltEndPos2.transform.parent == null)
            {
                //beltEndPos2.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void beltSelectExited()
    {
        //beltLinekdPos3.transform.GetChild(0).gameObject.SetActive(false);
        if (isLinked) return;
        //Debug.Log(Vector3.Distance(beltEndPos1.transform.position, beltLinekdPos1.transform.position) + " " + Vector3.Distance(beltEndPos2.transform.position, beltLinekdPos2.transform.position));
        if (Vector3.Distance(beltEndPos1.transform.position, beltLinekdPos1.transform.position) > 0.07f) return;
        if (Vector3.Distance(beltEndPos2.transform.position, beltLinekdPos2.transform.position) > 0.07f) return;

        isLinked = true;
        beltEndPos1.GetComponent<XRGrabInteractable>().enabled = false;
        beltEndPos2.GetComponent<XRGrabInteractable>().enabled = false;
        beltEndPos1.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        beltEndPos2.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        beltEndPos1.transform.DOMove(beltLinekdPos1.transform.position, 0.5f);
        beltEndPos2.transform.DOMove(beltLinekdPos2.transform.position, 0.5f);
        beltLinekdPos3.transform.GetChild(0).gameObject.SetActive(false);

        Phone.SetActive(true);
        phoneOff.SetActive(true);
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
        //beltEndPos2.transform.GetChild(1).gameObject.SetActive(false);
        scriptIndex++;
        StartCoroutine(NextScript());
    }
}

partial class SimulationEdu : MonoBehaviour
{
    [Header("== Phone ==")]
    [SerializeField] GameObject Phone;
    [SerializeField] GameObject phoneOff; // �����ִ� �ڵ���
    [SerializeField] GameObject phoneOn; // �����ִ� �ڵ���
    [SerializeField] GameObject phoneUI; // �ڵ��� UI
    [SerializeField] GameObject airplaneButton; // ������� ��ư

    bool isButtonClicked = false;

    // �ڵ��� �������
    public void PhoneSelectEntered()
    {
        // UX On/Off
        //phoneOff.transform.GetChild(0).gameObject.SetActive(false);
        //phoneOff.transform.GetChild(1).gameObject.SetActive(true);
    }

    // �ڵ��� ��������
    public void PhoneSelectExited()
    {
        // �ڵ��� ������ �� ���ڸ���
        phoneOff.transform.rotation = Quaternion.Euler(0, 0, 0);

        // UX On/Off
        //phoneOff.transform.GetChild(0).gameObject.SetActive(true);
        //phoneOff.transform.GetChild(1).gameObject.SetActive(false);
    }

    // �ڵ��� ������
    public void PhoneActivated()
    {
        phoneOff.SetActive(false);
        phoneOn.SetActive(true);
        phoneUI.SetActive(true);
    }

    // ������� on ��ư Ŭ��
    public void PhoneAirplaneOnButton()
    {
        // �̹� ��ư �������¸� ����X
        if (isButtonClicked) return;
        isButtonClicked = true;

        // UX
        airplaneButton.GetComponent<Image>().DOColor(new Color(0f, 1f, .5f, 1), .5f);
        //airplaneButton.transform.GetChild(1).gameObject.SetActive(false);

        Invoke("Phone_OFF", 2f); // 2�� �� �ڵ��� ����
    }

    private void Phone_OFF() 
    {
        // �ڸ��� off
        scriptText.transform.parent.gameObject.SetActive(false);

        // �ó����� ����â ����
        SelectScenarioCanvas.SetActive(true);

        // �ڵ��� ����
        Phone.SetActive(false); 
    }
}

partial class SimulationEdu : MonoBehaviour
{
    [Header("== SelectSimulation ==")]

    // �ó����� ����â
    [SerializeField] GameObject SelectScenarioCanvas;

    // �� �ó������� �ε���
    int scenarioIndex;



    public void SelectScenario_OnClick(int num)
    {
        postProcessManager.Instance.RedLightOn();
        SelectScenarioCanvas.gameObject.SetActive(false);

        scenarioIndex = 1;
        switch (num)
        {
            case 1:
                StartCoroutine(DecompressionScript());
                break;
            case 2:
                isWaterLanding = false;
                LandingObjects.SetActive(true);
                WaterLandingObjects.SetActive(false);
                StartCoroutine(LandingScript());
                break;
            case 3:
                isWaterLanding = true;
                LandingObjects.SetActive(false);
                WaterLandingObjects.SetActive(true);
                StartCoroutine(WaterLandingScript());
                break;

        }
    }

    // �ó����� ���� �Ϸ��
    private void EducationCompleted()
    {
        postProcessManager.Instance.RedLightOff();
        isHandOnLeg = false;
        isLinked = true;

        string key = "SimulationEduCompleted";
        localizeStringEvent.StringReference.SetReference("EduSimulation_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        scriptText.transform.parent.gameObject.SetActive(true);

        // �ڸ��� ũ������
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);

        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));
        scriptText.DOKill();
        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("").OnComplete(()=> {
            Invoke("EducationCompletedUIChange", 5f); // 5�� �� UI ����
        });
    }
    // �ó����� ���� �Ϸ�� UI ����
    private void EducationCompletedUIChange()
    {
        scriptText.transform.parent.gameObject.SetActive(false);
        SelectScenarioCanvas.gameObject.SetActive(true);
    }


    // ���θ޴� �̵�
    public void GoToMainTitle()
    {
        SceneManager.LoadScene("MainTitle");
    }
}

// ȭ�� ��Ȳ
partial class SimulationEdu
{

    [Header("== FireSimulation ==")]
    [SerializeField] GameObject OxygenMasks;
    [SerializeField] GameObject PlayerMask;
    [SerializeField] GameObject GrabUX;
    [SerializeField] GameObject TriggerUX;
    [SerializeField] GameObject PlayerMaskOriginPos;

    // ȭ�� - ��Ҹ���ũ �ó�����
    IEnumerator DecompressionScript()
    {
        string key = "EduSimulation_Decompression" + scenarioIndex;
        localizeStringEvent.StringReference.SetReference("EduSimulation_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��ٴ� ���� �Ⱥ����ٰ�, ���̰� �� ���� 
        scriptText.transform.parent.gameObject.SetActive(true);

        // �ڸ��� ũ������
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
        // �¹����� �Ⱥ��̰� �Ұ�
        stewardess.SetActive(false);
        //stewardess.GetComponent<Animator>().SetBool("Talk", false); // �̹� true�� �����Ǿ��ִ� ��찡 �־ false�� ���� ���� true�� ����
        //stewardess.GetComponent<Animator>().SetBool("Talk", true);
        //stewardess.GetComponent<Stewardess>().RandomTalkAnimation(); // Talk �ִϸ��̼� ��������

        switch (scenarioIndex)
        {
            case 3: // ��Ҹ���ũ ��������, �ڸ� ���
                OxygenMasks.SetActive(true);
                PlayerMask.SetActive(true);
                OxygenMasks.transform.DOLocalMoveY(0f, 5f).SetDelay(1f);
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 2f);
                scriptText.transform.parent.gameObject.SetActive(false); // �ڸ��� ��Ȱ��ȭ
                scenarioIndex++;
                StartCoroutine(DecompressionScript());
                break;
            case 4: // �ȳ� ��
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 2f);
                scriptText.transform.parent.gameObject.SetActive(false); // �ڸ��� ��Ȱ��ȭ
                break;
            default:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 2f);
                scriptText.transform.parent.gameObject.SetActive(false); // �ڸ��� ��Ȱ��ȭ
                scenarioIndex++;
                StartCoroutine(DecompressionScript());
                break;
        }
    }


    // ����ũ ������ ������ ����
    public void MaskSelectEntered()
    {
        // UX On/Off
        GrabUX.SetActive(false);
        TriggerUX.SetActive(true);
    }

    // ����ũ �� ������ ���� ��ġ��
    public void MaskSelectExited()
    {
        // UX On/Off
        GrabUX.SetActive(true);
        TriggerUX.SetActive(false);
    }

    // ����ũ Trigger �� (����ũ �����) ����
    public void MaskActivated(GameObject parent)
    {
        GrabUX.SetActive(false);
        PlayerMask.GetComponent<XRGrabInteractable>().enabled = false;

        PlayerMask.transform.DOLocalRotate(new Vector3(90f, 0f, 0f),2f).OnComplete(() =>
        {
            // ����ũ ���� ��ġ��, ������ ���� ���󺹱�
            OxygenMasks.transform.localPosition = new Vector3(0, 2, 0);
            PlayerMask.SetActive(false);
            PlayerMask.transform.position = PlayerMaskOriginPos.transform.position;
            OxygenMasks.SetActive(false);

            GrabUX.SetActive(true);
            PlayerMask.GetComponent<XRGrabInteractable>().enabled = true;
            PlayerMask.transform.localRotation = Quaternion.Euler(0, 0, 0);

            PlayerMask.transform.parent = parent.transform;
            // ���� �Ϸ�
            EducationCompleted();
        });
    }
}

// ������� ��Ȳ
partial class SimulationEdu
{
    [Header("== LandingSimulation ==")]
    [SerializeField] GameObject LandingObjects;
    [SerializeField] GameObject LeftLegPos;
    [SerializeField] GameObject RightLegPos;
    [SerializeField] GameObject LeftHandPos;
    [SerializeField] GameObject RightHandPos;
    [SerializeField] GameObject LeftHand_posture;
    [SerializeField] GameObject RightHand_posture;
    [SerializeField] GameObject curtain;

    [SerializeField] GameObject MovePos1;
    [SerializeField] GameObject MovePos2;
    [SerializeField] GameObject MovePos3;
    [SerializeField] GameObject MovePos4;
    [SerializeField] GameObject MovePos5;
    [SerializeField] GameObject MovePos6;

    bool isWaterLanding = false;

    bool isHandOnLeg = false;

    // ������� �ó�����
    IEnumerator LandingScript()
    {
        string key = "EduSimulation_Landing" + scenarioIndex;
        localizeStringEvent.StringReference.SetReference("EduSimulation_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��ٴ� ���� �Ⱥ����ٰ�, ���̰� �� ���� 
        scriptText.transform.parent.gameObject.SetActive(true);

        // �ڸ��� ũ������
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
        // �¹����� �Ⱥ��̰� �Ұ�
        stewardess.SetActive(false);

        switch (scenarioIndex)
        {
            case 3: // ��ݹ����ڼ� �˻�
                RightLegPos.SetActive(true);
                LeftLegPos.SetActive(true);
                StartCoroutine(CheckHandPos_Seat_Landing());
                break;
            case 7: // ������ƮǮ�� ��� �̵�
                beltEndPos2.GetComponent<XRGrabInteractable>().enabled = true;
                break;
            case 8: // �� ���ļ� �����̵�Ÿ�� �̵�
                LeftHandPos.SetActive(true);
                RightHandPos.SetActive(true);
                StartCoroutine(CheckHandPos_Exit_Landing());
                break;
            default:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 2f);
                scenarioIndex++;
                StartCoroutine(LandingScript());
                break;
        }
    }

    private void PlayerMoveToExit_Landing()
    {
        curtain.SetActive(false);
        Player_XR_Origin.transform.DOMove(MovePos1.transform.position, 2f).SetDelay(2f).OnComplete(() =>
        {
            Player_XR_Origin.transform.DOMove(MovePos2.transform.position, 2f).OnComplete(() =>
            {
                Player_XR_Origin.transform.DOMove(MovePos3.transform.position, 4f).OnComplete(() =>
                {
                    Player_XR_Origin.transform.DOLocalRotate(new Vector3(0f, -90f, 0f), 1f);
                    Player_XR_Origin.transform.DOMove(MovePos4.transform.position, 3f).OnComplete(() =>
                    {
                        scenarioIndex++;
                        StartCoroutine(LandingScript());
                    });
                });
            });
        });
    }
    public IEnumerator CheckHandPos_Seat_Landing()
    {
        if (!isHandOnLeg)
        {
            if (Vector3.Distance(LeftHand_posture.transform.position, LeftLegPos.transform.position) < .2f &&
                Vector3.Distance(RightHand_posture.transform.position, RightLegPos.transform.position) < .2f)
            {
                isHandOnLeg = true;
                StopCoroutine(CheckHandPos_Seat_Landing());

                LeftLegPos.SetActive(false);
                RightLegPos.SetActive(false);

                scenarioIndex++;
                StartCoroutine(LandingScript());
            }
        }
        yield return new WaitForSeconds(.1f);
        StartCoroutine(CheckHandPos_Seat_Landing());
    }

    IEnumerator CheckHandPos_Exit_Landing()
    {
        //Debug.Log(Vector3.Distance(LeftHandPos.transform.position, LeftHand_posture.transform.position) + ", " + Vector3.Distance(rightHand.transform.position, rightHandPos.transform.position));

        // �� �������� �ֱ������� �˻�
        if (Vector3.Distance(LeftHandPos.transform.position, LeftHand_posture.transform.position) < .15f && Vector3.Distance(RightHandPos.transform.position, RightHand_posture.transform.position) < .15f)
        {
            LeftHandPos.SetActive(false);
            RightHandPos.SetActive(false);

            Player_XR_Origin.transform.DOMove(MovePos5.transform.position, 4f).SetDelay(1f).SetEase(Ease.InOutQuad).OnComplete(() => {
                Player_XR_Origin.transform.DOMove(MovePos6.transform.position, 2f).SetDelay(1f).OnComplete(() =>
                {
                    // ������ ���� ��ü�� ���󺹱�
                    curtain.SetActive(true);

                    beltEndPos1.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                    beltEndPos2.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

                    Player_XR_Origin.transform.position = Move_VR_Origin.transform.position; // �÷��̾� ī�޶� �¼� ī�޶�� �̵�
                    Player_XR_Origin.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    EducationCompleted();
                });
            });
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(CheckHandPos_Exit_Landing());
        }

    }
}

// ������� ��Ȳ
partial class SimulationEdu
{
    [Header("== WaterLandingSimulation ==")]
    [SerializeField] GameObject WaterLandingObjects;
    [SerializeField] GameObject lifeJacketObject;
    [SerializeField] GameObject lifeJacketBag;
    [SerializeField] GameObject lifeJacketModel;
    [SerializeField] GameObject jacktGrabUX;
    [SerializeField] GameObject jacketTriggerUX;
    [SerializeField] GameObject boatPosition;
    [SerializeField] GameObject originLifeJacketPosition;

    // ������� �ó�����
    IEnumerator WaterLandingScript()
    {
        string key = "EduSimulation_WaterLanding" + scenarioIndex;
        localizeStringEvent.StringReference.SetReference("EduSimulation_StringTable", key);
        string scriptValue = localizeStringEvent.StringReference.GetLocalizedString(key);
        TextToSpeach.Instance.SpeechText(scriptValue.Replace('\n', ' '));

        // �ڸ��ٴ� ���� �Ⱥ����ٰ�, ���̰� �� ���� 
        scriptText.transform.parent.gameObject.SetActive(true);

        // �ڸ��� ũ������
        RectTransform rect = scriptText.transform.parent.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 50 + (scriptValue.Split("\n").Length - 1) * 20);
        scriptText.transform.parent.GetComponent<Image>().sprite = stewardess.GetComponent<Stewardess>().textSprites[scriptValue.Split("\n").Length - 1];

        scriptText.DOText(scriptValue, scriptValue.Length * 0.1f).From("");
        // �¹����� �Ⱥ��̰� �Ұ�
        stewardess.SetActive(false);

        switch (scenarioIndex)
        {
            case 3: // ��ݹ����ڼ� �˻�
                RightLegPos.SetActive(true);
                LeftLegPos.SetActive(true);
                StartCoroutine(CheckHandPos_Seat_WaterLanding());
                break;
            case 8: // ��Ʈ Ǯ�� �������
                beltEndPos2.GetComponent<XRGrabInteractable>().enabled = true;
                //lifeJacketObject.SetActive(true);
                break;
            case 9:
                moveToBoat();
                break;
            default:
                yield return new WaitForSeconds(scriptValue.Length * 0.1f + 2f);
                scenarioIndex++;
                StartCoroutine(WaterLandingScript());
                break;
        }
    }

    public IEnumerator CheckHandPos_Seat_WaterLanding()
    {
        if (!isHandOnLeg)
        {
            if (Vector3.Distance(LeftHand_posture.transform.position, LeftLegPos.transform.position) < .2f &&
                Vector3.Distance(RightHand_posture.transform.position, RightLegPos.transform.position) < .2f)
            {
                isHandOnLeg = true;
                StopCoroutine(CheckHandPos_Seat_WaterLanding());

                LeftLegPos.SetActive(false);
                RightLegPos.SetActive(false);

                scenarioIndex++;
                StartCoroutine(WaterLandingScript());
            }
        }
        yield return new WaitForSeconds(.1f);
        StartCoroutine(CheckHandPos_Seat_WaterLanding());
    }

    private void PlayerMoveToExit_WaterLanding()
    {
        curtain.SetActive(false);
        Player_XR_Origin.transform.DOMove(MovePos1.transform.position, 2f).SetDelay(2f).OnComplete(() =>
        {
            Player_XR_Origin.transform.DOMove(MovePos2.transform.position, 2f).OnComplete(() =>
            {
                Player_XR_Origin.transform.DOMove(MovePos3.transform.position, 4f).OnComplete(() =>
                {
                    Player_XR_Origin.transform.DOLocalRotate(new Vector3(0f, -90f, 0f), 1f);
                    Player_XR_Origin.transform.DOMove(MovePos4.transform.position, 3f).OnComplete(() =>
                    {
                        scenarioIndex++;
                        StartCoroutine(WaterLandingScript());
                    });
                });
            });
        });
    }

    private void moveToBoat()
    {
        Player_XR_Origin.transform.DOMove(boatPosition.transform.position,6f).SetEase(Ease.Linear).SetDelay(5f).OnComplete(()=> {
            Ending_WaterLanding();
        });
    }

    private void Ending_WaterLanding()
    {
        // ������ ���� ��ü�� ���󺹱�
        curtain.SetActive(true);

        beltEndPos1.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
        beltEndPos2.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);

        Player_XR_Origin.transform.position = Move_VR_Origin.transform.position; // �÷��̾� ī�޶� �¼� ī�޶�� �̵�
        Player_XR_Origin.transform.localRotation = Quaternion.Euler(Vector3.zero);
        EducationCompleted();
    }

    public void EquipLifeJacket()
    {
        lifeJacketObject.SetActive(false);
        lifeJacketBag.SetActive(true);
        lifeJacketModel.SetActive(false);
        lifeJacketModel.transform.position = originLifeJacketPosition.transform.position;
        PlayerMoveToExit_WaterLanding();
    }
}

// �˾� ����
partial class SimulationEdu
{

    public void popup_reStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void popup_toMainTitle()
    {
        SceneManager.LoadScene("MainTitle");
    }

    public void popup_openPopup(GameObject popup)
    {
        popup.SetActive(true);
    }
    public void popup_exitPopup(GameObject popup)
    {
        popup.SetActive(false);
    }
}