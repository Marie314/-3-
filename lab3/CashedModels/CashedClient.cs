using Microsoft.Extensions.Caching.Memory;
using lab3.Models;

namespace lab3.CashedModels
{
    public class CashedClient
    {
        private readonly BarbershopContext _context;
        private readonly IMemoryCache _cache;
        private double time = 2 * 11 + 240;
        public CashedClient(BarbershopContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddClients(string cacheKey, int rowNumber)
        {
            IEnumerable<Client> storages = _context.Clients.Take(rowNumber).ToList();
            if (storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(time)
                });
            }
        }

        public IEnumerable<Client> GetClients(int rowNumber)
        {
            return _context.Clients.Take(rowNumber).ToList();
        }

        public IEnumerable<Client> GetClients(string cacheKey, int rowNumber)
        {
            IEnumerable<Client> storages;
            if (!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = _context.Clients.Take(rowNumber).ToList();
                if (storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(time)));
                }
            }
            return storages;
        }

        private string GetTable(IEnumerable<Client> clients)
        {
            string HtmlString = "<html><head><title>Клиенты</title>" +
                    "<meta charset='utf-8'></head>" +
                    "<body>" +
                        "<p><a href = '/'>На главную</a></p><br>" +
                        "<p>Список клиентов:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Имя</th>" +
                        "<th>фамилия</th>" +
                        "<th>Адрес</th>" +
                        "<th>Телефон</th>" +
                        "</tr>";
            foreach (var client in clients)
            {
                HtmlString += "<tr>" +
                $"<td>{client.Id}</td>" +
                $"<td>{client.Name}</td>" +
                $"<td>{client.Surname}</td>" +
                $"<td>{client.Address}</td>" +
                $"<td>{client.Telephone}</td>" +
                "</tr>";
            }
            HtmlString += "</table></body></html>";
            return HtmlString;
        }

        public string GetTable(string id)
        {
            var clients = _context.Clients.ToList().Where(x => x.Id == Convert.ToInt64(id));
            return GetTable(clients);
        }
    }
}
