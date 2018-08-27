using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private eCommerceContext _context;

        public ProductRepository(eCommerceContext context)
        {
            _context = context;
        }

        public void AddPriceToProduct(Price price)
        {
            _context.Price.Add(price);
        }

        public void AddProduct(Product product)
        {
            _context.Product.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            product.IsDeleted = true;
            EditProduct(product);
        }

        public void EditProduct(Product product)
        {
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void EditProductPrice(Price price)
        {
            _context.Entry(price).State = EntityState.Modified;
        }

        public Price GetCurrentPrice(int productId)
        {
            return GetPricesOfProduct(productId).FirstOrDefault(p => p.IsSale == false && p.IsActive == true);
        }

        public IEnumerable<Price> GetPricesOfProduct(int productId)
        {
            return _context.Price.Where(p => p.IsActive == true && p.ProductId == productId);
        }

        public Product GetProduct(int productId)
        {
            return GetProducts().FirstOrDefault(p => p.Id == productId);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Product.Include(p => p.ProductCategory).Where(p => p.IsDeleted == false).OrderByDescending(p => p.Id);
        }

        public Price GetSalePrice(int productId)
        {
            return GetPricesOfProduct(productId).FirstOrDefault(p => p.IsSale == true && p.IsActive == true);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
