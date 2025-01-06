using BankingAppBackend.DAL;
using Microsoft.EntityFrameworkCore;

namespace Backend_BankingApp.Utilities.Flyweight
{
     public interface IServiceFlyweight
     {
          string IdService { get; }
          string Title { get; }
          string Subtitle { get; }
          string Emblem { get; }
          bool IsValid();
     }

     public class ServiceFlyweight : IServiceFlyweight
     {
          public string IdService { get; }
          public string Title { get; }
          public string Subtitle { get; }
          public string Emblem { get; } 

          public ServiceFlyweight(string idService, string title, string subtitle, string emblem)
          {
               IdService = idService;
               Title = title;
               Subtitle = subtitle;
               Emblem = emblem;
          }

          public bool IsValid()
          {
               return !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(IdService);
          }
     }

     public class ServiceFlyweightFactory
     {
          private readonly Dictionary<string, IServiceFlyweight> _cache = new();
          private DateTime _lastUpdated;
          private readonly TimeSpan _cacheExpirationTime = TimeSpan.FromMinutes(5);
          private readonly BankDbContext _context;

          public ServiceFlyweightFactory(BankDbContext context)
          {
               _context = context;
               _lastUpdated = DateTime.MinValue;
          }

          public async Task<IServiceFlyweight> GetServiceByIdAsync(string id)
          {
               if (DateTime.Now - _lastUpdated > _cacheExpirationTime || !_cache.ContainsKey(id))
               {
                    var service = await _context.Services.FindAsync(id);
                    if (service != null)
                    {
                         _cache[id] = new ServiceFlyweight(service.IdService, service.Title, service.Subtitle, service.Emblem);
                         _lastUpdated = DateTime.Now;
                    }
               }

               return _cache.ContainsKey(id) ? _cache[id] : null;
          }

          public async Task<List<IServiceFlyweight>> GetAllServicesAsync()
          {
               if (DateTime.Now - _lastUpdated > _cacheExpirationTime || _cache.Count == 0)
               {
                    var services = await _context.Services.ToListAsync();
                    _cache.Clear();
                    foreach (var service in services)
                    {
                         _cache[service.IdService] = new ServiceFlyweight(service.IdService, service.Title, service.Subtitle, service.Emblem);
                    }
                    _lastUpdated = DateTime.Now;
               }

               return _cache.Values.ToList();
          }
     }

}

