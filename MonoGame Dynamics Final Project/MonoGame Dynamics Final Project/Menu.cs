#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame_Dynamics_Final_Project.Sprites;
using MonoGame_Dynamics_Final_Project.Weapons;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    public class Menu
    {
        SoundEffect select;
        SoundEffect click;

        private string[] menuItems;
        private int selIndex;

        private Color normal = Color.BlueViolet;
        private Color selected = Color.Coral;

        private KeyboardState preKeyState;
        private KeyboardState keyState;
        private GamePadState gpState;
        private GamePadState pregpState;
        private SpriteFont spriteFont;

        public int itemSelected = 0;

        private Vector2 position;
        private float width = 0f;
        private float height = 0f;

        public int SelIndex
        {
            get { return selIndex; }
            set
            {
                selIndex = value;
                //keep in range of menu items
                if (selIndex < 0)
                    selIndex = 0;
                if (selIndex >= menuItems.Length)
                    selIndex = menuItems.Length - 1;
            }
        }

        public int ItemSelected
        {
            get { return itemSelected; }
            set { itemSelected = value; }
        }

        public Menu(GraphicsDevice Device, ContentManager Content, SpriteFont _spriteFont, string[] _menuItems)
        {
            spriteFont = _spriteFont;
            menuItems = _menuItems;
            select = Content.Load<SoundEffect>("Audio Files/Sound Effects/powerup_gained something");
            click = Content.Load<SoundEffect>("Audio Files/Sound Effects/click");
            MeasureMenu(Device);
        }

        public void MeasureMenu(GraphicsDevice Device)
        {
            height = 0;
            width = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = spriteFont.MeasureString(item);
                //save width of widest
                if (size.X > width)
                    width = size.X;
                //add all heights
                height += spriteFont.LineSpacing;
            }
            position = new Vector2((Device.Viewport.Width - width) / 6, (Device.Viewport.Height - height) / 2);
        }

        private bool CheckKey(Keys theKey)
        {
            return (keyState.IsKeyUp(theKey) && preKeyState.IsKeyDown(theKey));
        }
        private bool CheckPad(Buttons button)
        {
            return (gpState.IsButtonUp(button) && pregpState.IsButtonDown(button));
        }

        public void MenuSelect()
        {
            keyState = Keyboard.GetState();

            //change index based on up or down getting pressed
            if (CheckKey(Keys.Down) || CheckPad(Buttons.DPadDown))
            {
                click.Play();
                SelIndex++;
                if (selIndex == menuItems.Length)
                {
                    SelIndex = 0;
                }
            }
            if (CheckKey(Keys.Up) || CheckPad(Buttons.DPadUp))
            {
                click.Play();
                SelIndex--;
                if (SelIndex < 0)
                {
                    SelIndex = menuItems.Length - 1;
                }
            }
            if (keyState.IsKeyDown(Keys.Enter) || gpState.IsButtonDown(Buttons.A))
            {
                select.Play();
                ItemSelected = SelIndex + 1;
            }
            preKeyState = keyState;
        }
        public void Update()
        {
            MenuSelect();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 location = position;
            Color hilight;
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == SelIndex)
                {
                    hilight = selected;
                }
                else
                {
                    hilight = normal;
                }
                spriteBatch.DrawString(spriteFont, menuItems[i], location + new Vector2(100, 70), hilight, 0.0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0.0f);
                location.Y += spriteFont.LineSpacing + 50;
            }
        }
    }
}