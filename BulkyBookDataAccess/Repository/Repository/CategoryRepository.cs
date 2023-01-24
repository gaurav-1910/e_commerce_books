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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
      
        public void Update(Category _category)
        {
            _db.Categories.Update(_category);
        }
    }
}
