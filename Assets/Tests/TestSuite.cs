using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.AI;


namespace Tests
{
    public class TowerTests
    {
        private TowerPreset normalPreset = ScriptableObject.CreateInstance<TowerPreset>();
        private TowerPreset debuffPreset = ScriptableObject.CreateInstance<TowerPreset>();
        private TowerPreset aoePreset = ScriptableObject.CreateInstance<TowerPreset>();
        private GameObject tile;

        [SetUp]
        public void Setup()
        {
            tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.AddComponent<TowerTile>();
            tile.tag = "Buildable";

            normalPreset.baseCost = 10;
            normalPreset.upgradeCost = 20;
            normalPreset.fireInterval = 1.0f;
            normalPreset.baseSellPrice = 15;
            normalPreset.towerPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            normalPreset.towerPrefab.AddComponent<Tower>();
            normalPreset.towerType = TowerType.Normal;
            normalPreset.towerBasicModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            normalPreset.towerUpgradedModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            debuffPreset.baseCost = 10;
            debuffPreset.upgradeCost = 20;
            debuffPreset.fireInterval = 1.0f;
            debuffPreset.baseSellPrice = 15;
            debuffPreset.towerPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debuffPreset.towerPrefab.AddComponent<Tower>();
            debuffPreset.towerType = TowerType.Debuff;
            debuffPreset.towerBasicModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            debuffPreset.towerUpgradedModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            aoePreset.baseCost = 10;
            aoePreset.upgradeCost = 20;
            aoePreset.fireInterval = 1.0f;
            aoePreset.baseSellPrice = 15;
            aoePreset.towerPrefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            aoePreset.towerPrefab.AddComponent<Tower>();
            aoePreset.towerType = TowerType.AOE;
            aoePreset.towerBasicModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            aoePreset.towerUpgradedModel = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            GameManager.towerPresets[TowerType.Normal] = normalPreset;
            GameManager.towerPresets[TowerType.Debuff] = debuffPreset;
            GameManager.towerPresets[TowerType.AOE] = aoePreset;
        }

        [Test]
        public void TestTowerCreation()
        {
            var tower = TowerFactory.CreateTower(tile.transform, normalPreset);
            Assert.IsNotNull(tower, "Tower is null");
            Assert.IsNotNull(tower.GetComponent<Tower>(), "Tower gameobject does not contain a Tower Component");
            GameObject.Destroy(tower);
        }

        [Test]
        public void TestTowerRegistration()
        {
            var tower = TowerFactory.CreateTower(tile.transform, normalPreset);
            var startID = GameManager.GetCurrentTowerID();
            GameManager.RegisterTower(tower.GetComponent<Tower>());
            var currentID = GameManager.GetCurrentTowerID();
            Assert.IsTrue(startID != currentID, "Registration failed");
            GameManager.UnregisterTower(currentID);
            GameManager.Destroy(tower);
        }

        [Test]
        public void TestTowerPurchase()
        {
            var _towerGO = GameManager.BuyTower(tile.transform, normalPreset.towerType);
            Assert.IsNotNull(_towerGO, $"Purchase failed for tower type {normalPreset.towerType}: function returned null, should have been GameObject");
            GameObject.Destroy(_towerGO);
            GameManager.UnregisterTower(GameManager.GetCurrentTowerID());
        }

        [Test]
        public void TestTowerUpgrade()
        {
            var _towerGO = GameManager.BuyTower(tile.transform, normalPreset.towerType);
            var _tower = GameManager.UpgradeTower(GameManager.GetCurrentTowerID());
            tile.tag = "Upgraded";
            Assert.IsNotNull(_tower, $"Upgrade failed for tower type {_tower.preset.towerType.ToString()}: function returned null, should have been GameObject");
            Assert.IsTrue(_tower.isUpgraded, $"Upgrade failed for tower type {_tower.preset.towerType.ToString()}: member isUpgraded is false");
            GameObject.Destroy(_towerGO);
            GameManager.UnregisterTower(GameManager.GetCurrentTowerID());
        }

