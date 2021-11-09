using System.Collections.Generic;
using Garage;
using Inventory;
using Profile;
using UnityEngine;
using UnityEngine.Advertisements;

public class MainMenuController : BaseController
{
    private readonly ResourcePath _viewPath = new ResourcePath {PathResource = "Prefabs/mainMenu"};
    private readonly ProfilePlayer _profilePlayer;
    private readonly MainMenuView _view;
    private readonly GarageController _garageController;

    public MainMenuController(Transform placeForUi, ProfilePlayer profilePlayer, List<UpgradeItemConfig> uprgradeItems)
    {
        _profilePlayer = profilePlayer;
        _view = LoadView(placeForUi);
        _view.Init(StartGame,DailyReward, FightGame, Exit);
        _garageController = new GarageController(uprgradeItems, profilePlayer.CurrentCar);
    }

    private void FightGame()
    {
        _profilePlayer.CurrentState.Value = GameState.Fight;
    }

    private void Exit()
    {
        Application.Quit();
    }

    private void DailyReward()
    {
        _profilePlayer.CurrentState.Value = GameState.Daily;
    }

    private MainMenuView LoadView(Transform placeForUi)
    {
        var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
        AddGameObjects(objectView);
        
        return objectView.GetComponent<MainMenuView>();
    }

    private void StartGame()
    {
        _profilePlayer.CurrentState.Value = GameState.Start;
        
        _profilePlayer.AnalyticTools.SendMessage("start_game", ("time", Time.realtimeSinceStartup));
        
        _profilePlayer.AdsShower.ShowInterstitialVideo();
        Advertisement.AddListener(_profilePlayer.AdsListener);
        
        
    }
}

