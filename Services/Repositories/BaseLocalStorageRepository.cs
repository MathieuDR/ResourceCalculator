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
        protected ILocalStorageService LocalStorageService { get; }

        protected abstract string RepositoryPrefix { get; }
        protected string KeysKey => $"{RepositoryPrefix}_{KeySuffix}";

        public BaseLocalStorageRepository(ILogger logger, ILocalStorageService localStorageService) {
            Logger = logger;
            LocalStorageService = localStorageService;
        }

        public Task<T> Get(Guid id) {
            return LocalStorageService.GetItemAsync<T>(GetKey(id)).AsTask();
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
            var task = LocalStorageService.SetItemAsync(GetKey(entity.Id), entity);
            await AddKey(entity);
            await task;

            return entity;
        }

        private async Task AddKey(T entity) {
            var keys = (await GetAllKeys() ?? Array.Empty<Guid>()).ToList();
            keys.Add(entity.Id);
            await LocalStorageService.SetItemAsync(KeysKey, keys.ToArray());
        }

        // remove key
        public async Task Remove(Guid id) {
            var keys = (await GetAllKeys() ?? Array.Empty<Guid>()).ToList();
            keys.Remove(id);
            await LocalStorageService.SetItemAsync(KeysKey, keys.ToArray());
            await LocalStorageService.RemoveItemAsync(GetKey(id));
        }

        public async Task<T> Update(T entity) {
            await LocalStorageService.SetItemAsync(GetKey(entity.Id), entity);
            return entity;
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
            await LocalStorageService.SetItemAsync(KeysKey, keys.ToArray());
            await LocalStorageService.RemoveItemAsync(GetKey(id));

            return true;
        }

        public Task<bool> Delete(T entity) {
            return Delete(entity.Id);
        }

        protected async Task<IEnumerable<Guid>> GetAllKeys() {
            return await LocalStorageService.GetItemAsync<IEnumerable<Guid>>(KeysKey);
        }

        // checks it em exists
        protected Task<bool> Exists(Guid id) {
            return LocalStorageService.ContainKeyAsync(GetKey(id)).AsTask();
        }

        protected string GetKey(Guid id) {
            return $"{RepositoryPrefix}_{id}";
        }
    }
}