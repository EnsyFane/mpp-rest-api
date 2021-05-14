using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Match> GetAll()
        {
            return _context.Matches.ToList();
        }

        public Match GetById(int id)
        {
            return _context.Matches.FirstOrDefault(m => m.Id == id);
        }

        public bool SaveChages()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Update(Match entity)
        {
        }
    }
}