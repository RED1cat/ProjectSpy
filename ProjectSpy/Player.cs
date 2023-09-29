using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSpy
{
    internal class Player
    {
        public Movement PlayerMovement;
        public bool DoMove = false;
        public bool DoKick = false;
        public bool ReceiveKick = false;
        public int PlayerId;
        public int Layer = 0;
        private float AnimationTimer;
        private int AnimationThreshold;
        private Rectangle[] PlayerAnimations;
        private byte CurrentAnimationId;
        private SpriteEffects AnimationFlip = SpriteEffects.None;
        public Rectangle PlayerMoveCollision = new Rectangle(12, 26, 8, 11);
        private Point PlayerMoveOffset = new Point(12, 26);

        public Rectangle PlayerHitCollision = new Rectangle(2, 3, 28, 34);
        private Point PlayerHit = new Point(10, 6);

        private Rectangle BombRectangle = new Rectangle(256, 15, 9, 12);
        private Point[] BombOffsets = new Point[4];

        private Rectangle SpringRectangle = new Rectangle(256, 28, 8, 15);
        private Point[] SpringOffsets = new Point[4];

        private Rectangle BucketRectangle = new Rectangle(257, 0, 8, 14);
        private Point[] BucketOffsets = new Point[4];

        private Rectangle TimeBombRectangle = new Rectangle(256, 44, 8, 8);
        private Point[] TimeBombOffsets = new Point[4];

        private int item = 0;
        private int itemLocationOffsetId = 0;
        private SpriteEffects itemFlip = SpriteEffects.None;

        public Player(int playerId)
        {
            PlayerId = playerId;
            //PlayerMoveOffset.Y -= (playerId * 104);

            AnimationTimer = 0;
            AnimationThreshold = 100;
            PlayerAnimations = new Rectangle[9];
            PlayerMovement = new Movement(playerId);
            PlayerMovement.Position = new Vector2(50, 50 + (playerId * 104));

            //anim
            //idle
            PlayerAnimations[0] = new Rectangle(0, 0 + (80 * playerId), 32, 39); //down
            PlayerAnimations[1] = new Rectangle(33, 0 + (80 * playerId), 32, 39); //up
            PlayerAnimations[2] = new Rectangle(66, 0 + (80 * playerId), 32, 39); //left right

            //walk
            PlayerAnimations[3] = new Rectangle(0, 40 + (80 * playerId), 32, 39); //down
            PlayerAnimations[4] = new Rectangle(33, 40 + (80 * playerId), 32, 39); //up
            PlayerAnimations[5] = new Rectangle(66, 40 + (80 * playerId), 32, 39); //left right

            //Kick
            PlayerAnimations[6] = new Rectangle(99, 0 + (80 * playerId), 32, 39);
            PlayerAnimations[7] = new Rectangle(99, 40 + (80 * playerId), 32, 39);

            //ReceiveKick
            PlayerAnimations[8] = new Rectangle(132, 40 + (80 * playerId), 32, 39);

            //item
            //Bomb
            BombOffsets[0] = new Point(19, 17); //down
            BombOffsets[1] = new Point(3, 17); //up
            BombOffsets[2] = new Point(6, 17); //left
            BombOffsets[3] = new Point(18, 17); //right

            //Spring
            SpringOffsets[0] = new Point(20, 21); //down
            SpringOffsets[1] = new Point(4, 21); //up
            SpringOffsets[2] = new Point(5, 21); //left
            SpringOffsets[3] = new Point(19, 21); //right

            //Bucket
            BucketOffsets[0] = new Point(19, 20); //down
            BucketOffsets[1] = new Point(6, 21); //up
            BucketOffsets[2] = new Point(6, 21); //left
            BucketOffsets[3] = new Point(18, 21); //right

            //TimeBomb
            TimeBombOffsets[0] = new Point(20, 21); //down
            TimeBombOffsets[1] = new Point(5, 21); //up
            TimeBombOffsets[2] = new Point(5, 21); //left
            TimeBombOffsets[3] = new Point(19, 21); //right


            CurrentAnimationId = 1;
        }
        public void Update(GameTime gameTime)
        {
            PlayerMovement.Update(gameTime);

            DoMove = PlayerMovement.DoMove;

            DoKick = PlayerMovement.KeyKick;

            PlayerMoveCollision.X = PlayerMovement.Position.ToPoint().X + PlayerMoveOffset.X;
            PlayerMoveCollision.Y = PlayerMovement.Position.ToPoint().Y + PlayerMoveOffset.Y;

            PlayerHitCollision.X = PlayerMovement.Position.ToPoint().X + PlayerHit.X;
            PlayerHitCollision.Y = PlayerMovement.Position.ToPoint().Y + PlayerHit.Y;

            if (Input.KeyPressed(Keys.Z))
            {
                if(item == 4)
                {
                    item = 0;
                }
                else
                {
                    item++;
                }
            }

            AnimationUpdate(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D playerTexture)
        {

            spriteBatch.Draw(playerTexture, PlayerMovement.Position, PlayerAnimations[CurrentAnimationId], Color.White, 0, new Vector2(0, 0), Program.GlobalScale, AnimationFlip, 0);//player

            if (!PlayerMovement.KeyKick)
            {
                switch (item)
                {
                    case 0:
                        break;
                    case 1:
                        spriteBatch.Draw(playerTexture, PlayerMovement.Position + BombOffsets[itemLocationOffsetId].ToVector2(), BombRectangle, Color.White, 0, new Vector2(0, 0), Program.GlobalScale, itemFlip, 0);//bomb
                        break;
                    case 2:
                        spriteBatch.Draw(playerTexture, PlayerMovement.Position + SpringOffsets[itemLocationOffsetId].ToVector2(), SpringRectangle, Color.White, 0, new Vector2(0, 0), Program.GlobalScale, itemFlip, 0);//bomb
                        break;
                    case 3:
                        spriteBatch.Draw(playerTexture, PlayerMovement.Position + BucketOffsets[itemLocationOffsetId].ToVector2(), BucketRectangle, Color.White, 0, new Vector2(0, 0), Program.GlobalScale, itemFlip, 0);//bomb
                        break;
                    case 4:
                        spriteBatch.Draw(playerTexture, PlayerMovement.Position + TimeBombOffsets[itemLocationOffsetId].ToVector2(), TimeBombRectangle, Color.White, 0, new Vector2(0, 0), Program.GlobalScale, itemFlip, 0);//bomb
                        break;

                }
            }

            //spriteBatch.Draw(texture, new Vector2(movement.Position.X + 19, movement.Position.Y + 20), new Rectangle(198, 80, 16, 14), Color.White);
        }

        void AnimationUpdate(GameTime gameTime)
        {
            if (AnimationTimer > AnimationThreshold)
            {
                if (!ReceiveKick)
                {
                    if (PlayerMovement.KeyKick)
                    {
                        if (PlayerMovement.LastKey == Movement.MovementKeys.Left)
                        {
                            AnimationFlip = 0;
                        }
                        else if (PlayerMovement.LastKey == Movement.MovementKeys.Right)
                        {
                            AnimationFlip = SpriteEffects.FlipHorizontally;
                        }
                        if (CurrentAnimationId == 7)
                        {
                            CurrentAnimationId = 6;
                        }
                        else
                        {
                            CurrentAnimationId = 7;
                        }
                    }


                    if ((PlayerMovement.KeyUp && !PlayerMovement.KeyLeft && !PlayerMovement.KeyRight) && !PlayerMovement.KeyKick)  //up
                    {
                        itemLocationOffsetId = 1;
                        itemFlip = SpriteEffects.FlipHorizontally;

                        if (CurrentAnimationId == 4 && AnimationFlip == 0)
                        {
                            AnimationFlip = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            CurrentAnimationId = 4;
                            AnimationFlip = 0;
                        }
                    }
                    else if ((PlayerMovement.KeyDown && !PlayerMovement.KeyLeft && !PlayerMovement.KeyRight) && !PlayerMovement.KeyKick)  //down
                    {
                        itemLocationOffsetId = 0;
                        itemFlip = 0;

                        if (CurrentAnimationId == 3 && AnimationFlip == 0)
                        {
                            AnimationFlip = SpriteEffects.FlipHorizontally;
                        }
                        else
                        {
                            CurrentAnimationId = 3;
                            AnimationFlip = 0;
                        }
                    }



                    if ((PlayerMovement.KeyLeft || PlayerMovement.KeyLeft && PlayerMovement.KeyUp || PlayerMovement.KeyLeft && PlayerMovement.KeyDown) && !PlayerMovement.KeyKick)  //left
                    {
                        itemLocationOffsetId = 2;
                        itemFlip = SpriteEffects.FlipHorizontally;

                        AnimationFlip = 0;
                        if (CurrentAnimationId == 5)
                        {
                            CurrentAnimationId = 2;
                        }
                        else
                        {
                            CurrentAnimationId = 5;
                        }
                    }
                    else if ((PlayerMovement.KeyRight || PlayerMovement.KeyRight && PlayerMovement.KeyUp || PlayerMovement.KeyRight && PlayerMovement.KeyDown) && !PlayerMovement.KeyKick)  //Right
                    {
                        itemLocationOffsetId = 3;
                        itemFlip = 0;

                        AnimationFlip = SpriteEffects.FlipHorizontally;
                        if (CurrentAnimationId == 5)
                        {
                            CurrentAnimationId = 2;
                        }
                        else
                        {
                            CurrentAnimationId = 5;
                        }
                    }
                }
                else
                {
                    if (CurrentAnimationId == 8)
                    {
                        ReceiveKick = false;
                    }
                    else
                    {
                        CurrentAnimationId = 8;
                    }
                }

                AnimationTimer = 0;
            }
            else
            {

                AnimationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (!PlayerMovement.KeyLeft && !PlayerMovement.KeyRight && !PlayerMovement.KeyUp && !PlayerMovement.KeyDown && !PlayerMovement.KeyKick && !ReceiveKick)
            {
                switch (PlayerMovement.LastKey)
                {
                    case Movement.MovementKeys.Left:
                        CurrentAnimationId = 2;
                        AnimationFlip = 0;

                        itemLocationOffsetId = 2;
                        itemFlip = SpriteEffects.FlipHorizontally;
                        break;
                    case Movement.MovementKeys.Right:
                        CurrentAnimationId = 2;
                        AnimationFlip = SpriteEffects.FlipHorizontally;

                        itemLocationOffsetId = 3;
                        itemFlip = 0;
                        break;
                    case Movement.MovementKeys.Up:
                        CurrentAnimationId = 1;

                        itemLocationOffsetId = 1;
                        itemFlip = SpriteEffects.FlipHorizontally;
                        break;
                    case Movement.MovementKeys.Down:
                        CurrentAnimationId = 0;

                        itemLocationOffsetId = 0;
                        itemFlip = 0;
                        break;
                }
            }
        }

        public void ChangeScale(float OldScale, float NewScale)
        {
            PlayerMovement.Acceleration = (PlayerMovement.Acceleration / OldScale) * NewScale;
            PlayerMovement.Decceleration = (PlayerMovement.Decceleration / OldScale) * NewScale;
            PlayerMovement.TopSeed = (PlayerMovement.TopSeed / OldScale) * NewScale;

            PlayerMovement.Position = (PlayerMovement.Position / OldScale) * NewScale;

            PlayerMoveOffset.X = (PlayerMoveOffset.X / (int)OldScale) * (int)NewScale;
            PlayerMoveOffset.Y = (PlayerMoveOffset.Y / (int)OldScale) * (int)NewScale;
            PlayerMoveCollision.X = (PlayerMoveCollision.X / (int)OldScale) * (int)NewScale;
            PlayerMoveCollision.Y = (PlayerMoveCollision.Y / (int)OldScale) * (int)NewScale;
            PlayerMoveCollision.Height = (PlayerMoveCollision.Height / (int)OldScale) * (int)NewScale;
            PlayerMoveCollision.Width = (PlayerMoveCollision.Width / (int)OldScale) * (int)NewScale;

            for (int i = 0; i < 4; i++)
            {
                BombOffsets[i].X = (BombOffsets[i].X / (int)OldScale) * (int)NewScale;
                BombOffsets[i].Y = (BombOffsets[i].Y / (int)OldScale) * (int)NewScale;

                SpringOffsets[i].X = (SpringOffsets[i].X / (int)OldScale) * (int)NewScale;
                SpringOffsets[i].Y = (SpringOffsets[i].Y / (int)OldScale) * (int)NewScale;

                BucketOffsets[i].X = (BucketOffsets[i].X / (int)OldScale) * (int)NewScale;
                BucketOffsets[i].Y = (BucketOffsets[i].Y / (int)OldScale) * (int)NewScale;

                TimeBombOffsets[i].X = (TimeBombOffsets[i].X / (int)OldScale) * (int)NewScale;
                TimeBombOffsets[i].Y = (TimeBombOffsets[i].Y / (int)OldScale) * (int)NewScale;
            }
        }
    }
}
