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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using Npgsql;

namespace QLBanGiay_Application.View
{
    public partial class frm_Orders : Form
    {
        private readonly UserService _userService;
        private readonly OrderService _orderService;
        private readonly CustomerService _CustomerService;
        private readonly OrderdetailService _orderdetailService;
        private readonly QlShopBanGiayContext _context;
        OrderRepository orderRepository;
        public frm_Orders(UserService userService)
        {
            InitializeComponent();

            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.Load += Frm_Orders_Load;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.txt_Sdt.KeyPress += Txt_Sdt_KeyPress;
            this.dgv_danhsachdh.CellClick += Dgv_danhsachdh_CellClick;
            this.btn_Datlai.Click += Btn_Datlai_Click;
            this.btn_Thoat.Click += Btn_Thoat_Click;
            this.btn_Xuathd.Click += Btn_Xuathd_Click;

            _context = new QlShopBanGiayContext();
            _CustomerService = new CustomerService(new CustomerRepository(_context));
            _orderService = new OrderService(new OrderRepository(_context));
            _orderdetailService = new OrderdetailService(new OrderdetailRepository(_context));
            _userService = userService;
        }

        private void Btn_Xuathd_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main(_userService);
            mainForm.Show();
        }

        private void Btn_Datlai_Click(object? sender, EventArgs e)
        {
            txt_Madm.Text = "";
            txt_Timkiem.Text = "";
            txt_Sdt.Text = "";
            txt_Diachigiao.Text = "";
            LoadPaymentmethod();
            LoadCustomer();
            LoadStatusPaymentmethod();
            LoadStatusOrder();
            date_Dat.Value = DateTime.Now;
            date_Giao.Value = DateTime.Now;
            LoadOrder();
        }

        private void Txt_Sdt_KeyPress(object? sender, KeyPressEventArgs e)
        {
            char inputChar = e.KeyChar;

            if (!char.IsDigit(inputChar) && inputChar != 8)
            {
                e.Handled = true;
            }
            if (txt_Sdt.Text.Length >= 10 && inputChar != 8)
            {
                e.Handled = true;
            }
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim().ToLower();

            var order = _orderService.SearchOrders(keyword);
            if (order != null)
            {
                dgv_danhsachdh.DataSource = order;
            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn.");
            }
        }

