
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBanGiay.Attributes;
using QLBanGiay.Models.Models;

[AuthorizeUser]
public class ProductController : Controller
{
    private readonly HttpClient _httpClient;

    public ProductController()
    {
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> AllProduct(
    int page = 1,
    int pageSize = 12,
    string sortBy = "price", // Default sort by price
    string sortOrder = "asc", // Default sort order
    long? parentCategoryId = null,
    long? categoryId = null,
    decimal? priceMin = null,
    decimal? priceMax = null,
    string searchTerm = "")
    {
        // Lấy các tham số từ URL hiện tại
        var queryParameters = HttpContext.Request.Query
            .Where(q => !string.IsNullOrEmpty(q.Value))
            .ToDictionary(q => q.Key, q => q.Value.ToString());

        // Cập nhật hoặc thêm các tham số mới
        queryParameters["page"] = page.ToString();
        queryParameters["pageSize"] = pageSize.ToString();
        queryParameters["sortBy"] = sortBy;
        queryParameters["sortOrder"] = sortOrder;

        if (parentCategoryId.HasValue)
            queryParameters["parentCategoryId"] = parentCategoryId.Value.ToString();
        else
            queryParameters.Remove("parentCategoryId");

        if (categoryId.HasValue)
            queryParameters["categoryId"] = categoryId.Value.ToString();
        else
            queryParameters.Remove("categoryId");

        if (priceMin.HasValue)
            queryParameters["priceMin"] = priceMin.Value.ToString();
        else
            queryParameters.Remove("priceMin");

        if (priceMax.HasValue)
            queryParameters["priceMax"] = priceMax.Value.ToString();
        else
            queryParameters.Remove("priceMax");

        if (!string.IsNullOrEmpty(searchTerm))
            queryParameters["searchTerm"] = searchTerm;
        else
            queryParameters.Remove("searchTerm");

        // Xây dựng URL API với tham số query
        string apiUrl = $"https://localhost:7063/api/ProductApi?{string.Join("&", queryParameters.Select(q => $"{q.Key}={q.Value}"))}";

        // Gọi API
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonData);

            // Gán thông tin vào ViewBag
            ViewBag.CurrentPage = apiResponse.CurrentPage;
            ViewBag.TotalPages = apiResponse.TotalPages;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            ViewBag.ParentCategoryId = parentCategoryId;
            ViewBag.CategoryId = categoryId;
            ViewBag.PriceMin = priceMin;
            ViewBag.PriceMax = priceMax;
            ViewBag.SearchTerm = searchTerm;

            return View(apiResponse.Data); // Truyền danh sách sản phẩm vào View
        }

        return View(new List<Product>()); // Trả về danh sách rỗng nếu lỗi
    }


    // Lớp phản hồi từ API
    public class ApiResponse
    {
        public List<Product> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
	public async Task<IActionResult> DetailProduct(int id)
	{
		try
		{
			string productApiUrl = $"https://localhost:7063/api/ProductApi/{id}";
			var response = await _httpClient.GetAsync(productApiUrl);

			if (!response.IsSuccessStatusCode)
			{
				return View("DetailProduct", new Product
				{
					Productname = "Product not found",
					Isactive = false
				});
			}

			var productJson = await response.Content.ReadAsStringAsync();
			var product = JsonConvert.DeserializeObject<Product>(productJson);
			return View(product);
		}
		catch (Exception ex)
		{
			// Trả về view cùng một kiểu dữ liệu
			return View("DetailProduct", new Product
			{
				Productname = $"Error: {ex.Message}",
				Isactive = false
			});
		}
	}
}