        [Test]
        public void TestTowerSell()
        {
            GameManager.BuyTower(tile.transform, normalPreset.towerType);
            var startMoney = GameManager.Money;
            var type = GameManager.GetTower(GameManager.GetCurrentTowerID()).preset.towerType;
            GameManager.SellTower(GameManager.GetCurrentTowerID());
            Assert.IsNull(GameManager.GetTower(GameManager.GetCurrentTowerID()), $"Sell failed for tower type {type.ToString()}: function returned a non null, should have been null");
            Assert.IsTrue(startMoney < GameManager.Money, "Sell failed: Money did not increase");
        }

        [Test]
        public void TestTowerFire()
        {

        }
    }

    public class EnemyTests
    {
        Enemy preset;
        EnemyLogic logic;
        [SetUp]
        public void Setup()
        {
            preset = ScriptableObject.CreateInstance<Enemy>();
            preset.enemyPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            preset.enemyPrefab.AddComponent<EnemyLogic>();
            //preset.enemyPrefab.AddComponent<NavMeshAgent>().enabled = false;
            preset.health = 100;
            preset.maxHealth = 100;
            preset.speed = 1;
            preset.damage = 1;
            preset.coinPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            preset.coinPrefab.AddComponent<Rigidbody>().isKinematic = true;
            preset.coinPrefab.tag = "Coin";
            preset.coinPrefab.layer = 6;
            preset.moneyRange = new Vector2(5, 10);

        }

        [Test]
        public void TestEnemyCreation()
        {
            var enemy = EnemyFactory.CreateEnemy(preset);
            Assert.IsNotNull(enemy, "Enemy Creation failed");
            GameObject.Destroy(enemy);
        }

        [Test]
        public void TestEnemyDestruction()
        {
            var enemy = EnemyFactory.CreateEnemy(preset);
            Assert.IsNotNull(enemy, "Enemy Creation failed");
            var enemyLogic = enemy.GetComponent<EnemyLogic>();
            Assert.IsNotNull(enemyLogic, "No EnemyLogic Script");
            enemyLogic.health = 100;
            enemyLogic.moneyCarried = Random.Range(preset.moneyRange.x, preset.moneyRange.y);
            var startHealth = enemyLogic.health;
            enemyLogic.DealDamage(1);
            Assert.Less(enemyLogic.health, startHealth, "Enemy was not dealt damage");
            Assert.AreEqual(enemyLogic.health, startHealth - 1, "Wrong or no amount of damage was dealt");
            enemyLogic.DealDamage((int)enemyLogic.health);
            Assert.IsTrue(enemy == null, "Enemy was not destroyed");
            var coins = GameObject.FindGameObjectsWithTag("Coin");
            var coinCount = coins.Length;
            Assert.Greater(coinCount, preset.moneyRange.x, "Coin count is less than min");
            Assert.Less(coinCount, preset.moneyRange.y, "Coin count is greater than max");
            foreach (var go in coins)
            {
                GameObject.Destroy(go);
            }
        }
    }

    public class GameModeTests
    {
        [Test]
        public void TestGameMode()
        {
            GameManager.SetGameMode(GameMode.Build);
            Assert.IsTrue(GameManager.GameMode == GameMode.Build, "Gamemode was not set correctly");
            GameManager.SetGameMode(GameMode.StartOfWave);
            Assert.IsTrue(GameManager.GameMode == GameMode.StartOfWave, "Gamemode was not set correctly");
            GameManager.SetGameMode(GameMode.WaveInProgress);
            Assert.IsTrue(GameManager.GameMode == GameMode.WaveInProgress, "Gamemode was not set correctly");
            GameManager.SetGameMode(GameMode.EndOfWave);
            Assert.IsTrue(GameManager.GameMode == GameMode.EndOfWave, "Gamemode was not set correctly");
        }
    }

    public class BuildModeTests
    {
        [Test]
        public void TestBuildMode()
        {
            GameManager.SetBuildMode(BuildMode.None);
            Assert.IsTrue(GameManager.BuildMode == BuildMode.None, "Buildmode was not set correctly");
            GameManager.SetBuildMode(BuildMode.Place);
            Assert.IsTrue(GameManager.BuildMode == BuildMode.Place, "Buildmode was not set correctly");
            GameManager.SetBuildMode(BuildMode.Upgrade);
            Assert.IsTrue(GameManager.BuildMode == BuildMode.Upgrade, "Buildmode was not set correctly");
            GameManager.SetBuildMode(BuildMode.Sell);
            Assert.IsTrue(GameManager.BuildMode == BuildMode.Sell, "Buildmode was not set correctly");
        }
    }
}
