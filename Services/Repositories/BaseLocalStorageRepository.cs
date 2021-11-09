using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using Services.Models;

namespace Services.Repositories {
    internal abstract class BaseLocalStorageRepository<T> : IRepository<T> where T : Entity {
        protected readonly string KeySuffix = "keys";
        protected ILogger Logger { get; }
        protected ISyncLocalStorageService LocalStorageService { get; }

        protected abstract string RepositoryPrefix { get; }
        protected string KeysKey => $"{RepositoryPrefix}_{KeySuffix}";

        public BaseLocalStorageRepository(ILogger logger, ISyncLocalStorageService localStorageService) {
            Logger = logger;
            LocalStorageService = localStorageService;
        }

        public Task<T> Get(Guid id) {
            return Task.FromResult(LocalStorageService.GetItem<T>(GetKey(id)));
        }

        public async Task<IEnumerable<T>> GetAll() {
            var keys = await GetAllKeys();
            if (keys is null) {
                return Array.Empty<T>();
            }

            return await GetEntities(keys);
        }

        public IAsyncEnumerable<T> GetAllAsync() {
            var keys = GetAllKeys().GetAwaiter().GetResult();

            if (keys is null) {
                return AsyncEnumerable.Empty<T>();
            }

            return GetEntitiesAsync(keys);
        }

        private async Task<IEnumerable<T>> GetEntities(IEnumerable<Guid> keys) {
            return await GetEntitiesAsync(keys).ToListAsync();
        }


        private async IAsyncEnumerable<T> GetEntitiesAsync(IEnumerable<Guid> keys) {
            foreach (var key in keys) {
                yield return await Get(key);
            }
        }

        public async Task<T> Add(T entity) {
            LocalStorageService.SetItem(GetKey(entity.Id), entity);
            await AddKey(entity);

            return entity;
        }
        
        public async Task Add(IEnumerable<T> entities) {
            var entitiesArr = entities as T[] ?? entities.ToArray();
            var keyTask = AddKeys(entitiesArr);
            foreach (var entity in entitiesArr) {
                 LocalStorageService.SetItem(GetKey(entity.Id), entity);
            }

            await keyTask;
        }

        private async Task AddKey(T entity) {
            var keys = (await GetAllKeys() ?? Array.Empty<Guid>()).ToList();
            keys.Add(entity.Id);
            LocalStorageService.SetItem(KeysKey, keys.ToArray());
        }
        
        private async Task AddKeys(IEnumerable<T> entity) {
            var keys = (await GetAllKeys() ?? Array.Empty<Guid>()).ToList();
            keys.AddRange(entity.Select(x=>x.Id));
            LocalStorageService.SetItem(KeysKey, keys.ToArray());
        }

        // remove key
        public async Task Remove(Guid id) {
            var keys = (await GetAllKeys() ?? Array.Empty<Guid>()).ToList();
            keys.Remove(id);
            LocalStorageService.SetItem(KeysKey, keys.ToArray());
            LocalStorageService.RemoveItem(GetKey(id));
        }

        public Task<T> Update(T entity) {
            LocalStorageService.SetItem(GetKey(entity.Id), entity);
            return Task.FromResult(entity);
        }

        public async Task<T> Upsert(T entity) {
            if (await Exists(entity.Id)) {
                return await Update(entity);
            }

            return await Add(entity);
        }

        public async Task<bool> Delete(Guid id) {
            var keys = (await GetAllKeys() ?? Array.Empty<Guid>()).ToList();

            if (!keys.Contains(id)) {
                // Return if it doesn't exist
                return true;
            }

            keys.Remove(id);
            LocalStorageService.SetItem(KeysKey, keys.ToArray());
            LocalStorageService.RemoveItem(GetKey(id));

            return true;
        }

        public Task<bool> Delete(T entity) {
            return Delete(entity.Id);
        }

        public async Task<bool> SeedDatabase(T[] entities) {
            await Add(entities);
            return true;
        }

        protected async Task<IEnumerable<Guid>> GetAllKeys() {
            var keys = (LocalStorageService.GetItem<IEnumerable<Guid>>(KeysKey) ?? Array.Empty<Guid>()).ToList();
            Logger.LogInformation("Keys: " + string.Join(", ", keys.Select(x=>x.ToString())));
            return keys;
        }

        // checks it em exists
        protected Task<bool> Exists(Guid id) {
            return Task.FromResult(LocalStorageService.ContainKey(GetKey(id)));
        }

        protected string GetKey(Guid id) {
            return $"{RepositoryPrefix}_{id}";
        }
    }
}