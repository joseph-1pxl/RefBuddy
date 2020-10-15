using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private Text _scoreText = null;    

    [SerializeField]
    private StatsRecord _statsRecord = null;

    [SerializeField]
    private RectTransform _matchInfoRect = null;
    [SerializeField]
    private RectTransform _buttonsRect = null;

    [Header("Teams Stats")]
    [SerializeField]
    private TeamManager _teamManager = null;
    [SerializeField]
    private Transform[] _teamsStats = null;

    [Header("Halfs")]
    [SerializeField]
    private Image[] _half1Buttons = null;
    [SerializeField]
    private Image[] _half2Buttons = null;

    [Header("Sprites")]
    [SerializeField]
    private Sprite _goalSprite = null;
    [SerializeField]
    private Sprite _yellowCardSprite = null;
    [SerializeField]
    private Sprite _redCardSprite = null;
    [SerializeField]
    private Sprite _highlightSprite = null;
    [SerializeField]
    private Sprite[] _normalSprites = null;
    [SerializeField]
    private Sprite[] _highlightedSprites = null;

    private CSVExporter _CSVExporter;

    private int _half = 0;
    private int[] _teamScores = new int[2];

    private Dictionary<RecordCategory, List<string>> _team1Timestamps = new Dictionary<RecordCategory, List<string>>();
    private Dictionary<RecordCategory, List<string>> _team2Timestamps = new Dictionary<RecordCategory, List<string>>();

    private void Start()
    {
        SetHalftime(0);
        _CSVExporter = new CSVExporter();        
    }

    public void RecordGoal(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var goalRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        goalRecord.SetSprite(_goalSprite);                
        goalRecord.SetTime(_timer.GetCurrentTime());
        goalRecord.SetTeam(teamId);
        goalRecord.RecordDeleted += OnRecordDeleted;

        if(teamId >= 0 && teamId <= 1)
        {
            _teamScores[teamId]++;
            _scoreText.text = string.Format("{0} : {1}", _teamScores[0], _teamScores[1]);
        }

        RecordTimestamp(teamId, RecordCategory.Goal);
    }
    
    public void RecordYellowCard(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var yellowCardRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        yellowCardRecord.SetSprite(_yellowCardSprite);
        yellowCardRecord.SetTime(_timer.GetCurrentTime());
        yellowCardRecord.RecordDeleted += OnRecordDeleted;

        RecordTimestamp(teamId, RecordCategory.YellowCard);
    }

    public void RecordRedCard(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var redCardRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        redCardRecord.SetSprite(_redCardSprite);
        redCardRecord.SetTime(_timer.GetCurrentTime());
        redCardRecord.RecordDeleted += OnRecordDeleted;

        RecordTimestamp(teamId, RecordCategory.RedCard);
    }

    public void RecordHighlight(int teamId)
    {
        int teamIndex = _half == 0 ? teamId + _half : teamId + _half + 1;
        var highlightRecord = Instantiate(_statsRecord, _teamsStats[teamIndex]);
        highlightRecord.SetSprite(_highlightSprite);
        highlightRecord.SetTime(_timer.GetCurrentTime());
        highlightRecord.RecordDeleted += OnRecordDeleted;

        RecordTimestamp(teamId, RecordCategory.Highlight);
    }

    private void OnRecordDeleted(int teamId, RecordCategory recordCategory, string recordedTimestamp)
    {
        if (recordCategory == RecordCategory.Goal)
        {
            if (teamId >= 0 && teamId <= 1)
            {
                _teamScores[teamId]--;
                _scoreText.text = string.Format("{0} : {1}", _teamScores[0], _teamScores[1]);
            }
        }

        var teamTimestamps = teamId == 0 ? _team1Timestamps : _team2Timestamps;
        if (teamTimestamps.ContainsKey(recordCategory))
        {
            teamTimestamps[recordCategory].Remove(recordedTimestamp);
        }
    }

    private void RecordTimestamp(int teamId, RecordCategory recordCategory)
    {
        if(teamId == 0)
        {
            if (!_team1Timestamps.ContainsKey(recordCategory))
            {
                _team1Timestamps[recordCategory] = new List<string>();
            }

            _team1Timestamps[recordCategory].Add(_timer.GetCurrentTime());
        }
        else
        {
            if (!_team2Timestamps.ContainsKey(recordCategory))
            {
                _team2Timestamps[recordCategory] = new List<string>();
            }

            _team2Timestamps[recordCategory].Add(_timer.GetCurrentTime());
        }
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

    public void Export()
    {
        _CSVExporter.ClearReport();
        _CSVExporter.SetTeams(_teamManager.Teams[0].TeamName, _teamManager.Teams[1].TeamName);

        int team1RecordsCount = 0;
        foreach (var key in _team1Timestamps.Keys)
        {
            team1RecordsCount += _team1Timestamps[key].Count;
        }

        int team2RecordsCount = 0;
        foreach (var key in _team2Timestamps.Keys)
        {
            team2RecordsCount += _team2Timestamps[key].Count;
        }

        var maxTeamRecords = team1RecordsCount > team2RecordsCount ? _team1Timestamps : _team2Timestamps;

        foreach (var key in maxTeamRecords.Keys)
        {
            for (int i = 0; i < maxTeamRecords[key].Count; i++)
            {
                string[] stringToAppend = new string[4];

                if(_team1Timestamps.ContainsKey(key) && i < _team1Timestamps[key].Count)
                {
                    stringToAppend[0] = key.ToString();
                    stringToAppend[1] = _team1Timestamps[key][i];
                }
                else
                {
                    stringToAppend[0] = string.Empty;
                    stringToAppend[1] = string.Empty;
                }

                if (_team2Timestamps.ContainsKey(key) && i < _team2Timestamps[key].Count)
                {
                    stringToAppend[2] = key.ToString();
                    stringToAppend[3] = _team2Timestamps[key][i];
                }
                else
                {
                    stringToAppend[2] = string.Empty;
                    stringToAppend[3] = string.Empty;
                }

                _CSVExporter.AppendToReport(stringToAppend);
            }
        }
    }
}
