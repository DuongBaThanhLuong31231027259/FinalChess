using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;

namespace ChessUI
{
    public static class Images
    {
        // Lớp tĩnh lưu trữ và quản lý hình ảnh các quân cờ.

        private static readonly Dictionary<PieceType, ImageSource> whiteSources = new()
    {
        // Từ điển ánh xạ loại quân cờ (PieceType) với hình ảnh tương ứng của quân cờ trắng.
        { PieceType.Pawn, LoadImage("Assets/PawnW.png") },   // Quân Tốt trắng.
        { PieceType.Bishop, LoadImage("Assets/BishopW.png") }, // Quân Tượng trắng.
        { PieceType.Knight, LoadImage("Assets/KnightW.png") }, // Quân Mã trắng.
        { PieceType.Rook, LoadImage("Assets/RookW.png") },   // Quân Xe trắng.
        { PieceType.Queen, LoadImage("Assets/QueenW.png") }, // Quân Hậu trắng.
        { PieceType.King, LoadImage("Assets/KingW.png") }    // Quân Vua trắng.
    };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
    {
        // Từ điển ánh xạ loại quân cờ (PieceType) với hình ảnh tương ứng của quân cờ đen.
        { PieceType.Pawn, LoadImage("Assets/PawnB.png") },   // Quân Tốt đen.
        { PieceType.Bishop, LoadImage("Assets/BishopB.png") }, // Quân Tượng đen.
        { PieceType.Knight, LoadImage("Assets/KnightB.png") }, // Quân Mã đen.
        { PieceType.Rook, LoadImage("Assets/RookB.png") },   // Quân Xe đen.
        { PieceType.Queen, LoadImage("Assets/QueenB.png") }, // Quân Hậu đen.
        { PieceType.King, LoadImage("Assets/KingB.png") }    // Quân Vua đen.
    };

        private static ImageSource LoadImage(string filePath)
        {
            // Phương thức nạp hình ảnh từ file với đường dẫn tương đối.
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }

        public static ImageSource GetImage(Player color, PieceType type)
        {
            // Phương thức trả về hình ảnh của quân cờ dựa trên màu sắc (Player) và loại quân (PieceType).
            return color switch
            {
                Player.White => whiteSources[type], // Nếu là quân trắng, lấy từ whiteSources.
                Player.Black => blackSources[type], // Nếu là quân đen, lấy từ blackSources.
                _ => null // Trả về null nếu không hợp lệ.
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            // Phương thức trả về hình ảnh của một quân cờ cụ thể.
            if (piece == null)
            {
                // Trường hợp quân cờ không tồn tại, trả về null.
                return null;
            }

            return GetImage(piece.Color, piece.Type);
            // Gọi phương thức GetImage với màu sắc và loại của quân cờ.
        }
    }

}
