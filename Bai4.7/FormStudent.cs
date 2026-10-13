using System;
using System.Windows.Forms;

namespace Bai4_7
{
    public partial class FormStudent : Form
    {
        public FormStudent()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            dgvStudents.Rows.Add("SV001", "Nguyễn Văn An", "CNTT 1");
            dgvStudents.Rows.Add("SV002", "Trần Thị Bình", "CNTT 2");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgvStudents.Rows.Add("SV" + (dgvStudents.Rows.Count + 1), "Sinh Viên Mới", "CNTT 1");

            // Truyền thông điệp cập nhật lên StatusStrip của Form1 (MDI Parent)[cite: 4]
            if (this.MdiParent is Form1 mainForm)
            {
                mainForm.UpdateMessage("Thêm sinh viên mới thành công");
            }
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow != null && !dgvStudents.CurrentRow.IsNewRow)
            {
                dgvStudents.Rows.Remove(dgvStudents.CurrentRow);

                // Giao tiếp truyền thông điệp xóa lên Form1[cite: 4]
                if (this.MdiParent is Form1 mainForm)
                {
                    mainForm.UpdateMessage("Đã xóa sinh viên thành công");
                }
            }
        }

        private void mnuRefresh_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Làm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (this.MdiParent is Form1 mainForm)
            {
                mainForm.UpdateMessage("Làm mới danh sách sinh viên");
            }
        }
    }
}