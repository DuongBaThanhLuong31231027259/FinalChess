namespace ChessLogic
{
    // Định nghĩa một kiểu liệt kê (enum) tên là Player để biểu diễn trạng thái của người chơi.
    public enum Player
    {
        // None: Không có người chơi nào chiến thắng (trường hợp ván cờ hòa).
        None,
        // White: Người chơi cầm quân trắng.
        White,
        // Black: Người chơi cầm quân đen.
        Black

    }

    public static class PlayerExtensions
    {
        // Phương thức này trả về đối thủ của người chơi hiện tại.
        public static Player Opponent(this Player player)
        {
            // Sử dụng câu lệnh switch để kiểm tra giá trị của người chơi hiện tại.
            switch (player)
            {
                // Nếu người chơi hiện tại là White, trả về Black (đối thủ là người chơi cầm quân đen).
                case Player.White:
                    return Player.Black;
                // Nếu người chơi hiện tại là Black, trả về White (đối thủ là người chơi cầm quân trắng).
                case Player.Black:
                    return Player.White;
                // Mặc định (trường hợp không rơi vào White hoặc Black), trả về None.
                default:
                    return Player.None;
            }
        }
    }
}
