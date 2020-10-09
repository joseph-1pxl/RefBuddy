using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    private const float _kOptionHeight = 131f;

    [Serializable]
    public struct Team
    {
        public string TeamName;
        public Sprite TeamLogo;
    }

    [SerializeField]
    private Transform _team1;

    [SerializeField]
    private Transform _team2;

    [SerializeField]
    private Team[] _teams;

    private Dropdown _team1Dropdown;
    private Dropdown _team2Dropdown;

    private Image _team1Logo;
    private Image _team2Logo;
    
    private Dictionary<string, Sprite> _teamLogos;

    public Team[] Teams { get { return _teams; } }

    private void Start()
    {
        _teamLogos = new Dictionary<string, Sprite>();
        foreach (var team in _teams)
        {
            _teamLogos[team.TeamName] = team.TeamLogo;
        }

        _team1Dropdown = _team1.GetComponentInChildren<Dropdown>();
        _team2Dropdown = _team2.GetComponentInChildren<Dropdown>();

        _team1Logo = _team1.GetComponentInChildren<Image>();
        _team2Logo = _team2.GetComponentInChildren<Image>();

        List<Dropdown.OptionData> teamOptions = _teamLogos.Select(kvp => new Dropdown.OptionData(kvp.Key)).ToList();
        _team1Dropdown.AddOptions(teamOptions);
        _team2Dropdown.AddOptions(teamOptions);

        _team1Dropdown.onValueChanged.AddListener(delegate
        {
            DropdownTeam1ValueChanged(_team1Dropdown);
        });

        _team2Dropdown.onValueChanged.AddListener(delegate
        {
            DropdownTeam2ValueChanged(_team2Dropdown);
        });

        DropdownTeam1ValueChanged(_team1Dropdown);
        DropdownTeam2ValueChanged(_team2Dropdown);
    }

    private void DropdownTeam1ValueChanged(Dropdown change)
    {
        string optionText = change.options[change.value].text;
        if (!string.IsNullOrEmpty(optionText) &&  _teamLogos.ContainsKey(optionText))
        {
            _team1Logo.sprite = _teamLogos[optionText];
        }
    }

    private void DropdownTeam2ValueChanged(Dropdown change)
    {
        string optionText = change.options[change.value].text;
        if (!string.IsNullOrEmpty(optionText) && _teamLogos.ContainsKey(optionText))
        {
            _team2Logo.sprite = _teamLogos[optionText];
        }
    }
}
