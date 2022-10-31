using Microsoft.Extensions.Caching.Memory;
using lab3.Models;

namespace lab3.CashedModels
{
    public class CashedServiceKind
    {
        private readonly BarbershopContext _context;
        private readonly IMemoryCache _cache;
        private double time = 2 * 11 + 240;
        public CashedServiceKind(BarbershopContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void AddServiceKind(string cacheKey, int rowNumber)
        {
            IEnumerable<ServiceKind> storages = _context.ServiceKinds.Take(rowNumber).ToList();
            if (storages != null)
            {
                _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(time)
                });
            }
        }

        public IEnumerable<ServiceKind> GetServiceKind(int rowNumber)
        {
            return _context.ServiceKinds.Take(rowNumber).ToList();
        }

        public IEnumerable<ServiceKind> GetServiceKind(string cacheKey, int rowNumber)
        {
            IEnumerable<ServiceKind> storages;
            if (!_cache.TryGetValue(cacheKey, out storages))
            {
                storages = _context.ServiceKinds.Take(rowNumber).ToList();
                if (storages != null)
                {
                    _cache.Set(cacheKey, storages, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(time)));
                }
            }
            return storages;
        }

        private string GetTable(IEnumerable<ServiceKind> kinds)
        {
            string HtmlString = "<html><head><title>Main</title>" +
                    "<meta charset='utf-8'></head>" +
                    "<body>" +
                        "<p><a href = '/'>На главную</a></p><br>" +
                        "<p>Список типов сервиса:</p>" +
                        "<table border=1>" +
                        "<tr>" +
                        "<th>Id</th>" +
                        "<th>Название</th>" +
                        "<th>Описание</th>" +
                        "</tr>";
            foreach (var kind in kinds)
            {
                HtmlString += "<tr>" +
                $"<td>{kind.Id}</td>" +
                $"<td>{kind.Name}</td>" +
                $"<td>{kind.Description}</td>" +
                "</tr>";
            }
            HtmlString += "</table></body></html>";
            return HtmlString;
        }

        public string GetTable(string id)
        {
            var kinds = _context.ServiceKinds.ToList().Where(x => x.Id == Convert.ToInt64(id));
            return GetTable(kinds);
        }
    }
}
