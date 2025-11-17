using UnityEngine;

namespace MiningMiniGame {

    [CreateAssetMenu(fileName = "MiningMiniGameConfig", menuName = "Configs/MiningMiniGameConfig")]
    public class MiningMiniGameConfig : ScriptableObject
    {
        public float BlockTime = 5f;
        public int DifficultyDecreaseStreak = 3;
        public int DifficultyIncreaseStreak = 5;
        public int SpeedUpPrice = 3;
        public int SpeedUpAmountIncrease = 3;
        public int FailThrashold = 20;
        public int MaxLogLines = 30;
    }

}