using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace BruteForce
{
    public class Camera
    {
        public Character myPlayer;
        public Matrix Transform;
        Viewport View;
        public Vector2 center;

        public Camera(Viewport newView)
        {
            View = newView;
        }
        public void Update(GameTime gametime)
        {
            center = new Vector2(myPlayer.position.X - 640/* + 200*/, 0);

            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
        public void start()
        {
            center = new Vector2(myPlayer.position.X, 0);

            Transform = Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
        }
    }

}
