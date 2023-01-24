using BulkyBookDataAccess.Repository.IRepository;
using BulkyBookModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBookDataAccess.Repository.Repository
{
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
      
        public void Update(CoverType _coverType)
        {
            _db.CoverTypes.Update(_coverType);
        }
    }
}
