using Profile;
using UnityEngine;

public class FightController:BaseController
{
    private readonly ResourcePath _view;
    private FightWindowView _fightWindow;
    private readonly ProfilePlayer _profilePlayer;
    public FightController(ResourcePath fightResources, ProfilePlayer profilePlayer, Transform placeForUi)
    {
        _view = fightResources;
        _profilePlayer = profilePlayer;
        var prefab = ResourceLoader.LoadObject<FightWindowView>(fightResources);
            //_fightWindow = GameObject.Instantiate(prefab, placeForUi);
        _fightWindow = GameObject.Instantiate(prefab);
        AddGameObjects(_fightWindow.gameObject);
        _fightWindow.ExitRequested+=Exit;
    }

    private void Exit()
    {
        _profilePlayer.CurrentState.Value = GameState.Start;
    }
}

