using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {

    public event System.Action<Player> OnLocalPlayerJoined;

    private GameObject gameObject;
    private static GameManager m_instance;
    public static GameManager Instance {
        get {
            if(m_instance == null) {
                m_instance = new GameManager();
                m_instance.gameObject = new GameObject("_gameManager");
                m_instance.gameObject.AddComponent<InputController>();
                m_instance.gameObject.AddComponent<Timer>();
                m_instance.gameObject.AddComponent<Respawner>();
            }
            return m_instance;
        }
    }
    private InputController m_InputController;
    public InputController InputController {
        get {
            if (m_InputController == null) {
                m_InputController = gameObject.GetComponent<InputController>();
            }
            return m_InputController;
            
        }
    }

    private Timer m_Timer;
    public Timer Timer {
        get {
            if(m_Timer == null) {
                m_Timer = gameObject.GetComponent<Timer>();
            }
            return m_Timer;
        }
    }

    private Respawner m_Respawner;
    public Respawner Respawner {
        get {
            if(m_Respawner == null) {
                m_Respawner = gameObject.GetComponent<Respawner>();
            }
            return m_Respawner;
        }
    }

    private Player m_LocalPlayer;
    public Player LocalPlayer {
        get {
            return m_LocalPlayer;
        }
        set {
            m_LocalPlayer = value;
            if(OnLocalPlayerJoined != null) {
                OnLocalPlayerJoined(m_LocalPlayer);
            }
        }
    }

}