        private void Dgv_danhsachdh_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedOrder = (Order)dgv_danhsachdh.Rows[e.RowIndex].DataBoundItem;
                txt_Madm.Text = selectedOrder.Orderid.ToString();
                txt_Sdt.Text = selectedOrder.Phonenumber;
                cbo_Khachhang.SelectedValue = selectedOrder.Customerid;
                txt_Diachigiao.Text = selectedOrder.Deliveryaddress;
                if (selectedOrder.Ordertime.HasValue)
                {
                    date_Dat.Value = selectedOrder.Ordertime.Value.ToDateTime(TimeOnly.MinValue);

                }
                else
                {
                    date_Dat.Value = DateTime.Now;
                }
                if (selectedOrder.Expecteddeliverytime.HasValue)
                {
                    date_Giao.Value = selectedOrder.Expecteddeliverytime.Value.Date;
                }
                else
                {
                    date_Giao.Value = DateTime.Now;
                }
                cbo_Phuongthuctt.SelectedItem = selectedOrder.Paymentmethod;
                cbo_Trangthaidh.SelectedItem = selectedOrder.Orderstatus;
                cbo_Trangthaitt.SelectedItem = selectedOrder.Paymentstatus;
            }
        }

        private void Frm_Orders_Load(object? sender, EventArgs e)
        {
            LoadOrder();
            LoadPaymentmethod();
            LoadCustomer();
            LoadStatusPaymentmethod();
            LoadStatusOrder();
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Madm.Text))
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long orderId = long.Parse(txt_Madm.Text);

            try
            {
                var existingorder = _orderService.GetOrderById(orderId);
                if (existingorder == null)
                {
                    MessageBox.Show("Đơn hàng không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa đơn hàng này?",
                                                     "Xác nhận xóa",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);
                if (confirmResult == DialogResult.No)
                {
                    return;
                }

                var orderDetails = _orderdetailService.GetOrderDetailsByOrderId(orderId);
                if (orderDetails != null && orderDetails.Any())
                {
                    _orderdetailService.DeleteAllInvoiceDetailsByInvoiceId(orderId);
                }

                _context.Orders.Remove(existingorder);
                _context.SaveChanges();

                LoadOrder();

                MessageBox.Show("Xóa đơn hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrder()
        {
            var invoice = _orderService.GetAllOrders();
            if (invoice != null)
            {
                dgv_danhsachdh.DataSource = invoice;
                dgv_danhsachdh.ReadOnly = true;

                dgv_danhsachdh.AutoGenerateColumns = false;
                dgv_danhsachdh.Columns.Clear();

                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Orderid",
                    HeaderText = "Mã ĐH",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Customerid",
                    HeaderText = "Mã KH",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Phonenumber",
                    HeaderText = "Số Điện Thoại",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });


                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Deliveryaddress",
                    HeaderText = "Địa chỉ",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Paymentmethod",
                    HeaderText = "Phương Thức Thanh Toán",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Ordertime",
                    HeaderText = "TG Đặt Hàng",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Expecteddeliverytime",
                    HeaderText = "TGDK Nhận Hàng",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Orderstatus",
                    HeaderText = "Trạng Thái Đơn Hàng",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
                dgv_danhsachdh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Paymentstatus",
                    HeaderText = "Trạng Thái Thanh Toán",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachdh.ColumnHeadersHeight = 50;
                dgv_danhsachdh.RowsDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachdh.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dgv_danhsachdh.DefaultCellStyle.Font = new Font("Arial", 10);

                dgv_danhsachdh.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachdh.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_danhsachdh.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachdh.EnableHeadersVisualStyles = false;
            }
            else
            {
                MessageBox.Show("Không có đơn hàng nào để hiển thị.");
            }
        }
        private void LoadPaymentmethod()
        {
            var paymentmethod = new List<string>
            {
                "Credit Card",
                "Cash",
                "Bank Transfer",
                "PayPal"
            };

            cbo_Phuongthuctt.DataSource = paymentmethod;
            cbo_Phuongthuctt.SelectedIndex = -1;
        }
        private void LoadCustomer()
        {
            var customers = _CustomerService.GetAllCustomers();

            cbo_Khachhang.DataSource = customers;
            cbo_Khachhang.DisplayMember = "Customername";
            cbo_Khachhang.ValueMember = "Customerid";
            cbo_Khachhang.SelectedIndex = -1;
        }
        private void LoadStatusPaymentmethod()
        {
            var statusPaymentmethod = new List<string>
            {
                "Pending",
                "Completed",
                "Failed",
                "Paid",
                "Refunded"
            };

            cbo_Trangthaitt.DataSource = statusPaymentmethod;
            cbo_Trangthaitt.SelectedIndex = -1;
        }

        private void LoadStatusOrder()
        {
            var statusOrder = new List<string>
            {
                "Pending",
                "Processed",
                "Shipped",
                "Delivered",
                "Cancelled"
            };

            cbo_Trangthaidh.DataSource = statusOrder;
            cbo_Trangthaidh.SelectedIndex = -1;
        }

        private void btn_Themhd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_Sdt.Text) ||
                    string.IsNullOrWhiteSpace(txt_Diachigiao.Text) ||
                    cbo_Khachhang.SelectedIndex == -1 ||
                    cbo_Phuongthuctt.SelectedIndex == -1 ||
                    cbo_Trangthaidh.SelectedIndex == -1 ||
                    cbo_Trangthaitt.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var customerId = (long)cbo_Khachhang.SelectedValue;
                var customerName = cbo_Khachhang.Text;
                var phoneNumber = txt_Sdt.Text;
                var deliveryAddress = txt_Diachigiao.Text;
                var paymentMethod = cbo_Phuongthuctt.SelectedItem?.ToString() ?? "Cash";
                var orderStatus = cbo_Trangthaidh.SelectedItem?.ToString() ?? "Pending";
                var paymentStatus = cbo_Trangthaitt.SelectedItem?.ToString() ?? "Not Paid";
                var orderTime = DateTime.SpecifyKind(date_Dat.Value, DateTimeKind.Utc);
                var expectedDeliveryTime = DateTime.SpecifyKind(date_Giao.Value, DateTimeKind.Utc);

                var newOrder = new Order
                {
                    Customerid = customerId,
                    Customername = customerName,
                    Phonenumber = phoneNumber,
                    Deliveryaddress = deliveryAddress,
                    Paymentmethod = paymentMethod,
                    Ordertime = DateOnly.FromDateTime(orderTime),
                    Expecteddeliverytime = expectedDeliveryTime,
                    Orderstatus = orderStatus,
                    Paymentstatus = paymentStatus,
                    Iscart = false
                };


                if (!_context.Customers.Any(c => c.Customerid == customerId))
                {
                    MessageBox.Show("Khách hàng không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _orderService.AddOrder(newOrder);
                LoadOrder();
                MessageBox.Show("Đơn hàng đã được thêm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Có lỗi xảy ra: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nChi tiết: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_Capnhathd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_Madm.Text) ||
                    string.IsNullOrWhiteSpace(txt_Sdt.Text) ||
                    string.IsNullOrWhiteSpace(txt_Diachigiao.Text) ||
                    cbo_Khachhang.SelectedIndex == -1 ||
                    cbo_Phuongthuctt.SelectedIndex == -1 ||
                    cbo_Trangthaidh.SelectedIndex == -1 ||
                    cbo_Trangthaitt.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                var orderId = int.Parse(txt_Madm.Text);
                var customerId = Convert.ToInt64(cbo_Khachhang.SelectedValue);
                var customerName = cbo_Khachhang.Text;
                var phoneNumber = txt_Sdt.Text;
                var deliveryAddress = txt_Diachigiao.Text;
                var paymentMethod = cbo_Phuongthuctt.SelectedItem.ToString();
                var orderStatus = cbo_Trangthaidh.SelectedItem.ToString();
                var paymentStatus = cbo_Trangthaitt.SelectedItem.ToString();
                var orderTime = DateTime.SpecifyKind(date_Dat.Value, DateTimeKind.Utc);
                var expectedDeliveryTime = DateTime.SpecifyKind(date_Giao.Value, DateTimeKind.Utc);

                var existingOrder = _orderService.GetOrderById(orderId);

                if (existingOrder == null)
                {
                    MessageBox.Show("Đơn hàng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                existingOrder.Customerid = customerId;
                existingOrder.Customername = customerName;
                existingOrder.Phonenumber = phoneNumber;
                existingOrder.Deliveryaddress = deliveryAddress;
                existingOrder.Paymentmethod = paymentMethod;
                existingOrder.Ordertime = orderTime != null ? DateOnly.FromDateTime(orderTime) : null;
                existingOrder.Expecteddeliverytime = expectedDeliveryTime;
                existingOrder.Orderstatus = orderStatus;
                existingOrder.Paymentstatus = paymentStatus;

                _orderService.UpdateOrder(existingOrder);
                LoadOrder();
                MessageBox.Show("Đơn hàng đã được cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrder();
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng cho tất cả các trường!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Có lỗi xảy ra: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nChi tiết: {ex.InnerException.Message}";
                }
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Xuathd_Click(object sender, EventArgs e)
        {

        }
    }
}
