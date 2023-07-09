using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Verification : MonoBehaviour
{
    [SerializeField]
    List<TMP_InputField> _inputFields = new List<TMP_InputField>();
    [SerializeField]
    private TMP_Text _errorText;
    [SerializeField]
    private TMP_Text _successText;
    [SerializeField]
    private TMP_Text _resolveText;

    public TaskAnswers TasksAnswers;

    private int _totalCorrectAnswers;
    private List<TMP_InputField> _errorFields = new List<TMP_InputField>();

    private List<TMP_InputField> _successFields = new List<TMP_InputField>();

    public static event Action<int, int> moveNextLevel;

    private void Start()
    {
        _totalCorrectAnswers = _inputFields.Count;
    }

    private void Update()
    {
        if(_errorFields.Count > 0)
        {
            _resolveText.enabled = true;
        }
        else
        {
            _resolveText.enabled = false;
        }

    }

    private void LateUpdate()
    {
        if (_successFields.Count > 0)
        {
            for (int i = 0; i < _successFields.Count; i++)
            {
                _successFields[i].interactable = false;
            }
        }
        
    }

    public void Verify()
    {


        for (int i = 0; i < _inputFields.Count; i++)
        {
            if (string.IsNullOrEmpty(_inputFields[i].text))
            {
                _errorText.enabled = true;
            }
            else
            {
                if (_inputFields[i].text.ToLower() == TasksAnswers.Answers[i])
                {
                    for(int l = 0; l < _errorFields.Count; l++)
                    {
                        if (_inputFields[i].text == _errorFields[l].text)
                        {
                            _errorFields.Remove(_errorFields[l]);
                        }

                    }

                    if(_successFields.Count > 0)
                    {
                        for (int j = 0; j < _successFields.Count; j++)
                        {
                            if (!_successFields.Contains(_inputFields[i]))
                            {
                                _successFields.Add(_inputFields[i]);
                            }
                            _successFields[j].textComponent.color = Color.green; 
                        }
                    }
                    else
                    {
                        _successFields.Add(_inputFields[i]);
                    }
                }
                else
                {
                    _errorFields.Add(_inputFields[i]);
                }
            }
        }

        for (int m = 0; m < _successFields.Count; m++)
        {
            Debug.Log("Succes values" + _successFields[m].text);
        }


        if (_successFields.Count == _totalCorrectAnswers && _errorFields.Count == 0)
        {
            if (_resolveText.enabled == true || _errorText.enabled == true)
            {
                _resolveText.enabled = false;
                _errorText.enabled = false;
            }
            _successText.enabled = true;
        }
        else if (_errorFields.Count > 0)
        {
            for (int k = 0; k < _errorFields.Count; k++)
            {
                _errorFields[k].textComponent.color = Color.red;
            }
        }

        Debug.Log("succes" + _successFields.Count);
    }

    public void Submit()
    {
        moveNextLevel?.Invoke(int.Parse(transform.name), _successFields.Count);
    }
}
