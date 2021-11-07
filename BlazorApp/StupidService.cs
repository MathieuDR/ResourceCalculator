using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace BlazorApp {
    public class StupidService {
        private readonly Repository _repository;

        public StupidService(Repository repository) {
            _repository = repository;
        }
        
        public ValueTask<T> Get<T>(string key) {
            return _repository.Get<T>(key);
        }
        
        public  ValueTask<string> GetAsString(string key) {
            return  _repository.GetAsString(key);
        }
        
        public ValueTask Set(string key, object value){
            return _repository.Set(key, value);
        }
    }


    public class Repository {
        public Repository(ILocalStorageService localStorageService) {  
            _localStorageService = localStorageService;
        }

        private ILocalStorageService _localStorageService;

        public ValueTask<T> Get<T>(string key) {
            return _localStorageService.GetItemAsync<T>(key);
        }
        
        public  ValueTask<string> GetAsString(string key) {
            return  _localStorageService.GetItemAsStringAsync(key);
        }
        
        public ValueTask Set(string key, object value){
            return _localStorageService.SetItemAsync(key, value);
        }
    }
}