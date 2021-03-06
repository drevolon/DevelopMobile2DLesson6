using System.Collections.Generic;
using System.Linq;
using Inventory;
using Items;
using JetBrains.Annotations;
using UnityEngine;

namespace Garage
{
    public class GarageController : BaseController, IShedController
    {
        private readonly Car _car;

        private readonly UpgradeHandlersRepository _upgradeHandlersRepository;
        private readonly ItemsRepository _upgradeItemsRepository;
        private readonly InventoryModel _inventoryModel;

        public GarageController([NotNull] List<UpgradeItemConfig> upgradeItemConfigs, [NotNull] Car car)
        {
            _car = car;

            _upgradeHandlersRepository
                = new UpgradeHandlersRepository(upgradeItemConfigs);
            AddController(_upgradeHandlersRepository);

            _upgradeItemsRepository
                = new ItemsRepository(upgradeItemConfigs.Select(value => value.ItemConfig).ToList());
            AddController(_upgradeItemsRepository);
        
            _inventoryModel = new InventoryModel();
        }

        public void Enter()
        {
            Debug.Log($"Enter: car has speed : {_car.Speed}");
        }

        public void Exit()
        {
            UpgradeCarWithEquippedItems(_car, _inventoryModel.GetEquippedItems(), _upgradeHandlersRepository.UpgradeItems);
            Debug.Log($"Exit: car has speed : {_car.Speed}");
        }
    
        private void UpgradeCarWithEquippedItems(
            IUpgradableCar upgradableCar,
            IReadOnlyList<IItem> equippedItems,
            IReadOnlyDictionary<int, IUpgradeCarHandler> upgradeHandlers)
        {
            foreach (var equippedItem in equippedItems)
            {
                if (upgradeHandlers.TryGetValue(equippedItem.Id, out var handler))
                    handler.Upgrade(upgradableCar);
            }
        }
    }
}

