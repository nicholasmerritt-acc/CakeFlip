using System.Linq;
using UnityEngine;

namespace Util
{
    public static class Util
    {
        /// <summary>
        /// Return an item from the array, given a corresponding weights array that accounts for how often certain items should spawn.
        /// </summary>
        /// <returns></returns>
        public static T GetWeightedItem<T>(this T[] itemArray, int[] weights)
        {
            if (itemArray.Length != weights.Length)
            {
                Debug.LogError("itemArray and weights must be the same length.");
                return default;
            }

            int sumTotal = 0;
            int maxDropPercent = weights.Sum();
            float currentDropChance = Random.Range(0, maxDropPercent);
            T foundItem = itemArray[0];

            for (int i = 0; i < itemArray.Length; i++)
            {
                int dropRate = weights[i];
                sumTotal += dropRate;
                if (currentDropChance <= sumTotal)
                {
                    foundItem = itemArray[i];
                    Debug.Log($"Found {foundItem} with a {dropRate} % drop rate ({sumTotal} total)");
                    break;
                }
            }

            if (foundItem == null)
            {
                Debug.LogWarning("Weights might be set incorrectly. Returning default (first) item.");
                foundItem = itemArray[0];
            }

            return foundItem;
        }
    }

}