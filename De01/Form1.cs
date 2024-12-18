using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace De01
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối SQL Server
        string connectionString = "";
        SqlConnection conn;
        SqlDataAdapter adapter;
        DataTable dtSinhVien;
        public Form1()
        {
            InitializeComponent();
        }
        private void frmSinhvien_Load(object sender, EventArgs e)
        {
            LoadSinhVien(); // Load dữ liệu vào ListView
            LoadLop();      // Load lớp vào ComboBox
            SetControlState(false); // Tắt các nút điều khiển ban đầu
        }

        // Load dữ liệu sinh viên vào ListView
        private void LoadSinhVien()
        {
            lvSinhvien.Rows.Clear();
            string query = "SELECT MaSV, HotenSV, NgaySinh, MaLop FROM Sinhvien";
            conn = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(query, conn);
            dtSinhVien = new DataTable();
            adapter.Fill(dtSinhVien);

            foreach (DataRow row in dtSinhVien.Rows)
            {
                ListViewItem item = new ListViewItem(row["MaSV"].ToString());
                item.SubItems.Add(row["HotenSV"].ToString());
                item.SubItems.Add(Convert.ToDateTime(row["NgaySinh"]).ToString("dd/MM/yyyy"));
                item.SubItems.Add(row["MaLop"].ToString());
                lvSinhvien.Rows.Add(item);
            }
        }

        // Load danh sách lớp vào ComboBox
        private void LoadLop()
        {
            string query = "SELECT MaLop, TenLop FROM Lop";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataTable dtLop = new DataTable();
            adapter.Fill(dtLop);
            cboLop.DataSource = dtLop;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }

        // Xử lý chọn một dòng trong ListView
        private void dgvSinhvien_SelectionChanged(object sender, EventArgs e)
        {
            if (lvSinhvien.SelectedRows.Count > 0)
            {
                DataGridViewRow row = lvSinhvien.SelectedRows[0];

                txtMaSV.Text = row.Cells[0].Value.ToString();
                txtHotenSV.Text = row.Cells[1].Value.ToString();
                dtNgaysinh.Value = Convert.ToDateTime(row.Cells[2].Value);
                cboLop.SelectedValue = row.Cells[3].Value.ToString();

                SetControlState(true);
            }
        }

        // Điều khiển trạng thái các nút trên form
        private void SetControlState(bool state)
        {
            btSua.Enabled = state;
            btXoa.Enabled = state;
            btLuu.Enabled = !state;
            btKhong.Enabled = !state;
        }

        // Nút Thêm mới
        private void btThem_Click(object sender, EventArgs e)
        {
            ClearControls();
            SetControlState(false);
        }

        private void ClearControls()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtNgaysinh.Value = DateTime.Now;
            cboLop.SelectedIndex = 0;
        }

        // Nút Lưu
        private void btLuu_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Sinhvien (MaSV, HotenSV, NgaySinh, MaLop) " +
                           "VALUES (@MaSV, @HotenSV, @NgaySinh, @MaLop)";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                cmd.Parameters.AddWithValue("@HotenSV", txtHotenSV.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dtNgaysinh.Value);
                cmd.Parameters.AddWithValue("@MaLop", cboLop.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadSinhVien();
            MessageBox.Show("Thêm mới thành công!");
        }

        // Nút Xóa
        private void btXoa_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Sinhvien WHERE MaSV = @MaSV";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadSinhVien();
            MessageBox.Show("Xóa thành công!");
        }

        // Nút Sửa
        private void btSua_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Sinhvien SET HotenSV = @HotenSV, NgaySinh = @NgaySinh, MaLop = @MaLop " +
                           "WHERE MaSV = @MaSV";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                cmd.Parameters.AddWithValue("@HotenSV", txtHotenSV.Text);
                cmd.Parameters.AddWithValue("@NgaySinh", dtNgaysinh.Value);
                cmd.Parameters.AddWithValue("@MaLop", cboLop.SelectedValue);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            LoadSinhVien();
            MessageBox.Show("Cập nhật thành công!");
        }

        // Nút Thoát
        private void btThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void lvSinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
