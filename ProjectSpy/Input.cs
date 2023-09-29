using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ProjectSpy
{
    internal class Input
    {
        //private static bool ClickPressed = false;
        private static KeyboardState CurrentKeyboardState;
        private static KeyboardState PreviousKeyboardState;
        private static Dictionary<Keys, bool> KeysPressedThisFrame = new Dictionary<Keys, bool>();

        public static bool KeyPressed(Keys KEY)
        {
            return KeysPressedThisFrame.ContainsKey(KEY);
        }
        public static bool KeyDown(Keys KEY)
        {
            return CurrentKeyboardState.IsKeyDown(KEY);
        }
        private static void CheckButtonPressed()
        {
            Keys[] Keys = CurrentKeyboardState.GetPressedKeys();
            foreach (Keys Key in Keys)
            {
                if (PreviousKeyboardState.IsKeyUp(Key))
                {
                    KeysPressedThisFrame.Add(Key, true);
                }
            }
        }

        public static void Update()
        {
            KeysPressedThisFrame.Clear();
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            CheckButtonPressed();
        }
    }
}
