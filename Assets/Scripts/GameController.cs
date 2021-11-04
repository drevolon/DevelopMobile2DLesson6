using Inventory;
using Tools;

public class GameController : BaseController
{
    public GameController(ProfilePlayer profilePlayer, IInventoryModel inventoryModel, 
        IAbilityRepository abilitiesRepository)
    {
        var leftMoveDiff = new SubscriptionProperty<float>();
        var rightMoveDiff = new SubscriptionProperty<float>();
        
        var tapeBackgroundController = new TapeBackgroundController(leftMoveDiff, rightMoveDiff);
        AddController(tapeBackgroundController);
        
        var inputGameController = new InputGameController(leftMoveDiff, rightMoveDiff, profilePlayer.CurrentCar);
        AddController(inputGameController);
            
        var carController = new CarController();
        AddController(carController);

        var abilitiesController = new AbilitiesController(
            inventoryModel, abilitiesRepository, 
            new AbilityCollectionViewStub(), carController);

    }
}

