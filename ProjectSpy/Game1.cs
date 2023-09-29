using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ProjectSpy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        List<Player>  Players = new List<Player>();
        Collision Collisions = new Collision();
        Texture2D Background;
        Texture2D Player;
        Texture2D Pixel;
        int PlayersLayers;

        Vector2 TimerOffset1 = new Vector2(193, 17);
        Vector2 TimerOffset2 = new Vector2(193, 121);
        Vector2 PowerOffset1 = new Vector2(193, 41);
        Vector2 PowerOffset2 = new Vector2(193, 145);

        DateTime StartTime = DateTime.Now;
        int TimerSeconds = 0;
        int TimerMinutes = 0;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Program.ScreenWidth,
                PreferredBackBufferHeight = Program.ScreenHeight
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Players.Add(new Player(0));
            Players.Add(new Player(1));
            PlayersLayers = Players.Count;

            Collisions.Walls.Add(new Collision.Wall(new Point(64, 63), new Point(143, 63)));
            Collisions.Walls.Add(new Collision.Wall(new Point(19, 108), new Point(188, 108)));
            Collisions.Walls.Add(new Collision.Wall(new Point(64, 63), new Point(19, 108)));
            Collisions.Walls.Add(new Collision.Wall(new Point(143, 63), new Point(188, 108)));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Background = Content.Load<Texture2D>("Back");
            Player = Content.Load<Texture2D>("Players");
            Pixel = Content.Load<Texture2D>("pixel");
            GameFont.Initialize(Content.Load<Texture2D>("Font"));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Input.Update();
            foreach (Player player1 in Players)
            {
                player1.Update(gameTime);
                if (player1.DoMove)
                {
                    //collision
                    CollisionUpdate(player1);
                }
                if (player1.DoKick)
                {
                    foreach (Player player2 in Players)
                    {
                        if (player1.PlayerId != player2.PlayerId)
                        {
                            if (player1.PlayerHitCollision.Intersects(player2.PlayerHitCollision))
                            {
                                 player2.ReceiveKick = true;
                            }
                        }
                    }
                }
            }


            for (int i = 1; i < PlayersLayers; i++)
            {
                for (int j = 0; j < Players.Count; j++)
                {
                    if (j + 1 < Players.Count)
                    {
                        if (Players[j].PlayerMoveCollision.Y < Players[j + 1].PlayerMoveCollision.Y)
                        {
                            Players[j].Layer = i;
                            Players[j + 1].Layer = i + 1;
                        }
                        else
                        {
                            Players[j].Layer = i + 1;
                            Players[j + 1].Layer = i;
                        }
                    }
                }
            }

            if (Input.KeyPressed(Keys.Tab))
            {
                ChangeScale();
            }
            
            TimeSpan span = DateTime.Now - StartTime;

            TimerMinutes = 4 - span.Minutes;
            TimerSeconds = 60 - span.Seconds;

            if(TimerMinutes <= 0 && TimerSeconds <= 0)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            _spriteBatch.Draw(Background, new Vector2(0, 0), new Rectangle(0, 0, 256, 240), Color.White, 0, new Vector2(0, 0), Program.GlobalScale, SpriteEffects.None, 0);

            //int idLayer = 1;

            for (int i = 1; i <= PlayersLayers; i++)
            {
                foreach (Player player in Players)
                {
                    if(player.Layer == i)
                    {
                        player.Draw(_spriteBatch, Player);
                    }
                }
            }

            foreach (Player player in Players) 
            {
                

                _spriteBatch.Draw(Pixel, new Vector2(player.PlayerMoveCollision.X, player.PlayerMoveCollision.Y), new Rectangle(0, 0, player.PlayerMoveCollision.Width, player.PlayerMoveCollision.Height), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 1);
                GameFont.DrawText(_spriteBatch, new Vector2(2, 2 + (16 * player.PlayerId)), $"p:{player.PlayerId}; l:{player.Layer};\nX:{player.PlayerMovement.Position.X}; Y:{player.PlayerMovement.Position.Y}; cm X:{player.PlayerMoveCollision.X}; Y:{player.PlayerMoveCollision.Y}; ch x:{player.PlayerHitCollision.Center.X}; y:{player.PlayerHitCollision.Center.Y}", 1f);
            }

            GameFont.DrawText(_spriteBatch, TimerOffset1, $"time\n{TimerMinutes}:{TimerSeconds}", Program.GlobalScale);
            GameFont.DrawText(_spriteBatch, TimerOffset2, $"time\n{TimerMinutes}:{TimerSeconds}", Program.GlobalScale);

            GameFont.DrawText(_spriteBatch, PowerOffset1, "power", Program.GlobalScale);
            GameFont.DrawText(_spriteBatch, PowerOffset2, "power", Program.GlobalScale);

            _spriteBatch.End();
            

            base.Draw(gameTime);
        }

        void CollisionUpdate(Player player)
        {

            player.PlayerMovement.BanKeyLeft = Collisions.CheckCollision(Collisions.Walls[2].StartWall, Collisions.Walls[2].EndWall, player.PlayerMoveCollision);
            player.PlayerMovement.BanKeyRight = Collisions.CheckCollision(Collisions.Walls[3].StartWall, Collisions.Walls[3].EndWall, player.PlayerMoveCollision);

            if(player.PlayerMovement.BanKeyLeft || player.PlayerMovement.BanKeyRight)
            {
                player.PlayerMovement.BanKeyUp = true;
            }
            else
            {
                player.PlayerMovement.BanKeyUp = Collisions.CheckCollision(Collisions.Walls[0].StartWall, Collisions.Walls[0].EndWall, player.PlayerMoveCollision);
            }
            player.PlayerMovement.BanKeyDown = Collisions.CheckCollision(Collisions.Walls[1].StartWall, Collisions.Walls[1].EndWall, player.PlayerMoveCollision);
        }

        void ChangeScale()
        {
            switch (Program.GlobalScale)
            {
                case 1f:
                    Program.GlobalScale = 2f;
                    DoChangeScale(1f, 2f);
                    break;
                case 2f:
                    Program.GlobalScale = 3f;
                    DoChangeScale(2f, 3f);
                    break;
                case 3f:
                    Program.GlobalScale = 4f;
                    DoChangeScale(3f, 4f);
                    break;
                case 4f:
                    Program.GlobalScale = 1f;
                    DoChangeScale(4f, 1f);
                    break;
            }
        }
        void DoChangeScale(float OldScale, float NewScale)
        {
            _graphics.PreferredBackBufferWidth /= (int)OldScale;
            _graphics.PreferredBackBufferHeight /= (int)OldScale;

            TimerOffset1 /= OldScale;
            TimerOffset2 /= OldScale;

            PowerOffset1 /= OldScale;
            PowerOffset2 /= OldScale;

            
            _graphics.PreferredBackBufferWidth *= (int)NewScale;
            _graphics.PreferredBackBufferHeight *= (int)NewScale;

            TimerOffset1 *= NewScale;
            TimerOffset2 *= NewScale;

            PowerOffset1 *= NewScale;
            PowerOffset2 *= NewScale;

            _graphics.ApplyChanges();

            foreach (Player player in Players)
            {
                player.ChangeScale(OldScale, NewScale);
            }
            foreach (Collision.Wall wall in Collisions.Walls) 
            {
                wall.ChangeScaleWall(OldScale, NewScale);
            }
        }
    }
}