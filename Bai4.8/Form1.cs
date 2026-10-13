using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai4_8
{
    public partial class Form1 : Form
    {
        private BindingList<Student> studentList = new BindingList<Student>();

        public Form1()
        {
            InitializeComponent();
            dgvStudents.DataSource = studentList;
            dgvStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Phương thức xử lý sự kiện click nút Nạp dữ liệu bất đồng bộ
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            // 1. Quản lý trạng thái giao diện: Khóa nút bấm để tránh kích hoạt trùng lặp
            btnLoad.Enabled = false;
            progressBar1.Value = 0;
            lblStatus.Text = "Đang nạp dữ liệu: 0%";

            // 2. Khai báo đối tượng IProgress để cập nhật tiến trình từ luồng ngầm về UI Thread
            var progress = new Progress<int>(percent =>
            {
                progressBar1.Value = percent;
                lblStatus.Text = $"Đang nạp dữ liệu: {percent}%";
            });

            try
            {
                // 3. Sử dụng Task.Run kết hợp await để giả lập tác vụ nạp dữ liệu lớn trong nền
                List<Student> loadedData = await Task.Run(() => LoadDataFromDatabaseAsync(progress));

                // 4. Đưa dữ liệu sau khi nạp xong lên giao diện thông qua BindingList
                studentList.Clear();
                foreach (var s in loadedData)
                {
                    studentList.Add(s);
                }

                MessageBox.Show("Nạp dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 5. Mở lại nút bấm sau khi tiến trình hoàn tất hoặc gặp lỗi
                btnLoad.Enabled = true;
                lblStatus.Text = "Trạng thái: Hoàn tất";
            }
        }

        // Phương thức giả lập tác vụ nạp dữ liệu lớn chạy ở tiến trình ngầm
        private List<Student> LoadDataFromDatabaseAsync(IProgress<int> progress)
        {
            var list = new List<Student>();
            int totalSteps = 100;

            for (int i = 1; i <= totalSteps; i++)
            {
                // Giả lập độ trễ khi lấy dữ liệu nặng từ CSDL hoặc file
                Task.Delay(25).Wait();

                // Thêm dữ liệu giả lập vào danh sách
                if (i % 10 == 0)
                {
                    list.Add(new Student
                    {
                        StudentId = $"SV0{i}",
                        FullName = $"Sinh Viên Số {i}",
                        Age = 19 + (i % 3),
                        ClassName = "Công Nghệ Thông Tin 1"
                    });
                }

                // Báo cáo phần trăm tiến trình hiện tại
                progress?.Report(i);
            }

            return list;
        }
    }
}