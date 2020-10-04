using UnityEngine;
using UnityEngine.UI;

public class StatsRecord : MonoBehaviour
{
    public delegate void OnGoalRemoved(int teamId);
    public event OnGoalRemoved GoalRemoved;

    [SerializeField]
    private Image _category;

    [SerializeField]
    private Text _timeRecorded;

    [SerializeField]
    private Button _removeButton;

    private int _teamId;

    private void Start()
    {
        _removeButton.onClick.RemoveAllListeners();
        _removeButton.onClick.AddListener(RemoveRecord);
    }

    public void SetCategory(Sprite newCategory)
    {
        _category.sprite = newCategory;
    }

    public void SetTime(string timeRecorded)
    {
        _timeRecorded.text = timeRecorded;
    }

    public void SetTeam(int teamId)
    {
        _teamId = teamId;
    }

    public void RemoveRecord()
    {
        GoalRemoved?.Invoke(_teamId);
        Destroy(gameObject);
    }
}
