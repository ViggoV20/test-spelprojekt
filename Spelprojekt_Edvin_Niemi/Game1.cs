using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;

namespace Spelprojekt_Edvin_Niemi
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        Texture2D hero, enemy, shot;

        //text
        SpriteFont arialFont;
        //starta om spelet
        string startaOm = "Press R to restart";
        Vector2 startaOmPosition = new Vector2((1280 / 2)-210, (720 / 2)-50);
        //poäng
        int points = 0;
        Vector2 pointsPosition = new Vector2(50, 40);
        //tid
        int minut = 2;
        int sekunder = 0;
        int noll = 0;
        string tid2 = ":";
        Vector2 tid2Position = new Vector2(605, 36);
        Vector2 minutPosition = new Vector2(570, 40);
        Vector2 sekunderPosition = new Vector2(620, 40);
        Vector2 sekunderPosition2 = new Vector2(650, 40);   
        Vector2 nollPosition = new Vector2(620, 40);
        int sek = 0;
        int min = 0;
        bool tidSlut = false;

        //hero
        Rectangle heroRekt = new Rectangle(200,(780/2)-100,100,100);
        int heroHastighet = 5;

        //enemy
        Vector2 enemyPosition = new Vector2(800, 200);
        Rectangle enemyRekt = new Rectangle(980, (780/2)-100, 100, 100);
        Color enemyColor = Color.White;
        double enemyHastighet = 0.004;
        int enemyHP = 1;
        int enemyX = 0;
        int enemyY = 0;

        //shot
        List<Rectangle> skottLista = new List<Rectangle>();
        Rectangle shotRekt = new Rectangle(0, 0, 50, 50);
        int skottHastighet = 10;
        

        //Kontroller
        KeyboardState keyboard = Keyboard.GetState();
        KeyboardState oldKeyboard = Keyboard.GetState();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            arialFont = Content.Load<SpriteFont>("arial");
            hero = Content.Load<Texture2D>("hero");
            enemy = Content.Load<Texture2D>("enemy");
            shot = Content.Load<Texture2D>("shot");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //inputs
            oldKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            //restart
            if (keyboard.IsKeyUp(Keys.R) && oldKeyboard.IsKeyDown(Keys.R) && tidSlut == true)
            {
                tidSlut = false;
                minut = 1;
                points = 0;
            }

            if (!tidSlut)
            {
                //Hero kontroller
            if (keyboard.IsKeyDown(Keys.W) && heroRekt.Y > 80)
            {
                heroRekt.Y -= heroHastighet;
            }
            if (keyboard.IsKeyDown(Keys.S) && heroRekt.Y < 560)
            {
                heroRekt.Y += heroHastighet;
            }
            //if (keyboard.IsKeyDown(Keys.A))
            //{
            //    heroRekt.X -= heroHastighet;
            //}
            //if (keyboard.IsKeyDown(Keys.D))
            //{
            //    heroRekt.X += heroHastighet;
            //}
            //skott

            if (keyboard.IsKeyDown(Keys.Space) && oldKeyboard.IsKeyUp(Keys.Space))
            {
                Rectangle skott = new Rectangle(heroRekt.X + 100, heroRekt.Y + 25, 50, 50);
                skottLista.Add(skott);

            }
            for (int i = skottLista.Count - 1; i >= 0; i--)
            {
                Rectangle skott = skottLista[i];
                skott.X += skottHastighet;
                skottLista[i] = skott;

                //Enemy kollision

                if (skottLista[i].Intersects(enemyRekt))
                {
                    //enemyColor = Color.Red;
                    skottLista.RemoveAt(i);
                    enemyHP--;
                    points++;
                }
                else if (skott.X > graphics.PreferredBackBufferWidth)
                {
                    skottLista.RemoveAt(i);
                }
                else
                {
                    enemyColor = Color.White;
                }
            }

            //Enemy rörelser
            if (enemyHP == 0)
            {
                Random randomY = new Random();
                int Yposition = randomY.Next(80, 560);
                enemyRekt.Y = Yposition;
                enemyHP = 1;
            }


            //tid
          
                sek++;
                //räknar sekunder
                if (sek == 60)
                {
                    sekunder--;
                    sek = 0;
                }
                //räknar minuter
                if (sekunder < 0)
                {
                    if (minut > 0)
                    {
                        minut--;
                        sekunder = 60;
                        sekunder--;
                    }
                }
                if (sekunder == 0 && minut == 0)
                {
                    tidSlut = true;
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (var skott in skottLista)
            {
                spriteBatch.Draw(shot, skott, Color.Black);
            }
            spriteBatch.Draw(hero, heroRekt, Color.White);
            if (enemyHP > 0)
            {
                spriteBatch.Draw(enemy, enemyRekt, enemyColor);
            }
            spriteBatch.DrawString(arialFont, minut.ToString(), minutPosition, Color.White);
            if (sekunder > 9)
            {
                spriteBatch.DrawString(arialFont, sekunder.ToString(), sekunderPosition, Color.White);
            }
                spriteBatch.DrawString(arialFont, tid2, tid2Position, Color.White);
            if (sekunder < 10 || sekunder < 0)
            {
                spriteBatch.DrawString(arialFont, noll.ToString(), nollPosition, Color.White);
                spriteBatch.DrawString(arialFont, sekunder.ToString(), sekunderPosition2, Color.White);
            }
            if (tidSlut)
            {
                spriteBatch.DrawString(arialFont, startaOm, startaOmPosition, Color.White);
            }
            spriteBatch.DrawString(arialFont, points.ToString(), pointsPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
