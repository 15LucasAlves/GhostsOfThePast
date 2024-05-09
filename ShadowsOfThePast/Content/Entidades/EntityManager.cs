using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowsOfThePast.Content.Entidades
{
    public class EntityManager
    {
        // Create list for all the entities, the ones that are on the screen, the ones that are going to be added and the ones that are going to be removed
        readonly List<GameEntity> entities = new List<GameEntity>();
        readonly List<GameEntity> toRemove = new List<GameEntity>();
        readonly List<GameEntity> toAdd = new List<GameEntity>();

        // Constructor
        public EntityManager()
        {


        }
    }
}
