using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Bai4_6
{
    public partial class Form1 : Form
    {
        private BindingList<Student> _studentList = new BindingList<Student>();
        private BindingSource _bindingSource = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            SetupDataBinding();
        }

        private void SetupDataBinding()
        {
            // 1. Nạp danh mục Giới tính vào ComboBox
            cboGender.Items.Add("Nam");
            cboGender.Items.Add("Nữ");

            // 2. Thêm dữ liệu mẫu
            _studentList.Add(new Student
            {
                StudentId = "SV001",
                FullName = "Nguyen Van A",
                Age = 20,
                Gender = "Nam",
                AvatarPath = ""
            });

            // 3. Cấu hình BindingSource và DataBinding 2 chiều[cite: 12]
            _bindingSource.DataSource = _studentList;
            dgvStudents.DataSource = _bindingSource;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Liên kết dữ liệu giữa các ô nhập liệu với BindingSource[cite: 12]
            txtStudentId.DataBindings.Add("Text", _bindingSource, "StudentId", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFullName.DataBindings.Add("Text", _bindingSource, "FullName", true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", _bindingSource, "Age", true, DataSourceUpdateMode.OnPropertyChanged);
            cboGender.DataBindings.Add("SelectedItem", _bindingSource, "Gender", true, DataSourceUpdateMode.OnPropertyChanged);

            // Sự kiện khi thay đổi dòng chọn trên DataGridView/BindingSource để load lại Avatar[cite: 12]
            _bindingSource.CurrentChanged += _bindingSource_CurrentChanged;
            LoadCurrentAvatar();
        }

        private void _bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            LoadCurrentAvatar();
        }

        private void LoadCurrentAvatar()
        {
            if (_bindingSource.Current is Student currentStudent)
            {
                if (!string.IsNullOrEmpty(currentStudent.AvatarPath) && File.Exists(currentStudent.AvatarPath))
                {
                    picAvatar.ImageLocation = currentStudent.AvatarPath;
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }

        // Chọn ảnh Avatar dùng OpenFileDialog[cite: 13]
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (_bindingSource.Current is Student currentStudent)
                    {
                        currentStudent.AvatarPath = ofd.FileName;
                        picAvatar.ImageLocation = ofd.FileName;
                    }
                }
            }
        }

        // Thêm mới sinh viên
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Student newStudent = new Student
            {
                StudentId = "SV" + (_studentList.Count + 101),
                FullName = "Sinh vien moi",
                Age = 19,
                Gender = "Nam",
                AvatarPath = ""
            };

            _studentList.Add(newStudent);
            _bindingSource.Position = _studentList.Count - 1;
            MessageBox.Show("Them sinh vien thanh công!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Cập nhật thông tin sinh viên
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _bindingSource.ResetCurrentItem();
            MessageBox.Show("Cap nhat thong tin thanh cong!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Xóa sinh viên đang chọn
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current is Student currentStudent)
            {
                DialogResult dr = MessageBox.Show("Ban co chac chan muon xoa sinh vien nay?", "Xac nhan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    _studentList.Remove(currentStudent);
                    MessageBox.Show("Xoa thanh cong!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Xuất file CSV dùng SaveFileDialog[cite: 13]
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "Students_Export.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("StudentId,FullName,Age,Gender,AvatarPath");

                    foreach (var s in _studentList)
                    {
                        sb.AppendLine($"{s.StudentId},{s.FullName},{s.Age},{s.Gender},{s.AvatarPath}");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuat file CSV thanh cong!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}