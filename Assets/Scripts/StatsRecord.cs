using UnityEngine;
using UnityEngine.UI;

public enum RecordCategory
{
    Goal = 0,
    RedCard,
    YellowCard,
    Highlight
};

public class StatsRecord : MonoBehaviour
{
    public delegate void OnRecordDeleted(int teamId, RecordCategory recordCategory, string recordedTimestamp);
    public event OnRecordDeleted RecordDeleted;

    [SerializeField]
    private Image _category = null;

    [SerializeField]
    private Text _timeRecorded = null;

    [SerializeField]
    private Button _removeButton = null;

    private RecordCategory m_recordCategory;
    private int _teamId;

    private void Start()
    {
        _removeButton.onClick.RemoveAllListeners();
        _removeButton.onClick.AddListener(RemoveRecord);
    }

    public void SetCategory(RecordCategory recordCategory)
    {
        m_recordCategory = recordCategory;
    }

    public void SetSprite(Sprite newCategory)
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
        RecordDeleted?.Invoke(_teamId, m_recordCategory, _timeRecorded.text);
        Destroy(gameObject);
    }
}
