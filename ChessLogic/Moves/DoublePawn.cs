namespace ChessLogic
{
    public class DoublePawn : Move // Lớp DoublePawn đại diện cho nước đi đặc biệt của quân Tốt khi di chuyển hai ô từ vị trí ban đầu.
    {
        public override MoveType Type => MoveType.DoublePawn; // Loại nước đi là DoublePawn (di chuyển hai ô với quân Tốt).
        public override Position FromPos { get; } // Vị trí ban đầu của quân Tốt.
        public override Position ToPos { get; } // Vị trí đích đến của quân Tốt sau khi di chuyển.

        private readonly Position skippedPos; // Vị trí trung gian mà quân Tốt "bỏ qua" trong nước đi này (cần thiết để thực hiện En Passant).

        // Constructor khởi tạo nước đi DoublePawn.
        // Tính toán vị trí trung gian dựa trên vị trí ban đầu (FromPos) và vị trí đích (ToPos).
        public DoublePawn(Position from, Position to) 
        {
            FromPos = from; // Gán vị trí ban đầu của quân Tốt.
            ToPos = to; // Gán vị trí đích đến.
            skippedPos = new Position((from.Row + to.Row) / 2, from.Column); // Tính vị trí trung gian (hàng nằm giữa).
        }

        public override bool Execute(Board board) // Thực hiện nước đi DoublePawn trên bàn cờ.
        { 
            Player player = board[FromPos].Color; // Lấy người chơi hiện tại từ quân cờ tại vị trí FromPos.
            board.SetPawnSkipPosition(player, skippedPos);   // Đặt vị trí bỏ qua (skippedPos) cho người chơi hiện tại để hỗ trợ logic En Passant.
            new NormalMove(FromPos, ToPos).Execute(board);    // Thực hiện di chuyển thông thường (NormalMove) từ vị trí FromPos đến ToPos.

            return true; // Trả về true để chỉ ra rằng nước đi đã được thực hiện thành công.
        }
    }
}
