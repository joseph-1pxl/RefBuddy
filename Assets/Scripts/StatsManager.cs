using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private Text _scoreText = null;    

    [SerializeField]
    private StatsRecord _statsRecord;

    [Header("Teams Stats")]
    [SerializeField]
    private Transform[] _teamsStats;

    [Header("Halfs")]
    [SerializeField]
    private Image[] _half1Buttons;
    [SerializeField]
    private Image[] _half2Buttons;

    [Header("Sprites")]
    [SerializeField]
    private Sprite _goalSprite;
    [SerializeField]
    private Sprite _yellowCardSprite;
    [SerializeField]
    private Sprite _redCardSprite;
    [SerializeField]
    private Sprite[] _normalSprites;
    [SerializeField]
    private Sprite[] _highlightedSprites;

    private int _half = 0;
    private int[] _teamScores = new int[2];

    private void Start()
    {
        SetHalftime(0);
    }

    public void RecordGoal(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var goalRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        goalRecord.SetCategory(_goalSprite);                
        goalRecord.SetTime(_timer.GetCurrentTime());
        goalRecord.SetTeam(teamId);
        goalRecord.GoalRemoved += OnGoalRemoved;

        if(teamId >= 0 && teamId <= 1)
        {
            _teamScores[teamId]++;
            _scoreText.text = string.Format("{0} : {1}", _teamScores[0], _teamScores[1]);
        }
    }

    private void OnGoalRemoved(int teamId)
    {
        if (teamId >= 0 && teamId <= 1)
        {
            _teamScores[teamId]--;
            _scoreText.text = string.Format("{0} : {1}", _teamScores[0], _teamScores[1]);
        }
    }

    public void RecordYellowCard(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var yellowCardRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        yellowCardRecord.SetCategory(_yellowCardSprite);
        yellowCardRecord.SetTime(_timer.GetCurrentTime());
    }

    public void RecordRedCard(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var redCardRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        redCardRecord.SetCategory(_redCardSprite);
        redCardRecord.SetTime(_timer.GetCurrentTime());
    }

    public void SetHalftime(int half)
    {
        _half = half;

        for (int i = 0; i < _half1Buttons.Length; i++)
        {
            _half1Buttons[i].sprite = half == 0 ? _highlightedSprites[i] : _normalSprites[i];
            _half2Buttons[i].sprite = half == 1 ? _highlightedSprites[i] : _normalSprites[i];
        }

        for (int i = 0; i < 2; i++)
        {
            _teamsStats[i].GetComponent<CanvasGroup>().alpha = half == 0 ? 1 : 0.65f;
        }

        for (int i = 2; i < 4; i++)
        {
            _teamsStats[i].GetComponent<CanvasGroup>().alpha = half == 1 ? 1 : 0.65f;
        }
    }
}
