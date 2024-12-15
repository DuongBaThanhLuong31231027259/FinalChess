using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for NameEntryWindow.xaml
    /// </summary>
    public partial class NameEntryWindow : Window
    {
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public NameEntryWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy tên người chơi từ TextBox
                Player1Name = Player1Input.Text;
                Player2Name = Player2Input.Text;

                // Kiểm tra tên người chơi có hợp lệ không
                if (string.IsNullOrWhiteSpace(Player1Name) || string.IsNullOrWhiteSpace(Player2Name))
                {
                    throw new ArgumentException("Tên người chơi không thể trống.");
                }

                // Đóng cửa sổ nhập tên và quay lại cửa sổ chính
                this.DialogResult = true;
                this.Close();
            }
            catch (ArgumentException ex)
            {
                // Hiển thị thông báo lỗi cho người dùng
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi khác
                MessageBox.Show("Đã xảy ra lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
