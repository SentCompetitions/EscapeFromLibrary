using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizLock : Interactable
{
    [SerializeField] private bool useAnimation;
    public string animationTriggerName;

    public string question;
    public QuizType quizType;

    public List<QuizSelectQuestion> quizSelectQuestions;
    public string writeQuestionRightAnswer;

    public GameObject quizSelectPrefab;
    public GameObject answerPrefab;

    private Animator _animator;
    private HingeJoint _joint;
    private GameObject _quizObject;

    private static readonly int ShowMobileUI = Animator.StringToHash("ShowMobileUI");
    private static readonly int ShowQuiz = Animator.StringToHash("ShowQuiz");

    private void Start()
    {
        if (useAnimation) TryGetComponent(out _animator);

        transform.Find("Door").TryGetComponent(out _joint);
        var limits = new JointLimits();
        limits.max = 0;
        limits.min = 0;
        if (_joint) _joint.limits = limits;
    }

    public override void OnInteract(Player player)
    {
        switch (quizType)
        {
            case QuizType.Select:
                _quizObject = Instantiate(quizSelectPrefab, Vector3.zero, Quaternion.identity, player.uiQuiz.transform);
                _quizObject.GetComponent<RectTransform>().position = Vector3.zero;
                _quizObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                _quizObject.transform.Find("Text").GetComponent<Text>().text = question;
                Transform quizQuestionsParent = _quizObject.transform.Find("Question");

                foreach (var newQuestion in quizSelectQuestions)
                {
                    GameObject newButton = Instantiate(answerPrefab, Vector3.zero, Quaternion.identity, quizQuestionsParent);
                    newButton.GetComponentInChildren<Text>().text = newQuestion.question;
                    if (newQuestion.isTrue)
                    {
                        newButton.GetComponent<Button>().onClick.AddListener(delegate { RightAnswerSelected(player); });
                    }
                    else
                    {
                        Image image = newButton.GetComponent<Image>();
                        newButton.GetComponent<Button>().onClick.AddListener(delegate { WrongAnswerSelected(player, image); });
                    }
                }

                break;
        }

        player.quizLock = this;

        Animator animator = player.ui.GetComponent<Animator>();
        animator.SetBool(ShowMobileUI, false);
        animator.SetBool(ShowQuiz, true);
        player.input.SwitchCurrentActionMap("UI");
        player.uiAction.enabled = true;
    }

    public void Close(Player player)
    {
        Animator animator = player.ui.GetComponent<Animator>();
        animator.SetBool(ShowMobileUI, true);
        animator.SetBool(ShowQuiz, false);
        player.input.SwitchCurrentActionMap("Main");
        player.uiAction.enabled = false;

        player.ForceInteract(gameObject);

        Destroy(_quizObject, 1f);
    }

    private void RightAnswerSelected(Player player)
    {
        var limits = new JointLimits();
        limits.max = 0;
        limits.min = 180;
        if (_joint) _joint.limits = limits;

        if (useAnimation) _animator.SetTrigger(animationTriggerName);

        Close(player);
    }

    private void WrongAnswerSelected(Player player, Image image)
    {
        image.color = Color.red;
        Close(player);
    }
}


public enum QuizType
{
    Select,
    Write
}

[Serializable]
public struct QuizSelectQuestion
{
    public string question;
    public bool isTrue;
}