using System;
using System.Collections.Generic;
using Inventory;
using Items;
using Profile;
using UnityEngine;

public class MainController : BaseController
{
    private ResourcePath RewardViewPath = new ResourcePath {PathResource = "Prefabs/Reward Window" };
    private ResourcePath FightViewPath = new ResourcePath {PathResource = "Prefabs/FightWindow"};

    private MainMenuController _mainMenuController;
    private DailyRewardController _dailyRewardController;
    private GameController _gameController;
    private InventoryController _inventoryController;
    private FightController _fightController;

    private readonly Transform _placeForUi;
    private readonly ProfilePlayer _profilePlayer;
    private readonly List<ItemConfig> _itemsConfig;
    private readonly List<AbilityConfig> _abilitiesConfig;
    private readonly List<UpgradeItemConfig> _upgradeItemsConfig;

    private InventoryModel _inventoryModel;
    private ItemsRepository _itemsRepository;

    public MainController(Transform placeForUi, ProfilePlayer profilePlayer, 
        List<ItemConfig> itemsConfig, 
        List<AbilityConfig> abilitiesConfig, 
        List<UpgradeItemConfig> upgradeItemsConfig)
    {
        _profilePlayer = profilePlayer;
        _placeForUi = placeForUi;
        _itemsConfig = itemsConfig;
        _abilitiesConfig = abilitiesConfig;
        _upgradeItemsConfig = upgradeItemsConfig;
        _inventoryModel = new InventoryModel();
        _itemsRepository = new ItemsRepository(itemsConfig);
        _inventoryController = new InventoryController(_inventoryModel, _itemsRepository);
        AddController(_inventoryController);
        OnChangeGameState(_profilePlayer.CurrentState.Value);
        profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
    }

   
    
    protected override void OnDispose()
    {
        AllClear();
        
        _profilePlayer.CurrentState.UnSubscriptionOnChange(OnChangeGameState);
        
        base.OnDispose();
    }

    private void OnChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                _gameController?.Dispose();
                _inventoryController?.Dispose();
                _mainMenuController?.Dispose();
                _dailyRewardController?.Dispose();
                _fightController?.Dispose();
                _mainMenuController = new MainMenuController(_placeForUi, _profilePlayer, _upgradeItemsConfig );
                break;
            case GameState.Game:
                _inventoryController.ShowInventory();
                _mainMenuController?.Dispose();
                _gameController?.Dispose();
                _dailyRewardController?.Dispose();
                _fightController?.Dispose();
                _gameController = new GameController(_profilePlayer, _inventoryModel, new AbilitiesRepositoryStub());
                break;
            case GameState.Daily:
                _gameController?.Dispose();
                _mainMenuController?.Dispose();
                _dailyRewardController?.Dispose();
                _inventoryController?.Dispose();
                _fightController?.Dispose();
                _dailyRewardController = new DailyRewardController(_profilePlayer, RewardViewPath, _placeForUi);
                break;
            case GameState.Fight:
                _gameController?.Dispose();
                _mainMenuController?.Dispose();
                _dailyRewardController?.Dispose();
                _inventoryController?.Dispose();
                _fightController?.Dispose();
                _fightController = new FightController(FightViewPath, _profilePlayer, _placeForUi);
                break;
            default:
                AllClear();
                break;
        }
    }

    private void AllClear()
    {
        _mainMenuController?.Dispose();
        _gameController?.Dispose();
        _inventoryController?.Dispose();
        _dailyRewardController?.Dispose();
        _fightController?.Dispose();
    }
}