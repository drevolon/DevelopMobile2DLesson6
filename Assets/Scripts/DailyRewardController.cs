using System;
using System.Collections;
using System.Collections.Generic;
using Profile;
using UnityEngine;

public class DailyRewardController
{
    private readonly ProfilePlayer _playerModel;
    private readonly Transform _placeForUi;
    private DailyRewardView _dailyRewardView;
    private List<ContainerSlotRewardView> _slots;

    private bool _isGetReward;

    private float maxDailyValueSecound = 86400f;

    public DailyRewardController(ProfilePlayer playerModel, ResourcePath viewResource, Transform placeForUi)
    {
        _playerModel = playerModel;
        _placeForUi = placeForUi;
        // _dailyRewardView = generateLevelView;
       var prefab=ResourceLoader.LoadObject<DailyRewardView>(viewResource);
       _dailyRewardView=GameObject.Instantiate(prefab, placeForUi);
       RefreshView();
    }

    public void RefreshView()
    {
        InitSlots();

        _dailyRewardView.StartCoroutine(RewardsStateUpdater());

        RefreshUi();
        SubscribeButtons();
    }

    private void InitSlots()
    {
        _slots = new List<ContainerSlotRewardView>();

        for (var i = 0; i < _dailyRewardView.Rewards.Count; i++)
        {
            var instanceSlot = GameObject.Instantiate(_dailyRewardView.ContainerSlotRewardView,
                _dailyRewardView.MountRootSlotsReward, false);

            _slots.Add(instanceSlot);
        }
    }

    private IEnumerator RewardsStateUpdater()
    {
        while (true)
        {
            RefreshRewardsState();
            yield return new WaitForSeconds(1);
        }
    }

    private void RefreshRewardsState()
    {
        _isGetReward = true;

        if (_dailyRewardView.TimeGetReward.HasValue)
        {
            var timeSpan = DateTime.UtcNow - _dailyRewardView.TimeGetReward.Value;

            if (timeSpan.Seconds > _dailyRewardView.TimeDeadline)
            {
                _dailyRewardView.TimeGetReward = null;
                _dailyRewardView.CurrentSlotInActive = 0;
            }
            else if (timeSpan.Seconds < _dailyRewardView.TimeCooldown)
            {
                _isGetReward = false;
            }
        }

        RefreshUi();
    }

    private void RefreshUi()
    {
        _dailyRewardView.GetRewardButton.interactable = _isGetReward;

        if (_isGetReward)
        {
            _dailyRewardView.TimerNewReward.text = "The reward is received today";
        }
        else
        {
            if (_dailyRewardView.TimeGetReward != null)
            {
                var nextClaimTime = _dailyRewardView.TimeGetReward.Value.AddSeconds(_dailyRewardView.TimeCooldown);
                var currentClaimCooldown = nextClaimTime - DateTime.UtcNow;
                var timeGetReward =
                    $"{currentClaimCooldown.Days:D2}:{currentClaimCooldown.Hours:D2}:{currentClaimCooldown.Minutes:D2}:{currentClaimCooldown.Seconds:D2}";

                _dailyRewardView.TimerNewReward.text = $"Time to get the next reward: {timeGetReward}";

                _dailyRewardView.DailySlider.minValue = 0f;
                _dailyRewardView.DailySlider.maxValue = maxDailyValueSecound;
                
                _dailyRewardView.DailySlider.value = maxDailyValueSecound - float.Parse(currentClaimCooldown.TotalSeconds.ToString());
            }
        }

        for (var i = 0; i < _slots.Count; i++)
            _slots[i].SetData(_dailyRewardView.Rewards[i], i + 1, i == _dailyRewardView.CurrentSlotInActive);
    }

    private void SubscribeButtons()
    {
        _dailyRewardView.GetRewardButton.onClick.AddListener(ClaimReward);
        _dailyRewardView.ResetButton.onClick.AddListener(ResetTimer);
        _dailyRewardView.ReturnButton.onClick.AddListener(Return);
    }

    private void Return()
    {
        _playerModel.CurrentState.Value = GameState.Start;
    }

    private void ClaimReward()
    {
        if (!_isGetReward)
            return;

        var reward = _dailyRewardView.Rewards[_dailyRewardView.CurrentSlotInActive];

        switch (reward.RewardType)
        {
            case RewardType.Wood:
                CurrencyView.Instance.AddWood(reward.CountCurrency);
                break;
            case RewardType.Diamond:
                CurrencyView.Instance.AddDiamond(reward.CountCurrency);
                break;
        }

        _dailyRewardView.TimeGetReward = DateTime.UtcNow;
        _dailyRewardView.CurrentSlotInActive =
            (_dailyRewardView.CurrentSlotInActive + 1) % _dailyRewardView.Rewards.Count;

        RefreshRewardsState();
    }

    private void ResetTimer()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
