using System.Linq;
using Inventory;
using Profile;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] 
    private Transform _placeForUi;
    
    [SerializeField] 
    private UnityAdsTools _unityAdsTools;

    [SerializeField] 
    private ItemConfig[] _itemsConfig;

    [SerializeField] 
    private AbilityConfig[] _abilitiesConfig;

    [SerializeField] 
    private UpgradeItemConfig[] _upgradeItemsConfig;
    
    private MainController _mainController;

    private void Awake()
    {
        var profilePlayer = new ProfilePlayer(15f, _unityAdsTools);
        profilePlayer.CurrentState.Value = GameState.Start;
        _mainController = new MainController(_placeForUi, profilePlayer, _itemsConfig.ToList(),
            _abilitiesConfig.ToList(), _upgradeItemsConfig.ToList());
    }

    protected void OnDestroy()
    {
        _mainController?.Dispose();
    }
}
