using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast
{
    internal class Camera
    {
        public Vector2 position;


        public Camera(Vector2 position)
        {
            this.position = position;
        }

        public void followPlayer(Rectangle targetPlayer, Vector2 screenSize)
        {
            position = new Vector2(-targetPlayer.X + (screenSize.X / 2 - targetPlayer.Width / 2), -targetPlayer.Y + (screenSize.Y / 2 - targetPlayer.Height / 2));
        }

    }
}
