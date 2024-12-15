using System;
using System.Windows;
using System.Windows.Controls;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        public event Action<Option> OptionSelected; // Sự kiện được kích hoạt khi người dùng chọn một tùy chọn trong menu.
                                                    // Tùy chọn có thể là "CONTINUE" hoặc "RESTART".

        public PauseMenu() // Constructor của lớp `PauseMenu`, khởi tạo giao diện người dùng.
        {
            InitializeComponent(); // Tải và khởi tạo các thành phần giao diện được định nghĩa trong XAML.
        }

        private void Continue_Click(object sender, RoutedEventArgs e) // Xử lý sự kiện khi người dùng nhấn nút "CONTINUE".
        {
            OptionSelected?.Invoke(Option.Continue); // Kích hoạt sự kiện `OptionSelected` và gửi giá trị `Option.Continue` để báo rằng người dùng muốn tiếp tục trò chơi.
        }

        private void Restart_Click(object sender, RoutedEventArgs e) // Xử lý sự kiện khi người dùng nhấn nút "RESTART".
        {
            OptionSelected?.Invoke(Option.Restart); // Kích hoạt sự kiện `OptionSelected` và gửi giá trị `Option.Restart` để báo rằng người dùng muốn khởi động lại trò chơi.
        }
    }
}
