using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class WordRepository : Repository<Word>
    {
        public WordRepository(EnglishBySongsDbContext db) : base(db)
        {

        }

        public override Word Get(int id)
        {
            return _db.Words.Find(id);
        }

        public override Word Get(Expression<Func<Word, bool>> predicate)
        {
            return _db.Words.Include(w1 => w1.Translations).Include(w1 => w1.Songs).FirstOrDefault(predicate);
        }

        public override async Task<Word> GetAsync(Expression<Func<Word, bool>> predicate)
        {
            return await _db.Words.Include(w1 => w1.Translations).Include(w1 => w1.Songs).FirstOrDefaultAsync(predicate);
        }

        public override List<Word> GetAll()
        {
            return _db.Words.AsNoTracking()
                .Include(w => w.Songs).AsNoTracking()
                .Include(w => w.Translations).AsNoTracking()
                .ToList();
        }

        public override List<Word> GetAll(Expression<Func<Word, bool>> predicate)
        {
            return _db.Words.AsNoTracking()
                .Include(w => w.Songs).AsNoTracking()
                .Include(w => w.Translations).AsNoTracking()
                .Where(predicate).AsNoTracking()
                .ToList();
        }

        public override async Task<List<Word>> GetAllAsync()
        {
            return await _db.Words.AsNoTracking()
                .Include(w => w.Songs).AsNoTracking()
                .Include(w => w.Translations).AsNoTracking()
                .ToListAsync();
        }

        public override async Task<List<Word>> GetAllAsync(Expression<Func<Word, bool>> predicate)
        {
            return await _db.Words.AsNoTracking()
                .Include(w => w.Songs).AsNoTracking()
                .Include(w => w.Translations).AsNoTracking()
                .Where(predicate).AsNoTracking()
                .ToListAsync();
        }

        public override void Remove(Word item)
        {
            _db.Words.Remove(_db.Words.Find(item.Id));
        }
    }
}
