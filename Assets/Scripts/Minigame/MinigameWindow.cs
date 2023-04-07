using System;
using UnityEngine;

namespace Minigame
{
    [Serializable]
    public struct WindowPositionData
    {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector3 pos;
    }
    
    public class MinigameWindow : MonoBehaviour
    {
        public static MinigameWindow Instance { get; private set; }

        public WindowPositionData[] positionData;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                Debug.LogError("There shouldn't be two instances of MinigameWindow, the other one has been destroyed.");
            }
            Instance = this;
        }

        public void ChangePosition(int position)
        {
            GetComponent<RectTransform>().anchorMin = positionData[position].anchorMin;
            GetComponent<RectTransform>().anchorMax = positionData[position].anchorMax;
            GetComponent<RectTransform>().anchoredPosition3D = positionData[position].pos;
        }
    }
}