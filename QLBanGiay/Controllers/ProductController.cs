
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBanGiay.Models.Models;


public class ProductController : Controller
{
    private readonly HttpClient _httpClient;

    public ProductController()
    {
        _httpClient = new HttpClient();
    }

    public async Task<IActionResult> AllProduct(int page = 1, int pageSize = 12)
    {
        string apiUrl = $"https://localhost:7063/api/ProductApi?page={page}&pageSize={pageSize}";
        var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonData);

            ViewBag.CurrentPage = apiResponse.CurrentPage;
            ViewBag.TotalPages = apiResponse.TotalPages;

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
