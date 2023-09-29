using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSpy
{
    internal static class GameFont
    {
        static Texture2D Font;
        static Dictionary<char, int> ListOffsets = new Dictionary<char, int>();

        public static void Initialize(Texture2D font)
        {
            Font = font;

            ListOffsets.Add('A', 0);
            ListOffsets.Add('B', 8);
            ListOffsets.Add('C', 16);
            ListOffsets.Add('D', 24);
            ListOffsets.Add('E', 32);
            ListOffsets.Add('F', 40);
            ListOffsets.Add('G', 48);
            ListOffsets.Add('H', 56);
            ListOffsets.Add('I', 64);
            ListOffsets.Add('J', 72);
            ListOffsets.Add('K', 80);
            ListOffsets.Add('L', 88);
            ListOffsets.Add('M', 96);
            ListOffsets.Add('N', 104);
            ListOffsets.Add('O', 112);
            ListOffsets.Add('P', 120);
            ListOffsets.Add('Q', 128);
            ListOffsets.Add('R', 136);
            ListOffsets.Add('S', 144);
            ListOffsets.Add('T', 152);
            ListOffsets.Add('U', 160);
            ListOffsets.Add('V', 168);
            ListOffsets.Add('W', 176);
            ListOffsets.Add('X', 184);
            ListOffsets.Add('Y', 192);
            ListOffsets.Add('Z', 200);
            ListOffsets.Add('0', 208);
            ListOffsets.Add('1', 216);
            ListOffsets.Add('2', 224);
            ListOffsets.Add('3', 232);
            ListOffsets.Add('4', 240);
            ListOffsets.Add('5', 248);
            ListOffsets.Add('6', 256);
            ListOffsets.Add('7', 264);
            ListOffsets.Add('8', 272);
            ListOffsets.Add('9', 280);
            ListOffsets.Add('.', 288);
            ListOffsets.Add(',', 296);
            ListOffsets.Add(':', 304);
            ListOffsets.Add(';', 312);
            ListOffsets.Add('!', 320);
            ListOffsets.Add('?', 328);
            ListOffsets.Add('-', 336);
            ListOffsets.Add('[', 344);
            ListOffsets.Add('(', 344);
            ListOffsets.Add(']', 352);
            ListOffsets.Add(')', 352);
            ListOffsets.Add('"', 360);
            ListOffsets.Add('\'', 360);
            ListOffsets.Add(' ', 368);
        }
        public static void DrawText(SpriteBatch spriteBatch,Vector2 vector,  string text, float scale)
        {
            text = text.ToUpper();
            int yOffset = (int)vector.Y;
            int xOffset = 0;
            for (int i = 0; i < text.Length; i++) 
            {
                int fontOffset;
                if(!ListOffsets.TryGetValue(text[i], out fontOffset))
                {
                    fontOffset = 328;
                }
                if (text[i] == '\n')
                {
                    yOffset += 8 * (int)scale;
                    xOffset = 0;
                }
                else
                {
                    spriteBatch.Draw(Font, new Vector2(vector.X + ((8 * scale) * xOffset), yOffset), new Rectangle(fontOffset, 0, 7, 7), Color.White, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                    xOffset++;
                }
            }
        }
    }
}
