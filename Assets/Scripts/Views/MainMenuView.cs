using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] 
    private Button _buttonStart;
    [SerializeField]
    private Button _buttonReward;
    [SerializeField]
    private Button _buttonFight;
    [SerializeField]
    private Button _buttonExit;
    public void Init(UnityAction startGame, UnityAction dailyReward, UnityAction fightGame, UnityAction exit)
    {
        _buttonStart.onClick.AddListener(startGame);
        _buttonReward.onClick.AddListener(dailyReward);
        _buttonFight.onClick.AddListener(fightGame);
        _buttonExit.onClick.AddListener(exit);
    }

    protected void OnDestroy()
    {
        _buttonStart.onClick.RemoveAllListeners();
        _buttonReward.onClick.RemoveAllListeners();
        _buttonExit.onClick.RemoveAllListeners();
        _buttonFight.onClick.RemoveAllListeners();
    }
}

