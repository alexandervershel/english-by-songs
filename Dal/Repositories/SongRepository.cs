using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class SongRepository : Repository<Song>
    {
        public SongRepository(EnglishBySongsDbContext db) : base(db)
        {
        }

        //public override Song Get(int id)
        //{
        //    return _db.Songs.Find(id);
        //}

        //public override Song Get(Expression<Func<Song, bool>> predicate)
        //{
        //    return _db.Songs.FirstOrDefault(predicate);
        //}

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

        //public override async Task<Song> GetAsync(int id)
        //{
        //    return await _db.Songs.FindAsync(id);
        //}

        //public override async Task<Song> GetAsync(Expression<Func<Song, bool>> predicate)
        //{
        //    return await _db.Songs.FirstOrDefaultAsync(predicate);
        //}

        public override void Remove(Song item)
        {
            // TODO: try to change
            _db.Songs.Remove(_db.Songs.Find(item.Id));
        }
    }
}
