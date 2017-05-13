using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{
        public override IQueryable<Product> All()
        {
            return this.Where(x => x.Is�R��);
        }

        public Product Get�浧���ByProductId(int id)
        {
            return this.All().FirstOrDefault(x => x.ProductId == id);
        }

        public IQueryable<Product> GetProduct�C���Ҧ����(bool Active, bool showAll = false)
        {
            IQueryable<Product> all = this.All();
            if (showAll)
            {
                all = base.All();
            }

            return this.All()
                .Where(p => p.Active.HasValue && p.Active.Value == Active)
                .OrderByDescending(p => p.ProductId).Take(10);
        }
	}

	public  interface IProductRepository : IRepository<Product>
	{

	}
}