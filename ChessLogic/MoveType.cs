namespace ChessLogic
{
    public enum MoveType  // Định nghĩa kiểu enum cho các loại nước đi trong cờ vua
    {
        Normal,           // Nước đi bình thường (chỉ di chuyển một quân cờ)
        CastleKS,         // Nước đi tướng (Castle) theo phía cánh vua (Kingside)
        CastleQS,         // Nước đi tướng (Castle) theo phía cánh hậu (Queenside)
        DoublePawn,       // Nước đi của quân tốt (Pawn) di chuyển hai ô lần đầu tiên
        EnPassant,        // Nước đi qua ô của quân tốt đối phương khi nó di chuyển hai ô và có thể bắt
        PawnPromotion     // Nước đi khi quân tốt thăng chức thành quân khác (Hậu, Xe, Mã, Tượng)
    }
}
