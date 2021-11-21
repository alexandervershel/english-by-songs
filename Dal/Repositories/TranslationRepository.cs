using Entities;

namespace Dal.Repositories
{
    public class TranslationRepository : Repository<Translation>
    {
        public TranslationRepository(EnglishBySongsDbContext db) : base(db)
        {
        }
    }
}
