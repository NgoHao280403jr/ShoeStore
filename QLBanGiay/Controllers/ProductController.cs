using System.Net.Http;
using System.Threading.Tasks;
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

    public async Task<IActionResult> AllProduct(int page = 1, int pageSize = 10)
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
        string apiUrl = $"https://localhost:7063/api/ProductApi/{id}";
        var response = await _httpClient.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(jsonData);
            return View(product);
        }

        return View("Error"); // Trả về trang lỗi nếu API không thành công
    }
}
