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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
      
        public void Update(Product _product)
        {
            _db.Products.Update(_product);// it will update all properties.
            var objFromDb = _db.Products.FirstOrDefault(u=>u.Id == _product.Id);
            if (objFromDb != null)
            { 
                objFromDb.Title = _product.Title;
                objFromDb.ISBN = _product.ISBN;
                objFromDb.Price = _product.Price;
                objFromDb.Description = _product.Description;
                objFromDb.ListPrice = _product.ListPrice;
                objFromDb.Price50= _product.Price50;
                objFromDb.Price100= _product.Price100;
                objFromDb.CategoryId = _product.CategoryId;
                objFromDb.Author = _product.Author;
                objFromDb.CoverTypeId = _product.CoverTypeId;
                if (_product.ImageUrl != null)
                { 
                    objFromDb.ImageUrl = _product.ImageUrl;
                }
            }
        }

    }
}





