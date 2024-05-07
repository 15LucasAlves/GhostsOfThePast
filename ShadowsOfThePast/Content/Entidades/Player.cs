using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast
{
    public class Player
    {
        // Player constructor
        public Player()
        {
            // HP and MP meter so we know how much hits can the player take and how much spells can he cast
            int healthPoints = 3;
            int manaPoints = 10;
            // We need to know the player's position in the map to draw it in the right place on different levels
            int startingPosition;
            // We need to know the player's score to open the portal eventually
            int score;
            // We need to know if the player is alive or not so we can end the game
            bool isAlive;
        }


        // Player movement method
        public void Move()
        {

            // Check if the player should move right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Move the player right


            }

            // Check if the player should move left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Move the player left


            }

            // Check if the player should jump
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Move the player Up


            }
        }


        // Player fire spell method
        public void Attack()
        {
            // Check if the player should fire spell
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                // Fire the spell


            }
        }


        // Player swap spell method
        public void SwapSpell()
        {
            // Check if the player should swap spell
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                // Swap the spell


            }
        }


        // Player take damage method
        public void TakeDamage(Player player)
        {
            // Deal damage to the player
            player.healhPoints -= 1;

            // Check if the player should die
            if (healthPoints <= 0)
            {
                // Player is dead
                isAlive = false;
            }
        }

    }
}
