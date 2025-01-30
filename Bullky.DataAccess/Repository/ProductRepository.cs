using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository(ApplicationDbContext db) : Repository<Product>(db),IProductsRepository
    {
        private readonly ApplicationDbContext _db = db;

        public void update(Product product)
        {
            _db.Products.Update(product);
        }
    }
}
