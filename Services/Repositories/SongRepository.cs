using Dal;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class SongRepository : Repository<Song>
    {
        public SongRepository() : base()
        {
        }

        public override List<Song> GetAll()
        {
            return _db.Songs.AsNoTracking().Include(w => w.Words).AsNoTracking().ToList();
        }

        public override List<Song> GetAll(Expression<Func<Song, bool>> predicate)
        {
            return _db.Songs.Include(w => w.Words).Where(predicate).ToList();
        }

        public override async Task<List<Song>> GetAllAsync()
        {
            return await _db.Songs.Include(w => w.Words).ToListAsync();
        }

        public override async Task<List<Song>> GetAllAsync(Expression<Func<Song, bool>> predicate)
        {
            return await _db.Songs.Include(w => w.Words).Where(predicate).ToListAsync();
        }

        public override void Remove(Song item)
        {
            // TODO: try to change
            _db.Songs.Remove(_db.Songs.Find(item.Id));
        }
    }
}
