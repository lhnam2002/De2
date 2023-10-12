using BUS;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GUI
{
   
    public partial class frmQuanLySanPham : Form
    {
        private readonly SPService sPService = new SPService();
        private readonly LoaiService loaiService = new LoaiService();
        private Model1 context = new Model1();
        public frmQuanLySanPham()
        {
            InitializeComponent();
        }

        private void frmQuanLySanPham_Load(object sender, EventArgs e)
        {
            List<SanPham1> sanphams = sPService.GetAll();
            List<LoaiSP> loaids= loaiService.GetAll();
            BindGrid(sanphams);
            Fillcmb(loaids);
        }

        private void BindGrid(List<SanPham1> ds)
        {
            lvSanPham.Items.Clear();
            foreach (var sanpham in ds)
            {
                ListViewItem item = new ListViewItem(sanpham.MaSP);
                item.SubItems.Add(sanpham.TenSP);
                item.SubItems.Add(sanpham.NgayNhap.ToString());
                item.SubItems.Add(sanpham.LoaiSP.TenLoai);
                lvSanPham.Items.Add(item);
            }
        }
        private void Fillcmb(List<LoaiSP> ds)
        {
            cmbLoaiSP.DataSource = ds;
            cmbLoaiSP.DisplayMember = "TenLoai";
            cmbLoaiSP.ValueMember = "MaLoai";
        }
       

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    // Tạo một đối tượng sản phẩm mới
                    SanPham1 sanphamMoi = new SanPham1();
                    sanphamMoi.MaSP = txtMaSP.Text;
                    sanphamMoi.TenSP = txtTenSP.Text;
                    sanphamMoi.NgayNhap = dtNgayNhap.Value;
                    sanphamMoi.MaLoai = (string)cmbLoaiSP.SelectedValue;

                    // Cập nhật giao diện hiển thị danh sách sản phẩm
                    Model1 context = new Model1();
                    context.SanPham1.Add(sanphamMoi);
                    context.SaveChanges();

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindGrid(sPService.GetAll());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                SanPham1 sp = context.SanPham1.FirstOrDefault(p => p.MaSP == txtMaSP.Text);
                    if (sp != null)
                    {
                         context.SanPham1.Remove(sp);
                         context.SaveChanges();

                         // Hiển thị thông báo thành công
                         MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                         BindGrid(sPService.GetAll());
                    }
           
            }
            catch (Exception ex)
            {

            }
           
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();

                if (string.IsNullOrEmpty(txtMaSP.Text) || string.IsNullOrEmpty(txtTenSP.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    
                    SanPham1 sanpham = sPService.FindByID(txtMaSP.Text);
                    sanpham.MaSP = txtMaSP.Text;
                    sanpham.TenSP = txtTenSP.Text;
                    sanpham.NgayNhap = dtNgayNhap.Value;
                    sanpham.MaLoai = (string)cmbLoaiSP.SelectedValue;
                    
                    context.SanPham1.AddOrUpdate(sanpham);
                    context.SaveChanges();

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindGrid(sPService.GetAll());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvSanPham_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if(lvSanPham.SelectedItems.Count > 0)
                {
                    txtMaSP.Text = lvSanPham.SelectedItems[0].SubItems[0].Text;
                    txtTenSP.Text = lvSanPham.SelectedItems[0].SubItems[1].Text;
                    dtNgayNhap.Value = DateTime.Parse(lvSanPham.SelectedItems[0].SubItems[2].Text);
                    
                    cmbLoaiSP.Text = lvSanPham.SelectedItems[0].SubItems[3].Text;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn thoát????", "Thông báo",
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                   List<SanPham1> sp = context.SanPham1.Where(p => p.TenSP.Contains(txtTimKiem.Text)).ToList();
                BindGrid(sp);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
