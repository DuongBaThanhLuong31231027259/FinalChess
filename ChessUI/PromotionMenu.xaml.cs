using ChessLogic;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessUI
{
    /// <summary>
    /// Interaction logic for PromotionMenu.xaml
    /// </summary>
    public partial class PromotionMenu : UserControl
    {
        // Sự kiện được kích hoạt khi người chơi chọn một loại quân cờ để phong cấp.
        // Giá trị truyền đi là loại quân cờ được chọn (PieceType).
        public event Action<PieceType> PieceSelected;

        // Constructor của lớp `PromotionMenu`, nhận vào thông tin người chơi (Player).
        public PromotionMenu(Player player)
        {
            InitializeComponent();

            // Thiết lập hình ảnh hiển thị cho từng loại quân cờ dựa trên màu sắc của người chơi.
            QueenImg.Source = Images.GetImage(player, PieceType.Queen); // Hình ảnh quân Hậu.
            BishopImg.Source = Images.GetImage(player, PieceType.Bishop); // Hình ảnh quân Tượng.
            RookImg.Source = Images.GetImage(player, PieceType.Rook); // Hình ảnh quân Xe.
            KnightImg.Source = Images.GetImage(player, PieceType.Knight); // Hình ảnh quân Mã.
        }

        private void QueenImg_MouseDown(object sender, MouseButtonEventArgs e) // // Xử lý sự kiện khi người chơi nhấp chuột vào hình ảnh quân Hậu.
        {
            PieceSelected?.Invoke(PieceType.Queen);
        }

        private void BishopImg_MouseDown(object sender, MouseButtonEventArgs e) // // Xử lý sự kiện khi người chơi nhấp chuột vào hình ảnh quân Tượng.
        {
            PieceSelected?.Invoke(PieceType.Bishop);
        }

        private void RookImg_MouseDown(object sender, MouseButtonEventArgs e) // Xử lý sự kiện khi người chơi nhấp chuột vào hình ảnh quân Xe.
        {
            PieceSelected?.Invoke(PieceType.Rook);
        }

        private void KnightImg_MouseDown(object sender, MouseButtonEventArgs e) // Xử lý sự kiện khi người chơi nhấp chuột vào hình ảnh quân Mã.
        {
            PieceSelected?.Invoke(PieceType.Knight);
        }
    }
}
