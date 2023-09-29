using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ProjectSpy.Player
{
    internal class Movement
    {
        public bool DoMove = false;

        public float Acceleration = 0.50f;
        public float Decceleration = 0.25f;
        public float TopSeed = 1.0f;
        public float CurrentSpeed = 0;

        public Vector2 Velocity = new Vector2(0, 0);
        public Vector2 Position = new Vector2(0, 0);

        public bool BanKeyLeft = false;
        public bool BanKeyRight = false;
        public bool BanKeyUp = false;
        public bool BanKeyDown = false;
        public bool BanKeyKick = false;

        public bool KeyLeft = false;
        public bool KeyRight = false;
        public bool KeyUp = false;
        public bool KeyDown = false;
        public bool KeyKick = false;

        Keys KeyCodeLeft;
        Keys KeyCodeRight;
        Keys KeyCodeUp;
        Keys KeyCodeDown;
        Keys KeyCodeKick;

        public MovementKeys LastKey;

        public enum MovementKeys
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
            Kick = 4,
            Use = 5,
        }

        public Movement(int PlayerId)
        {
            switch (PlayerId)
            {
                case 0:
                    KeyCodeLeft = Keys.Left;
                    KeyCodeRight = Keys.Right;
                    KeyCodeUp = Keys.Up;
                    KeyCodeDown = Keys.Down;
                    KeyCodeKick = Keys.Space;
                    break;
                case 1:
                    KeyCodeLeft = Keys.A;
                    KeyCodeRight = Keys.D;
                    KeyCodeUp = Keys.W;
                    KeyCodeDown = Keys.S;
                    KeyCodeKick = Keys.E;
                    break;
            }
        }

        public void ProcessMovementKeys()
        {
            KeyLeft = Input.KeyDown(KeyCodeLeft);
            KeyRight = Input.KeyDown(KeyCodeRight);
            KeyUp = Input.KeyDown(KeyCodeUp);
            KeyDown = Input.KeyDown(KeyCodeDown);
            KeyKick = Input.KeyDown(KeyCodeKick);

            if (Input.KeyPressed(KeyCodeLeft))
            {
                LastKey = MovementKeys.Left;
            }
            else if (Input.KeyPressed(KeyCodeRight))
            {
                LastKey = MovementKeys.Right;
            }
            else if (Input.KeyPressed(KeyCodeUp))
            {
                LastKey = MovementKeys.Up;
            }
            else if (Input.KeyPressed(KeyCodeDown))
            {
                LastKey = MovementKeys.Down;
            }

            if (KeyLeft || KeyRight || KeyUp || KeyDown)
            {
                DoMove = true;
            }
            else
            {
                DoMove = false;
            }
        }


        public void Update(GameTime gameTime)
        {
            bool Move;

            ProcessMovementKeys();

            bool Left = KeyLeft && !BanKeyLeft;
            bool Right = KeyRight && !BanKeyRight;
            bool Up = KeyUp && !BanKeyUp;
            bool Down = KeyDown && !BanKeyDown;
            bool Kick = KeyKick && !BanKeyKick;



            Move = (Left || Right || Up || Down) && !Kick;

            if (Move)
            {
                if (CurrentSpeed < TopSeed)
                {
                    CurrentSpeed += Acceleration;
                }
            }
            else
            {
                if (CurrentSpeed > 0)
                {
                    CurrentSpeed -= Decceleration;
                }
            }

            if (CurrentSpeed > TopSeed)
            {
                CurrentSpeed = TopSeed;
            }

            if (CurrentSpeed < 0)
            {
                CurrentSpeed = 0;
            }
            Velocity = Vector2.Zero;
            if (CurrentSpeed > 0)
            {
                bool DiagonalMovement = (Left || Right) && (Down || Up);
                if (Left && !Right)
                {
                    Velocity.X = -CurrentSpeed;
                }
                else if (Right && !Left)
                {
                    Velocity.X = CurrentSpeed;
                }
                if (Up && !Down)
                {
                    Velocity.Y = -CurrentSpeed;
                }
                else if (Down && !Up)
                {
                    Velocity.Y = CurrentSpeed;
                }
                if (DiagonalMovement)
                {
                    Velocity = Vector2.Normalize(Velocity) * CurrentSpeed;
                }
            }
            if (Velocity != Vector2.Zero)
            {
                Position += Velocity;
            }
        }
    }
}
