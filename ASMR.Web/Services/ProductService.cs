//
// asmr: Coffee Beans Management Solution
// © 2021 Pandora Karya Digital. All right reserved.
//
// Written by Danang Galuh Tegar Prasetyo [connect@danang.id]
//
// ProductService.cs
//

using System.Linq;
using System.Threading.Tasks;
using ASMR.Core.Entities;
using ASMR.Web.Data;
using ASMR.Web.Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace ASMR.Web.Services;

public interface IProductService : IServiceBase
{
	public IQueryable<Product> GetAllProducts();

	public IQueryable<Product> GetProductsByBeanId(string id);

	public Task<Product> GetProductById(string id, bool includeSensitiveInformation = true);

	public Task<Product> CreateProduct(Product product);

	public Task<Product> ModifyProduct(string id, Product product);

	public Task<Product> RemoveProduct(string id);
}

public class ProductService : ServiceBase, IProductService
{
	public ProductService(ApplicationDbContext dbContext) : base(dbContext)
	{
	}

	public IQueryable<Product> GetAllProducts()
	{
		return DbContext.Products
			.Include(e => e.Bean)
			.AsQueryable();
	}

	public IQueryable<Product> GetProductsByBeanId(string id)
	{
		return DbContext.Products
			.Where(e => e.BeanId == id)
			.Include(e => e.Bean)
			.AsQueryable();
	}

	public Task<Product> GetProductById(string id, bool includeSensitiveInformation = true)
	{
		var products = DbContext.Products
			.Where(e => e.Id == id)
			.Include(e => e.Bean);
		return includeSensitiveInformation
			? products.ThenInclude(e => e.Inventory)
				.FirstOrDefaultAsync()
			: products.FirstOrDefaultAsync();
	}

	public async Task<Product> CreateProduct(Product product)
	{
		var entityEntry = await DbContext.Products.AddAsync(product);
		return entityEntry.Entity;
	}

	public async Task<Product> ModifyProduct(string id, Product product)
	{
		var entity = await DbContext.Products
			.Where(e => e.Id == id)
			.Include(e => e.Bean)
			.FirstOrDefaultAsync();
		if (entity is null)
		{
			return null;
		}

		if (product.CurrentInventoryQuantity >= 0)
		{
			entity.CurrentInventoryQuantity = product.CurrentInventoryQuantity;
		}

		if (product.Price >= 0)
		{
			entity.Price = product.Price;
		}

		if (product.WeightPerPackaging > 0)
		{
			entity.WeightPerPackaging = product.WeightPerPackaging;
		}

		DbContext.Products.Update(entity);
		return entity;
	}

	public async Task<Product> RemoveProduct(string id)
	{
		var entity = await DbContext.Products
			.Where(e => e.Id == id)
			.Include(e => e.Bean)
			.FirstOrDefaultAsync();
		if (entity is null)
		{
			return null;
		}

		DbContext.Products.Remove(entity);
		return entity;
	}
}