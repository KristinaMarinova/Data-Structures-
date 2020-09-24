namespace _01.Loader
{
    using _01.Loader.Interfaces;
    using _01.Loader.Models;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Loader : IBuffer
    {
        private List<IEntity> _entities;

        public Loader()
        {
            this._entities = new List<IEntity>();
        }
        public int EntitiesCount => this._entities.Count;

        public void Add(IEntity entity)
        {
            this._entities.Add(entity);
        }

        public void Clear()
        {
            this._entities.Clear();
        }

        public bool Contains(IEntity entity)
        {
            return this.GetById(entity.Id) != null;
        }

        public IEntity Extract(int id)
        {
            IEntity found = this.GetById(id);
            if (found != null)
            {
                this._entities.Remove(found);
            }
            return found;
        }

        public IEntity Find(IEntity entity)
        {
            return this.GetById(entity.Id);
        }

        public List<IEntity> GetAll()
        {
            return this._entities;
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void RemoveSold()
        {
            throw new NotImplementedException();
        }

        public void Replace(IEntity oldEntity, IEntity newEntity)
        {
            int indexOfEntity = this._entities.IndexOf(oldEntity);
            this.ValidateEntity(indexOfEntity);
            this._entities[indexOfEntity] = newEntity;
        }

        public List<IEntity> RetainAllFromTo(BaseEntityStatus lowerBound, BaseEntityStatus upperBound)
        {
            throw new NotImplementedException();
        }

        public void Swap(IEntity first, IEntity second)
        {
            throw new NotImplementedException();
        }

        public IEntity[] ToArray()
        {
            throw new NotImplementedException();
        }

        public void UpdateAll(BaseEntityStatus oldStatus, BaseEntityStatus newStatus)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private IEntity GetById(int id)
        {
            for (int i = 0; i < this.EntitiesCount; i++)
            {
                var currentEntity = this._entities[i];

                if (currentEntity.Id == id)
                {
                    return currentEntity;
                }
            }

            return null;
        }

        private void ValidateEntity(int index)
        {
            if (index == -1)
            {
                throw new InvalidOperationException("Entity not found");
            }
        }
    }
}
