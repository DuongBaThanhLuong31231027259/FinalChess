namespace ChessLogic
{
    // Lớp trừu tượng đại diện cho một nước đi trong cờ vua
    public abstract class Move  
    {
        // Thuộc tính trừu tượng chỉ loại nước đi (ví dụ: bình thường, tướng, thăng chức, ...)
        public abstract MoveType Type { get; }
        // Thuộc tính trừu tượng chỉ vị trí xuất phát của quân cờ
        public abstract Position FromPos { get; }
        // Thuộc tính trừu tượng chỉ vị trí đích của quân cờ
        public abstract Position ToPos { get; }

        // Phương thức trừu tượng để thực thi nước đi trên bàn cờ
        public abstract bool Execute(Board board);

        // Phương thức kiểm tra tính hợp lệ của nước đi
        public virtual bool IsLegal(Board board)  
        {
            // Lấy màu của người chơi hiện tại từ quân cờ tại vị trí xuất phát
            Player player = board[FromPos].Color;
            // Tạo một bản sao của bàn cờ hiện tại
            Board boardCopy = board.Copy();
            // Thực thi nước đi trên bản sao của bàn cờ
            Execute(boardCopy);  
            // Kiểm tra xem sau khi thực thi nước đi, người chơi hiện tại có bị chiếu không
            return !boardCopy.IsInCheck(player);  
        }
    }

}
