using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;
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

        Texture2D loga;
        Rectangle logarect;

        Texture2D Shot;
        Rectangle Shotrect;

        Texture2D greenshot;
        Rectangle greenshotrect;

        Vector2 Spacepos = new Vector2(0, 0);
        Vector2 Spacepos2 = new Vector2(0, -1200);

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
        int shotspeed = 12;
        bool shots = false;

        //score
        SpriteFont arialFont;
        string meddelande = $"Score:";
        Vector2 meddelandePosition = new Vector2(20, 20);

        //tiefigthermove
        int moveX = 20;
        int moveXtimer = 0;
        int moveY = 20;
        bool TiefighterY = false;
        int moveYtimer = 0;
        
        //score
        int Score = 0;
        

        //shoting teifitghter
        int Tiefightershot;
        int greenshotspeed = 16;


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

            //text
            arialFont = Content.Load<SpriteFont>("File");

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
                    break;
                case 2:
                    Updateending();
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
                    Drawending();
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

                greenshotrect.Y += greenshotspeed;

            }


        }

        protected void Intersectplayer()
        {

            if (greenshotrect.Intersects(xwingRectangle) == true)
            {

                SwitchScene(2);

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

                    temp.X += moveX;

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

        void Updategame()
        {

            MoveShot();
            FireShot();
            Intersecttiefighter();
            Tiefigthershoting();
            Intersectplayer();

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
                moveYtimer = 0 ;

            }

            moveYtimer++;
        }

        void Updateending()
        {



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

            foreach (Rectangle tiefighterRectangle in tiefighterPositioner)
            {
                _spriteBatch.Draw(tiefigtherbild, tiefighterRectangle, Color.White);
            }

            _spriteBatch.End();

        }

        void Drawending()
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(Spacebild, Spacepos, Color.White);
            _spriteBatch.Draw(Spacebild2, Spacepos2, Color.White);

            _spriteBatch.End();

        }
    }
}
