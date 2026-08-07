using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Util
{
    public static class Util
    {
        /// <summary>
        /// Return an item from the array, given a corresponding weights array that accounts for how often certain items should spawn.
        /// </summary>
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

        /// <summary>
        /// Get a random item from an array.
        /// </summary>
        public static T GetRandomItem<T>(this T[] itemArray)
        {
            return itemArray[Random.Range(0, itemArray.Length)];
        }

        /// <summary>
        /// Attempt to set destination, and log an error if we fail.
        /// </summary>
        public static bool TrySetDestination(this NavMeshAgent agent, Vector3 destination)
        {
            bool succeeded = agent.SetDestination(destination);
            if (!succeeded)
            {
                Debug.LogError($"NavMeshAgent {agent} ({agent.name}) failed to set destination: {destination}");
            }
            return succeeded;
        }

        public static void TrySetEnabledCollider(this GameObject obj, bool enable)
        {
            if (obj.TryGetComponent<Collider>(out Collider collider))
            {
                collider.enabled = enable;
            }
        }
    }

}