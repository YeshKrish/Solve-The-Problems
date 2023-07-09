using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private List<GameObject> _tasks = new List<GameObject>();

    [SerializeField]
    private GameObject _gameCompleted;
    [SerializeField]
    private TMPro.TMP_Text _mark;    
    [SerializeField]
    private TMPro.TMP_Text _percentage;

    private int _finalLevel = 4;
    private float _total=0f;
    private float _totalMarks=33f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        _tasks[0].SetActive(true);
    }

    private void OnEnable()
    {
        VerificationColor.moveNextLevel += MoveNextLevel;
        VerificationChecbox.moveNextLevel += MoveNextLevel;
        Verification.moveNextLevel += MoveNextLevel;
    }
    public void MoveNextLevel(int level, int score)
    {
        Debug.Log("Level Score" + score);
        _total += score;
        string per= ((_total / _totalMarks) * 100).ToString();
        Debug.Log("Totalll" + _total);

        if (level < _finalLevel)
        {
            _tasks[level-1].SetActive(false);
            _tasks[level].SetActive(true);
        }
        else if(level == _finalLevel)
        {
            _tasks[level-1].SetActive(false);
            _gameCompleted.SetActive(true);
        }

        if (_gameCompleted.activeInHierarchy)
        {
            _percentage.text = per;
            _mark.text = _total.ToString();
        }
    }

    private void OnDisable()
    {
        VerificationColor.moveNextLevel -= MoveNextLevel;
        VerificationChecbox.moveNextLevel -= MoveNextLevel;
        Verification.moveNextLevel -= MoveNextLevel;

    }

    public void Quit()
    {
        Application.Quit();
    }
}
