namespace ChessLogic
{
    public class EnPassant : Move
    {
        public override MoveType Type => MoveType.EnPassant; // Loại nước đi là EnPassant.
        public override Position FromPos { get; } // Vị trí ban đầu của quân Tốt thực hiện nước đi.
        public override Position ToPos { get; } // Vị trí đích đến của quân Tốt sau khi thực hiện nước đi.

        private readonly Position capturePos;  // Vị trí của quân Tốt đối phương bị bắt (tại cột tương ứng và hàng hiện tại của quân Tốt di chuyển).

        // Constructor khởi tạo nước đi EnPassant.
        // - `from`: Vị trí ban đầu của quân Tốt.
        // - `to`: Vị trí đích đến sau khi quân Tốt thực hiện nước đi.
        public EnPassant(Position from, Position to)
        {
            FromPos = from; // Gán vị trí ban đầu. 
            ToPos = to; // Gán vị trí đích.
            capturePos = new Position(from.Row, to.Column); // Xác định vị trí quân Tốt bị bắt (cùng hàng với FromPos, cùng cột với ToPos).
        }

        public override bool Execute(Board board) // Thực hiện nước đi EnPassant trên bàn cờ.
        { 
            new NormalMove(FromPos, ToPos).Execute(board); // Thực hiện nước đi thông thường từ FromPos đến ToPos.
            board[capturePos] = null; // Loại bỏ quân Tốt bị bắt tại vị trí capturePos.

            return true; // Trả về true để chỉ ra rằng nước đi đã được thực hiện thành công.
        }
    }
}
