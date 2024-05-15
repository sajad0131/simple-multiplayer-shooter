using Project.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    [SerializeField]
    private FixedButton shootButton,secondShootButton, reloadButton;

    [SerializeField]
    public FloatingJoystick moveJoystick;
    [SerializeField]
    private FixedTouchField aimArea;

    private PlayerManager playerManager;

    public Vector2 aim, move;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        shootButton = GameObject.FindGameObjectWithTag("FireButton").GetComponent<FixedButton>();
        secondShootButton = GameObject.FindGameObjectWithTag("SecondFireButton").GetComponent<FixedButton>();
        moveJoystick = GameObject.FindGameObjectWithTag("MoveJoystick").GetComponent<FloatingJoystick>();
        aimArea = GameObject.FindGameObjectWithTag("AimArea").GetComponent<FixedTouchField>();
        reloadButton = GameObject.FindGameObjectWithTag("ReloadButton").GetComponent<FixedButton>();


    }

    // Update is called once per frame
    void Update()
    {
        playerManager.shootButton = shootButton.Pressed;
        playerManager.moveAxis = moveJoystick.Direction;
        playerManager.aimAxis = aimArea.TouchDist;
        aim = aimArea.TouchDist;
        move = moveJoystick.Direction;
        playerManager.reloadButton = reloadButton.Pressed;
        playerManager.run = moveJoystick.run;
        playerManager.secondShootButton = secondShootButton.Pressed;
    }
}
