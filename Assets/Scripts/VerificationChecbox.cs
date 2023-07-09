using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class VerificationChecbox : MonoBehaviour
{
    [SerializeField]
    private List<ToggleGroup> _toggleGroups = new List<ToggleGroup>();
    [SerializeField]
    private TMP_InputField _verbInput;
    [SerializeField]
    private TMP_Text _errorText;
    [SerializeField]
    private TMP_Text _successText;
    [SerializeField]
    private TMP_Text _resolveText;

    public TaskAnswers TasksAnswers;

    private int _totalCorrectAnswers;
    private bool isVerb;
    private bool _isVerbGiven;
    private List<ToggleGroup> _errorFields = new List<ToggleGroup>();
    private List<ToggleGroup> _successFields = new List<ToggleGroup>();

    public static event Action<int, int> moveNextLevel;

    private void Start()
    {
        _totalCorrectAnswers = _toggleGroups.Count;
    }

    public void CheckToggleGroups()
    {
        if ((_errorFields.Count > 0 || !isVerb) && _isVerbGiven)
        {
            _resolveText.enabled = true;
        }
        else
        {
            _resolveText.enabled = false;
        }

        if (string.IsNullOrEmpty(_verbInput.text))
        {
            _isVerbGiven = false;
            _errorText.enabled = true;
        }
        else
        {
            _isVerbGiven = true;
            isVerb = VerbChecker(_verbInput.text);
            if (isVerb)
            {
                _verbInput.textComponent.color = Color.green;
                _verbInput.interactable = false;
            }
            else
            {
                _verbInput.textComponent.color = Color.red;
                if(_errorText.enabled == true)
                {
                    _errorText.enabled = false;
                }
            }
        }


        for(int i = 0; i < _toggleGroups.Count; i++)
        {

            if (AreBothTogglesNotActive(_toggleGroups[i]))
            {
                _errorText.enabled = true;
            }
            else
            {

                Toggle activeToggle = GetActiveToggle(_toggleGroups[i]);


                Debug.Log("actiber" + " " +activeToggle.name);

                Debug.Log(activeToggle.transform.name + " " + TasksAnswers.Answers[i]);

                if (activeToggle.transform.name == TasksAnswers.Answers[i])
                {
                    for (int l = 0; l < _errorFields.Count; l++)
                    {
                        Toggle activeErrorToggle = GetActiveToggle(_errorFields[l]);
                        Toggle inActiveErrorToggle = GetInactiveToggle(_errorFields[l]);
                        if(inActiveErrorToggle != null)
                        {
                            Image errorBackground = inActiveErrorToggle.targetGraphic as Image;
                            errorBackground.color = Color.clear;
                        }

                        if (activeToggle == activeErrorToggle)
                        {
                            _errorFields.Remove(_errorFields[l]);
                        }

                    }

                    if (_successFields.Count > 0)
                    {
                        for (int j = 0; j < _successFields.Count; j++)
                        {
                            if (!_successFields.Contains(_toggleGroups[i]))
                            {
                                _successFields.Add(_toggleGroups[i]);
                                DisableAnswerChange(_toggleGroups[i]);
                            }
                            Image background = GetActiveToggle(_successFields[j]).targetGraphic as Image;
                            background.color = Color.green;
                        }
                    }
                    else
                    {
                        _successFields.Add(_toggleGroups[i]);
                        DisableAnswerChange(_toggleGroups[i]);
                    }
                }
                else
                {
                    _errorFields.Add(_toggleGroups[i]);
                    for (int k = 0; k < _errorFields.Count; k++)
                    {
                        Image background = GetActiveToggle(_errorFields[k]).targetGraphic as Image;
                        background.color = Color.red;
                    }
                }
            }
        }

        Debug.Log("Sucxc" + _successFields.Count);
        if (_successFields.Count == _totalCorrectAnswers && _errorFields.Count == 0 && isVerb)
        {
            if (_resolveText.enabled == true || _errorText.enabled == true)
            {
                _resolveText.enabled = false;
                _errorText.enabled = false;
            }
            _successText.enabled = true;
        }
    }

    bool AreBothTogglesNotActive(ToggleGroup toggleGroup)
    {
        List<Toggle> activeToggles = toggleGroup.ActiveToggles().ToList();
        Debug.Log("acar" + activeToggles.Count);
        return activeToggles.Count == 0;
    }

    Toggle GetActiveToggle(ToggleGroup toggleGroup)
    {
        return toggleGroup.ActiveToggles().FirstOrDefault();
    }

    Toggle GetInactiveToggle(ToggleGroup toggleGroup)
    {
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();

        for (int i = 0; i < toggles.Length; i++)
        {
            if (!toggles[i].isOn)
            {
                return toggles[i];
            }
        }

        return null;
    }

    private void DisableAnswerChange(ToggleGroup toggleGroup)
    {
        int children = toggleGroup.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            toggleGroup.transform.GetChild(i).GetComponent<Toggle>().interactable = false;
        }
    }

    private bool VerbChecker(string verb)
    {
        verb = verb.ToLower();
        string lastThreeCharacters;
        if (verb.Length > 3)
        {
            lastThreeCharacters = verb.Substring(verb.Length - 3);
            if (lastThreeCharacters == "ing")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    public void Submit()
    {
        if (isVerb)
        {
            int mark = _successFields.Count + 1;
            moveNextLevel?.Invoke(int.Parse(transform.name), mark);
        }
        else
        {
            int mark = _successFields.Count;
            moveNextLevel?.Invoke(int.Parse(transform.name), mark);
        }
       
    }
}