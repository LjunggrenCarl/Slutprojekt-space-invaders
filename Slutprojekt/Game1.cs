using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Slutprojekt
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D Spacebild;
        Texture2D Spacebild2;

        Texture2D xwingbild;
        Rectangle xwingRectangle;

        Texture2D tiefigtherbild;
        Rectangle tiefighterRectangle;
        List<Rectangle> tiefighterPositioner = new List<Rectangle>();

        Texture2D knappbild;
        Rectangle knapprect;

        Texture2D Yesknappbild;
        Rectangle Yesknapprect;

        Texture2D Noknappbild;
        Rectangle Noknapprect;

        Texture2D loga;
        Rectangle logarect;

        Texture2D Shot;
        Rectangle Shotrect;

        Texture2D greenshot;
        Rectangle greenshotrect;

        Texture2D GameOver;
        Rectangle GameOverRect;

        Vector2 Spacepos = new Vector2(0, 0);
        Vector2 Spacepos2 = new Vector2(0, -1200);

        Texture2D Lives;

        MouseState mus = Mouse.GetState();
        MouseState gammalMus = Mouse.GetState();

        int scene = 0;

        //fullscreen
        int windowWidth;
        int windowHeight;

        //xwing movement
        KeyboardState tangentbord = Keyboard.GetState();
        KeyboardState checktangentbord = Keyboard.GetState();

        //xwing speed
        int xwingspeed = 10;

        //shoting
        int shotspeed = 50;
        bool shots = false;

        //score
        SpriteFont arialFont;
        string meddelande = $"Score:";
        Vector2 meddelandePosition = new Vector2(20, 20);

        //tiefigthermove
        int moveX = 15;
        int moveXtimer = 0;
        int moveY = 15;
        bool TiefighterY = false;
        int moveYtimer = 0;
        
        //score
        int Score = 0;
        

        //shoting teifitghter
        int Tiefightershot;
        int greenshotspeed = 8;

        //levels
        int Showlevel = 1;
        int level = 2;
        SpriteFont arialFontlevel;
        string levelmeddelande = $"Level:";
        Vector2 LevelPosition = new Vector2(900, 20);

        //Play again
        SpriteFont arialFontPlayagain;
        string Playagainmeddelande = $"Play Again?";
        Vector2 PlayagainPosition = new Vector2(875, 620);

        //lives
        int Shiplives = 3;
        List<Rectangle> XwingRectangleLives = new List<Rectangle>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            //fullscreen
            windowWidth = _graphics.PreferredBackBufferWidth;
            windowHeight = _graphics.PreferredBackBufferHeight;

            FullScreen();

            //tiefighterlist
            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    tiefighterPositioner.Add(new Rectangle(200 + 120 * x, 200 + 120 * y, 64, 64));
                }
            }

            //lives
            for (int a = 0; a < Shiplives; a++)
            {

                XwingRectangleLives.Add(new Rectangle((windowWidth - 60 - (a * 50)), 25, 40, 40));

            }

            //mus
            IsMouseVisible = true;

            base.Initialize();
        }

        protected void FullScreen()
        {
            //fullscreen
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;

            _graphics.ApplyChanges();

            windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Space
            Spacebild = Content.Load<Texture2D>("Space");
            Spacebild2 = Content.Load<Texture2D>("Space");

            //xwing
            xwingbild = Content.Load<Texture2D>("xwing");
            xwingRectangle = new Rectangle(895, 1000, xwingbild.Width * 2, xwingbild.Height * 2);

            //tiefighter
            tiefigtherbild = Content.Load<Texture2D>("tiefighter");
            tiefighterRectangle = new Rectangle(200, 150, tiefigtherbild.Width * 2, tiefigtherbild.Height * 2);

            //knapp
            knappbild = Content.Load<Texture2D>("knapp");
            knapprect = new Rectangle(800, 900, knappbild.Width * 1, knappbild.Height * 1);

            //loga
            loga = Content.Load<Texture2D>("space invaders star");
            logarect = new Rectangle(275, 0, loga.Width * 2, loga.Height * 2);

            //red shot
            Shot = Content.Load<Texture2D>("Bullet");
            Shotrect = new Rectangle(0, -100, Shot.Width * 1, Shot.Height * 1);

            //green shot
            greenshot = Content.Load<Texture2D>("greenshot");
            greenshotrect = new Rectangle(-100, -100, Shot.Width * 1, Shot.Height * 1);

            GameOver = Content.Load<Texture2D>("Game Over");
            GameOverRect = new Rectangle(windowWidth  / 2 - GameOver.Width/2, windowHeight / 2 - GameOver.Height / 2 - 200, GameOver.Width * 1, GameOver.Height * 1);

            //yes knapp
            Yesknappbild = Content.Load<Texture2D>("Yes button");
            Yesknapprect = new Rectangle(600, 900, Yesknappbild.Width * 1, Yesknappbild.Height * 1);

            //no knapp
            Noknappbild = Content.Load<Texture2D>("No button");
            Noknapprect = new Rectangle(1030, 900, Noknappbild.Width * 1, Noknappbild.Height * 1);

            //lives
            Lives = Content.Load<Texture2D>("Heart");

            //text
            arialFont = Content.Load<SpriteFont>("File");

            arialFontlevel = Content.Load<SpriteFont>("File");

            arialFontPlayagain = Content.Load<SpriteFont>("File");

        }

        protected override void Update(GameTime gameTime)
        {
            //exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Movingbackground();

            gammalMus = mus;
            mus = Mouse.GetState();

            //scenes update
            switch (scene)
            {

                case 0:
                    Updatemenu();
                    break;
                case 1:
                    Updategame();
                    Updatemenu();
                    break;
                case 2:
                    Updateloseending();
                    break;
               
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //scences draw
            switch (scene)
            {
                case 0:
                    Drawmeny();
                    break;
                case 1:
                    Drawgame();
                    break;
                case 2:
                    Drawloseending();
                    break;
               
            }

            base.Draw(gameTime);
        }

        //movingbackground
        protected void Movingbackground()
        {

            Spacepos.Y += 1;
            Spacepos2.Y += 1;

            if (Spacepos.Y == windowHeight)
            {

                Spacepos = new Vector2(0, -1200);

            }

            if (Spacepos2.Y == windowHeight)
            {

                Spacepos2 = new Vector2(0, -1200);

            }

        }

        //mousebutton
        bool VänsterMusTryckt()
        {
            if (mus.LeftButton == ButtonState.Pressed && gammalMus.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void SwitchScene(int nyscen)
        {

            scene = nyscen;

        }

        void Updatemenu()
        {
            //scene switch
            if (VänsterMusTryckt() && knapprect.Contains(mus.Position))
            {
                SwitchScene(1);
            }

            if (Shiplives == 0)
            {

                SwitchScene(2);

            }

            
        }


        protected void MoveShot()
        { 

            // moves shot
            if (shots && tangentbord.IsKeyDown(Keys.Space) && checktangentbord.IsKeyUp(Keys.Space) && Shotrect.Y < 0)
            {
                Shotrect = new Rectangle((xwingRectangle.X + (xwingbild.Width - 2 )), (windowHeight - xwingbild.Height -80), (Shot.Width * 1), (Shot.Height * 1));
            }

        }

        protected void FireShot()
        {
            if (shots)
            {
                    // shot moves up
                
                if (Shotrect.Y > (0 - Shot.Height * 4))
                {

                    Shotrect.Y -= shotspeed;

                }
               
            }
        }

        protected void Tiefigthershoting()
        {

            Random rand = new Random();

            Tiefightershot = rand.Next(0, tiefighterPositioner.Count - 1);

            if (greenshotrect.Y > windowHeight)
            {

                Rectangle tiefighterRectangle = tiefighterPositioner[Tiefightershot];

                greenshotrect.X = tiefighterRectangle.X + tiefighterRectangle.Width / 2;
                greenshotrect.Y = tiefighterRectangle.Y + tiefighterRectangle.Height;

            }
            else
            {

                greenshotrect.Y += greenshotspeed + (level * 2);

            }


        }

        protected void Intersectplayer()
        {

            if (greenshotrect.Intersects(xwingRectangle) == true)
            {

                greenshotrect = new Rectangle(-100, -100, Shot.Width * 1, Shot.Height * 1);
                Shiplives--;
                XwingRectangleLives.RemoveAt(Shiplives);

            }

        }

        //Shot hits tiefighter
        protected void Intersecttiefighter()
        {
            for (int i = 0; i < tiefighterPositioner.Count; i++)
            {

                if (Shotrect.Intersects(tiefighterPositioner[i]) == true)
                {

                    tiefighterPositioner.RemoveAt(i);
                    Shotrect = new Rectangle(0, -100, Shot.Width * 1, Shot.Height * 1);

                    Score += 10;
                }

            }

        }

        //tiefighter moving x
        void TiefightermoveX ()
        {

            for (int i = 0; i < tiefighterPositioner.Count; i++)
            {

                Rectangle temp = tiefighterPositioner[i];

                if (temp.X > 0 || temp.X <(windowWidth - temp.Width))
                {

                    temp.X += moveX + (level * 2);

                    tiefighterPositioner[i] = temp;

                }

                if (temp.X > (windowWidth - temp.Width) || temp.X < 0)
                {

                    moveX *= -1;

                }

                if (tiefighterPositioner[i].X <= 0)
                {

                    TiefighterY = true;
                    

                }

                if (tiefighterPositioner[i].X >= (windowWidth - temp.Width))
                {

                    TiefighterY = true;
                    

                }

            }

        }

        //tiefighter moving y
        void TiefightermoveY()
        {
            if (TiefighterY == true)
            {

                
                for (int i = 0; i < tiefighterPositioner.Count; i++)
                {

                    Rectangle temp = tiefighterPositioner[i];

                    temp.Y += moveY;
                    tiefighterPositioner[i] = temp;



                }
                TiefighterY = false;

            }
           

        }

        protected void RedrawTiefigther()
        {

            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    tiefighterPositioner.Add(new Rectangle(200 + 120 * x, 200 + 120 * y, 64, 64));
                }
            }

        }

        void Reset()
        {

            tiefighterPositioner.Clear();
            Showlevel = 1;
            level = 2;
            Shiplives = 3;
            Score = 0;

            //tiefigthers
            for (int x = 0; x < 13; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    tiefighterPositioner.Add(new Rectangle(200 + 120 * x, 200 + 120 * y, 64, 64));
                }
            }

            //lives
            for (int a = 0; a < Shiplives; a++)
            {

                XwingRectangleLives.Add(new Rectangle((windowWidth - 60 - (a * 50)), 25, 40, 40));

            }

            Shotrect = new Rectangle(0, -100, Shot.Width * 1, Shot.Height * 1);

            greenshotrect = new Rectangle(-100, -100, Shot.Width * 1, Shot.Height * 1);

        }

        void Updategame()
        {

            MoveShot();
            FireShot();
            Intersecttiefighter();
            Intersectplayer();

            if (tiefighterPositioner.Count != 0)
            {

                Tiefigthershoting();

                //timer x
                if (moveXtimer >= 60)
                {

                    TiefightermoveX();
                    moveXtimer = 0;

                }

                moveXtimer++;

                //timer y
                if (moveYtimer >= 75)
                {

                    TiefightermoveY();
                    moveYtimer = 0;

                }

                moveYtimer++;

            }

            //level
            if (tiefighterPositioner.Count == 0)
            {

                RedrawTiefigther();
                level++;
                Showlevel++;
                Score += 200;
                

            }

            //shotspeed buff for admin
            if (tangentbord.IsKeyDown(Keys.K))
            {

                shotspeed = 80;

            }

            //xwing movement
            checktangentbord = tangentbord;
            tangentbord = Keyboard.GetState();
            

            if ((tangentbord.IsKeyDown(Keys.Left) || tangentbord.IsKeyDown(Keys.A)) && xwingRectangle.X > (0))
            {
                xwingRectangle.X -= xwingspeed;
            }
            if ((tangentbord.IsKeyDown(Keys.Right) || tangentbord.IsKeyDown(Keys.D)) && xwingRectangle.X < (windowWidth - 128))
            {
                xwingRectangle.X += xwingspeed;
            }

             //shoting
            if (tangentbord.IsKeyDown(Keys.Space) && checktangentbord.IsKeyUp(Keys.Space) && Shotrect.Y < 0)
            {
                shots = true;
            }

            //score
            meddelande = $"Score: {Score}";

            //level
            levelmeddelande = $"Level: {Showlevel}";


        }

        void Updateloseending()
        {

            if (VänsterMusTryckt() && Yesknapprect.Contains(mus.Position))
            {

                Reset();
                SwitchScene(1);

            }

            if (VänsterMusTryckt() && Noknapprect.Contains(mus.Position))
            {

                Exit();

            }

        }

        void Drawmeny()
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(Spacebild, Spacepos, Color.White);
            _spriteBatch.Draw(Spacebild2, Spacepos2, Color.White);
            _spriteBatch.Draw(loga, logarect, Color.White);
            _spriteBatch.Draw(knappbild, knapprect, Color.White);

            _spriteBatch.End();
        }

        void Drawgame()
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(Spacebild, Spacepos, Color.White);
            _spriteBatch.Draw(Spacebild2, Spacepos2, Color.White);
            _spriteBatch.Draw(greenshot, greenshotrect, Color.White);
            _spriteBatch.Draw(Shot, Shotrect, Color.White);
            _spriteBatch.Draw(xwingbild, xwingRectangle, Color.White);
            _spriteBatch.DrawString(arialFont, meddelande, meddelandePosition, Color.White);
            _spriteBatch.DrawString(arialFontlevel, levelmeddelande, LevelPosition, Color.White);

            foreach (Rectangle tiefighterRectangle in tiefighterPositioner)
            {
                _spriteBatch.Draw(tiefigtherbild, tiefighterRectangle, Color.White);
            }

            foreach (Rectangle space in XwingRectangleLives)
            {
                _spriteBatch.Draw(Lives, space, Color.LightBlue);
            }

            _spriteBatch.End();

        }

        void Drawloseending()
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(Spacebild, Spacepos, Color.White);
            _spriteBatch.Draw(Spacebild2, Spacepos2, Color.White);
            _spriteBatch.Draw(GameOver, GameOverRect, Color.White);
            _spriteBatch.DrawString(arialFontPlayagain, Playagainmeddelande, PlayagainPosition, Color.White);
            _spriteBatch.Draw(Yesknappbild, Yesknapprect, Color.White);
            _spriteBatch.Draw(Noknappbild, Noknapprect, Color.White);
            _spriteBatch.DrawString(arialFont, meddelande, meddelandePosition, Color.White);

            _spriteBatch.End();

        }

    }
}
