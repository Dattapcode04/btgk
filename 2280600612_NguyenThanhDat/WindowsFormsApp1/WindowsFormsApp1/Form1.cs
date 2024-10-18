using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class frmSanPham : Form
    {
        public frmSanPham()
        {
            InitializeComponent();
        }
        private void LoadSanPham()
        {
            try
            {

                using (Model1 context = new Model1())
                {
                    List<Sanpham> listSanPham = context.Sanphams.ToList();

                    dgvSanPham.Rows.Clear();

                    foreach (var sp in listSanPham)
                    {
                        int index = dgvSanPham.Rows.Add();

                        dgvSanPham.Rows[index].Cells[0].Value = sp.MaSP;
                        dgvSanPham.Rows[index].Cells[1].Value = sp.TenSP;
                        dgvSanPham.Rows[index].Cells[2].Value = sp.Ngaynhap.ToString();
                        dgvSanPham.Rows[index].Cells[3].Value = sp.LoaiSP.TenLoai;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm load danh sách loại sản phẩm vào combobox
        private void LoadLoaiSanPham()
        {
            using (var db = new Model1())
            {
                var loaisps = db.LoaiSPs.ToList();
                cboLoaiSP.DataSource = loaisps;
                cboLoaiSP.DisplayMember = "TenLoai";
                cboLoaiSP.ValueMember = "MaLoai";
            }
        }
        private void BindGrid(List<Sanpham> listSanPham)
        {
            dgvSanPham.Rows.Clear();

            foreach (var item in listSanPham)
            {
                int index = dgvSanPham.Rows.Add();
                dgvSanPham.Rows[index].Cells[0].Value = item.MaSP;
                dgvSanPham.Rows[index].Cells[1].Value = item.TenSP;
                dgvSanPham.Rows[index].Cells[2].Value = item.Ngaynhap.ToString();
                dgvSanPham.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai;
            }
        }
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadSanPham();
            LoadLoaiSanPham();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy các thay đổi?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Reset các control về trạng thái ban đầu nếu người dùng chọn "Yes"
                btThem.Enabled = true;
                btSua.Enabled = true;
                btLuu.Enabled = false;
                MessageBox.Show("Các thay đổi đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thoát", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        private void ResetFormControls()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgaynhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = -1; // Hoặc thiết lập lại giá trị đầu tiên của ComboBox nếu cần
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string tenSP = txtTenSP.Text.Trim();

                if (string.IsNullOrWhiteSpace(tenSP))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (Model1 context = new Model1())
                {
                    var listSanPham = context.Sanphams
                                            .Where(sp => sp.TenSP.Contains(tenSP))
                                            .ToList();
                    if (listSanPham.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm nào với tên: " + tenSP, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    BindGrid(listSanPham);

                    // Hiển thị tên sản phẩm tìm thấy đầu tiên vào TextBox
                    txtTenSPFound.Text = listSanPham[0].TenSP;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xóa", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var db = new Model1())
                {
                    var masp = txtMaSP.Text;
                    var sanpham = db.Sanphams.SingleOrDefault(sp => sp.MaSP == masp);
                    if (sanpham != null)
                    {
                        db.Sanphams.Remove(sanpham);
                        db.SaveChanges();
                        LoadSanPham();
                    }
                }

                // Kích hoạt lại các nút cần thiết
                btThem.Enabled = true;
                btSua.Enabled = true;
                btLuu.Enabled = false;
            }
        }

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
                dtNgaynhap.Value = Convert.ToDateTime(row.Cells["Ngaynhap"].Value);
                cboLoaiSP.Text = row.Cells["LoaiSP"].Value.ToString();
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            // Bỏ trống các ô nhập liệu để nhập mới sản phẩm
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgaynhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = 0;

            // Kích hoạt nút "Lưu" và vô hiệu hóa nút "Thêm"
            btLuu.Enabled = true;
            btThem.Enabled = false;
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            btLuu.Enabled = true;
            btSua.Enabled = false;
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new Model1())
                {
                    var masp = txtMaSP.Text;
                    var sanpham = db.Sanphams.SingleOrDefault(sp => sp.MaSP == masp);

                    if (sanpham == null)
                    {
                        // Thêm mới
                        sanpham = new Sanpham
                        {
                            MaSP = txtMaSP.Text,
                            TenSP = txtTenSP.Text,
                            Ngaynhap = dtNgaynhap.Value,
                            MaLoai = cboLoaiSP.SelectedValue.ToString()
                        };
                        db.Sanphams.Add(sanpham);

                        // Thông báo thêm mới thành công
                        MessageBox.Show("Sản phẩm mới đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Sửa
                        sanpham.TenSP = txtTenSP.Text;
                        sanpham.Ngaynhap = dtNgaynhap.Value;
                        sanpham.MaLoai = cboLoaiSP.SelectedValue.ToString();

                        // Thông báo sửa thành công
                        MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    // Tải lại danh sách sản phẩm sau khi lưu
                    LoadSanPham();
                }

                // Kích hoạt lại các nút sau khi lưu
                btThem.Enabled = true;
                btSua.Enabled = true;
                btLuu.Enabled = false;
            }
            catch (Exception ex)
            {
                // Thông báo lỗi nếu xảy ra
                MessageBox.Show("Đã xảy ra lỗi khi lưu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}
