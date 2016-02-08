using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.System;
using SFML.Window;
using SFML.Graphics;

namespace SFML_IG
{
    class Menu
    {

        private Font font;
        private Text title;
        private Text[] options;
        private Text pointer;
        private int selected;

        public Menu()
        {

            selected = 0;

            font = new Font("sansation.ttf");

            title = new Text("INVASION GRID", font);
            title.Position = new Vector2f(50.0f, 10.0f);
            title.CharacterSize = 40;

            options = new Text[4];

            options[0] = new Text("Easy Mode", font);
            options[1] = new Text("HARDCORE MODE", font);
            options[2] = new Text("High Scores", font);
            options[3] = new Text("Quit", font);

            for(int i = 0; i < 4; i++)
            {
                options[i].Position = new Vector2f(50.0f, 100.0f * (i + 1));
                options[i].Color = Color.Red;
            }

            pointer = new Text(">", font);
            pointer.Position = new Vector2f(30.0f, 100.0f);
            pointer.Color = Color.White;

        }

        public void Draw(RenderWindow window)
        {
            window.Draw(title);

            for(int i = 0; i < 4; i++)
            {
                window.Draw(options[i]);
            }

            window.Draw(pointer);
        }

        public bool HandleInput(KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Return)
            {
                if(selected == 3)
                {
                    return false;
                }
            }

            if(e.Code == Keyboard.Key.Down)
            {
                if(selected < 3)
                {
                    selected++;
                }
                else
                {
                    selected = 0;
                }
                pointer.Position = new Vector2f(30.0f, 100.0f * (selected + 1));
            }
            else if(e.Code == Keyboard.Key.Up)
            {
                if (selected > 0)
                {
                    selected--;
                }
                else
                {
                    selected = 3;
                }
                pointer.Position = new Vector2f(30.0f, 100.0f * (selected + 1));
            }
            return true;
        }
    }
}
