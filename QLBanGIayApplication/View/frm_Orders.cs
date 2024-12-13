using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository;
using QLBanGiay_Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanGiay_Application.View
{
    public partial class frm_Orders : Form
    {
        private readonly OrderService _orderService;
        private readonly CustomerService _CustomerService;
        private readonly OrderdetailService _orderdetailService;
        private readonly QlShopBanGiayContext _context;
        public frm_Orders()
        {
            InitializeComponent();


            _context = new QlShopBanGiayContext();
            _CustomerService = new CustomerService(new CustomerRepository(_context));
            _orderService = new OrderService(new OrderRepository(_context));
            _orderdetailService = new OrderdetailService(new OrderdetailRepository(_context));
        }

        //private void Btn_TK_Click(object? sender, EventArgs e)
        //{
        //    var keyword = txt_Timkiem.Text.Trim().ToLower();

        //    var invoice = _orderService.SearchOrders(keyword);
        //    if (invoice != null)
        //    {
        //        dgv_danhsachkh.DataSource = invoice;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Không tìm thấy hóa đơn.");
        //    }
        //}

        //private void Frm_Orders_Load(object? sender, EventArgs e)
        //{
        //    LoadOrder();
        //    LoadPaymentmethod();
        //    LoadCustomer();
        //    LoadStatusPaymentmethod();
        //    LoadStatusOrder();
        //}

        //private void Dgv_danhsachkh_CellClick(object? sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex >= 0)
        //    {
        //        var selectedOrder = (Order)dgv_danhsachkh.Rows[e.RowIndex].DataBoundItem;
        //        var customer = _context.Orders.Include(c => c.Customer)
        //                            .FirstOrDefault(c => c.Customerid == selectedOrder.Customerid);

        //        txt_Madm.Text = selectedOrder.Orderid.ToString();
        //        txt_SDT.Text = selectedOrder.Phonenumber;
        //        cbb_KH.SelectedItem = customer.Customer.Customername;
        //        txt_DC.Text = selectedOrder.Deliveryaddress;
        //        if (selectedOrder.Ordertime.HasValue)
        //        {
        //            dtp_TGDH.Value = selectedOrder.Ordertime.Value.ToDateTime(TimeOnly.MinValue);
        //        }
        //        else
        //        {
        //            dtp_TGDH.Value = DateTime.Now;
        //        }
        //        if (selectedOrder.Expecteddeliverytime.HasValue)
        //        {
        //            dtp_TGDK.Value = selectedOrder.Expecteddeliverytime.Value.ToDateTime(TimeOnly.MinValue);
        //        }
        //        else
        //        {
        //            dtp_TGDK.Value = DateTime.Now;
        //        }
        //        cbb_PTTT.SelectedItem = selectedOrder.Paymentmethod;
        //        cbb_StatusDH.SelectedItem = selectedOrder.Orderstatus;
        //        cbb_StatusPayment.SelectedItem = selectedOrder.Paymentstatus;
        //    }
        //}

        //private void Btn_Xoa_Click(object? sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txt_Madm.Text))
        //    {
        //        MessageBox.Show("Vui lòng chọn đơn hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    long orderId = long.Parse(txt_Madm.Text);

        //    try
        //    {

        //        var existingorder = _orderService.GetOrderById(orderId);
        //        if (existingorder == null)
        //        {
        //            MessageBox.Show("Đơn hàng không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        var orderDetails = _orderdetailService.GetOrderDetailsByOrderId(orderId);
        //        if (orderDetails != null && orderDetails.Any())
        //        {
        //            _orderdetailService.DeleteAllInvoiceDetailsByInvoiceId(orderId);
        //        }
        //        _orderService.DeleteOrder(orderId);
        //        LoadOrder();
        //        MessageBox.Show("Xóa đơn hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //private void LoadOrder()
        //{
        //    var invoice = _orderService.GetAllOrders();
        //    if (invoice != null)
        //    {
        //        dgv_danhsachkh.DataSource = invoice;
        //        dgv_danhsachkh.ReadOnly = true;

        //        dgv_danhsachkh.AutoGenerateColumns = false;
        //        dgv_danhsachkh.Columns.Clear();

        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Orderid",
        //            HeaderText = "Mã ĐH",
        //            Width = 50,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });

        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Customername",
        //            HeaderText = "Tên KH",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });

        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Phonenumber",
        //            HeaderText = "Số Điện Thoại",
        //            Width = 50,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });


        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Deliveryaddress",
        //            HeaderText = "Địa chỉ",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });
        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Paymentmethod",
        //            HeaderText = "Phương Thức Thanh Toán",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });
        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Ordertime",
        //            HeaderText = "TG Đặt Hàng",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });
        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Expecteddeliverytime",
        //            HeaderText = "TGDK Nhận Hàng",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });
        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Orderstatus",
        //            HeaderText = "Trạng Thái Đơn Hàng",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });
        //        dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            DataPropertyName = "Paymentstatus",
        //            HeaderText = "Trạng Thái Thanh Toán",
        //            Width = 200,
        //            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
        //        });

        //        dgv_danhsachkh.ColumnHeadersHeight = 50;
        //        dgv_danhsachkh.RowsDefaultCellStyle.BackColor = Color.LightGreen;
        //        dgv_danhsachkh.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
        //        dgv_danhsachkh.DefaultCellStyle.Font = new Font("Arial", 10);

        //        dgv_danhsachkh.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
        //        dgv_danhsachkh.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        //        dgv_danhsachkh.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
        //        dgv_danhsachkh.EnableHeadersVisualStyles = false;
        //    }
        //    else
        //    {
        //        MessageBox.Show("Không có đơn hàng nào để hiển thị.");
        //    }
        //}
        //private void LoadPaymentmethod()
        //{
        //    var paymentmethod = _context.Orders.Select(c => c.Paymentmethod)
        //                                         .Distinct()
        //                                         .ToList();

        //    cbb_StatusPayment.DataSource = paymentmethod;
        //    cbb_StatusPayment.SelectedIndex = -1;
        //}
        //private void LoadCustomer()
        //{
        //    var customers = _context.Orders
        //                            .Select(c => c.Customername)
        //                            .Distinct()
        //                            .ToList();

        //    cbb_KH.DataSource = customers;
        //    cbb_KH.SelectedIndex = -1;
        //}
        //private void LoadStatusPaymentmethod()
        //{
        //    var statusPaymentmethod = _context.Orders.Select(c => c.Paymentstatus)
        //                             .Distinct()
        //                             .ToList();

        //    cbb_StatusPayment.DataSource = statusPaymentmethod;
        //    cbb_StatusPayment.SelectedIndex = -1;
        //}
        //private void LoadStatusOrder()
        //{
        //    var statusOrder = _context.Orders.Select(c => c.Orderstatus)
        //                 .Distinct()
        //                 .ToList();

        //    cbb_StatusPayment.DataSource = statusOrder;
        //    cbb_StatusPayment.SelectedIndex = -1;
        //}
    }
}
