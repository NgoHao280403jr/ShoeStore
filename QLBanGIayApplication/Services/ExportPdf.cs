using iTextSharp.text.pdf;
using iTextSharp.text;
using QLBanGiay.Models.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using QLBanGiay_Application.Repository;

namespace QLBanGiay_Application.Services
{
    public class ExportPdf
    {
        private readonly OrderService _orderService;
        private readonly OrderdetailService _orderDetailService;
        public ExportPdf(OrderService orderService, OrderdetailService orderDetailService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }
        public void ExportInvoiceToPdf(long orderId)
        {
            // Lấy thông tin hóa đơn và chi tiết hóa đơn
            Order order = _orderService.GetOrderById(orderId);
            IEnumerable<Orderdetail> orderDetails = _orderDetailService.GetOrderDetailsByOrderId(orderId);

            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            // Hộp thoại lưu file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.FileName = $"Invoice_{order.Orderid}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = saveFileDialog.FileName;

                    // Tạo tài liệu PDF
                    Document document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));

                    document.Open();

                    // Tiêu đề hóa đơn
                    var titleFont = FontFactory.GetFont("Times New Roman", 18, iTextSharp.text.Font.BOLD);
                    var normalFont = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.NORMAL);

                    document.Add(new Paragraph("HOA DON BAN HANG", titleFont));
                    document.Add(new Paragraph($"Ma Hoa Don: {order.Orderid}", normalFont));
                    document.Add(new Paragraph($"Ten Khach Hang: {order.Customername}", normalFont));
                    document.Add(new Paragraph($"SDT: {order.Phonenumber}", normalFont));
                    document.Add(new Paragraph($"Dia Chi Giao Hang: {order.Deliveryaddress}", normalFont));
                    document.Add(new Paragraph($"Phuong Thuc Thanh Toan: {order.Paymentmethod}", normalFont));
                    document.Add(new Paragraph($"Ngay Dat Hang: {order.Ordertime?.ToString("dd/MM/yyyy")}", normalFont));
                    document.Add(new Paragraph($"Ngay Giao Du Kien: {order.Expecteddeliverytime?.ToString("dd/MM/yyyy")}", normalFont));
                    document.Add(new Paragraph($"Trang Thai Don Hang: {order.Orderstatus}", normalFont));
                    document.Add(new Paragraph(" "));

                    // Bảng chi tiết hóa đơn
                    PdfPTable table = new PdfPTable(5) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 1, 3, 1, 2, 2 });

                    // Header của bảng
                    table.AddCell("STT");
                    table.AddCell("Ten San Pham");
                    table.AddCell("Size");
                    table.AddCell("So Luong");
                    table.AddCell("Thanh Tien");

                    int index = 1;
                    double totalAmount = 0;

                    foreach (var detail in orderDetails)
                    {
                        table.AddCell(index.ToString());
                        table.AddCell(detail.Product?.Productname ?? "N/A");
                        table.AddCell(detail.Size);
                        table.AddCell(detail.Quantity?.ToString() ?? "0");
                        table.AddCell($"{detail.Subtotal?.ToString("N0")} VND");

                        totalAmount += detail.Subtotal ?? 0;
                        index++;
                    }

                    // Tổng tiền
                    PdfPCell totalCell = new PdfPCell(new Phrase($"Tong Tien: {totalAmount.ToString("N0")} VND", FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD, BaseColor.RED)))
                    {
                        Colspan = 5,
                        HorizontalAlignment = Element.ALIGN_RIGHT
                    };
                    table.AddCell(totalCell);

                    document.Add(table);

                    // Kết thúc hóa đơn
                    document.Add(new Paragraph("Cam on quy khach da mua hang!", normalFont));
                    document.Close();

                    MessageBox.Show("Xuất hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
