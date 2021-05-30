using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Persistence
{
    public class SqlMatchRepository : IMatchRepository
    {
        private readonly RestApiDBContext _context;

        public SqlMatchRepository(RestApiDBContext context)
        {
            _context = context;
        }

        public void Add(Match entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Matches.Add(entity);
        }

        public void Delete(Match entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Matches.Remove(entity);
        }

        public IEnumerable<Match> GetAll()
        {
            return _context.Matches.ToList();
        }

        public Match GetById(int id)
        {
            return _context.Matches.FirstOrDefault(m => m.Id == id);
        }

        public async Task<bool> SaveChagesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void Update(Match entity)
        {
        }
    }
}