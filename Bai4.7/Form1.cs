using System;
using System.Windows.Forms;

namespace Bai4_7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenChildForm(Type formType)
        {
            Form existingForm = null;
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == formType)
                {
                    existingForm = f;
                    break;
                }
            }

            if (existingForm != null)
            {
                existingForm.Activate(); // Kích hoạt form nếu đã tồn tại
            }
            else
            {
                Form newForm = (Form)Activator.CreateInstance(formType);
                newForm.MdiParent = this;
                newForm.Show();
            }
        }

        public void UpdateMessage(string message)
        {
            lblStatus.Text = $"Trạng thái: {message} ({DateTime.Now:HH:mm:ss})";
        }

        private void mnuQLSinhVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(typeof(FormStudent));
        }

        private void mnuCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void mnuTileHorizontal_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mnuTileVertical_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đã đăng xuất hệ thống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}