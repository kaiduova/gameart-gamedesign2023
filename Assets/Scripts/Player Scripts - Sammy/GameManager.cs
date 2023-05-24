using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public CompositeCollider2D LevelCameraBounds;
    public CinemachineConfiner CinemachineConfiner;

    private void Start() { 
        Application.targetFrameRate = 60;
        CinemachineConfiner.m_BoundingShape2D = LevelCameraBounds;
    }

    private void Update() {
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 1: { CinemachineConfiner.m_BoundingShape2D = LevelCameraBounds; } break;
            case 2: { CinemachineConfiner.m_BoundingShape2D = LevelCameraBounds; } break;
        }
    }
}