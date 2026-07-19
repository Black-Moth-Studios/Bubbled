using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerSetup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var player1 = PlayerInput.all[0];
        var player2 = PlayerInput.all[1];

        player1.user.UnpairDevices();
        player2.user.UnpairDevices();

        int gamepads = Gamepad.all.Count;

        if(gamepads >= 2)
        {
            InputUser.PerformPairingWithDevice(
                Gamepad.all[0],
                user: player1.user
            );

            InputUser.PerformPairingWithDevice(
                Gamepad.all[1],
                user: player2.user
            );

            player1.user.ActivateControlScheme("Gamepad");
            player2.user.ActivateControlScheme("Gamepad");
        }

        else if(gamepads == 1)
        {
            InputUser.PerformPairingWithDevice(
                Gamepad.all[0],
                user: player1.user
            );

            InputUser.PerformPairingWithDevice(
                Keyboard.current,
                user: player2.user
            );


            player1.user.ActivateControlScheme("Gamepad");
            player2.user.ActivateControlScheme("Keyboard_Player2");
        }

        else
        {
            InputUser.PerformPairingWithDevice(
                Keyboard.current,
                user: player1.user
            );

            InputUser.PerformPairingWithDevice(
                Keyboard.current,
                user: player2.user
            );


            player1.user.ActivateControlScheme("Keyboard_Player1");
            player2.user.ActivateControlScheme("Keyboard_Player2");
        }
    }
}
