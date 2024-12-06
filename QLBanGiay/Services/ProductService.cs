using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models.Models;
using QLBanGiay.Repository.IRepository;

namespace QLBanGiay.Services
{
	public class ProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<object> GetProductsAsync(int page, int pageSize)
		{
			var totalItems = await _productRepository.GetTotalProductsAsync();
			var products = await _productRepository.GetProductsAsync(page, pageSize);

			var data = products.Select(p => new
			{
				ProductId = p.Productid,
				ProductName = p.Productname,
				p.Price,
				p.Discount,
				p.Image,
				IsActive = p.Isactive,
				Category = p.Category == null ? null : new
				{
					CategoryId = p.Category.Categoryid,
					CategoryName = p.Category.Categoryname,
					ParentCategory = p.Category.Parentcategory == null ? null : new
					{
						ParentCategoryId = p.Category.Parentcategory.Parentcategoryid,
						ParentCategoryName = p.Category.Parentcategory.Parentcategoryname
					}
				}
			});

			return new
			{
				Data = data,
				CurrentPage = page,
				PageSize = pageSize,
				TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
				TotalItems = totalItems
			};
		}

		public async Task<object> GetProductByIdAsync(int id)
		{
			var product = await _productRepository.GetProductByIdAsync(id);

			if (product == null) return null;

			return new
			{
				ProductId = product.Productid,
				ProductName = product.Productname,
				product.Price,
				product.Discount,
				product.Image,
				product.Productdescription,
				IsActive = product.Isactive,
				Category = product.Category == null ? null : new
				{
					CategoryId = product.Category.Categoryid,
					CategoryName = product.Category.Categoryname,
					ParentCategory = product.Category.Parentcategory == null ? null : new
					{
						ParentCategoryId = product.Category.Parentcategory.Parentcategoryid,
						ParentCategoryName = product.Category.Parentcategory.Parentcategoryname
					}
				},
				Sizes = product.ProductSizes.Select(size => new
				{
					SizeId = size.ProductSizeId,
					Size = size.Size
				}).ToList()
			};
		}

	}
}
