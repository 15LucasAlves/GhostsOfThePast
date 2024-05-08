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

        public bool AddEntity(GameEntity entity)
        {
            // Check if the entity is null
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Check if the entity is already on the list
            if (HasEntity(entity))
            {
                return false;
            }

            // Add the entity to the list
            toAdd.Add(entity);

            return true;
        }


        public bool RemoveEntity(GameEntity entity)
        {
            // Check if the entity is null
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Check if the entity is already on the list
            if (!HasEntity(entity))
            {
                return false;
            }

            // Add the entity to the list
            toRemove.Add(entity);

            return true;
        }


        // Checks if there are entities on the list's
        public bool HasEntity(GameEntity entity) => entities.Contains(entity) || toAdd.Contains(entity) || toRemove.Contains(entity);
    }
}
