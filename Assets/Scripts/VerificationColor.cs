using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VerificationColor : MonoBehaviour
{
    [SerializeField]
    List<Image> _colorFields = new List<Image>();
    [SerializeField]
    private TMP_Text _errorText;
    [SerializeField]
    private TMP_Text _successText;
    [SerializeField]
    private TMP_Text _resolveText;

    public TaskAnswers TasksAnswers;

    private int _totalCorrectAnswers;
    private List<Image> _errorFields = new List<Image>();

    private List<Image> _successFields = new List<Image>();

    [SerializeField]
    private GameObject _colorPanel;

    private Image _clickedImage;

    public static event Action<int,int> moveNextLevel;

    private void Start()
    {
        _totalCorrectAnswers = _colorFields.Count;
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
                _successFields[i].transform.parent.GetComponent<Image>().raycastTarget = false;
                _successFields[i].raycastTarget = false;
            }
        }
        
    }

    public void ColorMatch(Image image)
    {
        _clickedImage = image;
        _colorPanel.SetActive(true);
    }

    public void PutColor(string color)
    {
        if (_colorPanel.activeInHierarchy)
        {
            _colorPanel.SetActive(false);
        }
        Color baloonColor;
        ColorUtility.TryParseHtmlString(color, out baloonColor);
        _clickedImage.color = baloonColor;
    }
    public void Verify()
    {


        for (int i = 0; i < _colorFields.Count; i++)
        {
            if (_colorFields[i].color == Color.white)
            {
                _errorText.enabled = true;
            }
            else
            {
                string colorCode = ColorUtility.ToHtmlStringRGB(_colorFields[i].color);
                Debug.Log("Color Code "+ colorCode + "Ans " + TasksAnswers.Answers[i]);
                if (colorCode == TasksAnswers.Answers[i])
                {
                    for (int l = _errorFields.Count - 1; l >= 0; l--)
                    {
                        string errorColorCode = ColorUtility.ToHtmlStringRGB(_errorFields[l].color);
                        if (colorCode == errorColorCode && _colorFields[i].transform.name == _errorFields[l].transform.name)
                        {
                            _errorFields.RemoveAt(l);
                        }
                    }

                    if (_successFields.Count > 0)
                    {
                        for (int j = 0; j < _successFields.Count; j++)
                        {
                            if (!_successFields.Contains(_colorFields[i]))
                            {
                                _successFields.Add(_colorFields[i]);
                            }
                            //_successFields[j].textComponent.color = Color.green;
                        }
                    }
                    else
                    {
                        _successFields.Add(_colorFields[i]);
                    }
                }
                else
                {
                    Debug.Log("Error" + _colorFields[i].transform.name);
                    _errorFields.Add(_colorFields[i]);
                }
            }
        }

        //for (int m = 0; m < _successFields.Count; m++)
        //{
        //    Debug.Log("Succes values" + _successFields[m].text);
        //}

        Debug.Log("Success Fileds" + _successFields.Count + "total" + _totalCorrectAnswers);
        if (_successFields.Count == _totalCorrectAnswers && _errorFields.Count == 0)
        {
            if (_resolveText.enabled == true || _errorText.enabled == true)
            {
                _resolveText.enabled = false;
                _errorText.enabled = false;
            }
            _successText.enabled = true;
            
        }
        //else if (_errorFields.Count > 0)
        //{
        //    for (int k = 0; k < _errorFields.Count; k++)
        //    {
        //        _errorFields[k].textComponent.color = Color.red;
        //    }
        //}

        Debug.Log("succes" + _successFields.Count);
    }

    public void Submit()
    {

        moveNextLevel?.Invoke(int.Parse(transform.name), _successFields.Count);
    }
}
