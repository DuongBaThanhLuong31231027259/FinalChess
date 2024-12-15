namespace ChessLogic
{
    // Lớp NormalMove kế thừa từ lớp Move, đại diện cho một nước đi bình thường trong cờ vua
    public class NormalMove : Move
    {
        // Thuộc tính Type trả về kiểu nước đi là Normal (bình thường)
        public override MoveType Type => MoveType.Normal;

        // Thuộc tính FromPos lưu trữ vị trí xuất phát của quân cờ
        public override Position FromPos { get; }

        // Thuộc tính ToPos lưu trữ vị trí đích của quân cờ
        public override Position ToPos { get; }

        // Constructor của lớp NormalMove, nhận vào vị trí xuất phát và đích
        public NormalMove(Position from, Position to)
        {
            FromPos = from; // Gán giá trị vị trí xuất phát
            ToPos = to;     // Gán giá trị vị trí đích
        }

        // Phương thức Execute thực hiện nước đi trên bàn cờ
        public override bool Execute(Board board)
        {
            Piece piece = board[FromPos]; // Lấy quân cờ tại vị trí xuất phát
            bool capture = !board.IsEmpty(ToPos); // Kiểm tra nếu có quân cờ bị bắt ở vị trí đích

            // Di chuyển quân cờ từ vị trí xuất phát đến vị trí đích
            board[ToPos] = piece;
            board[FromPos] = null;

            piece.HasMoved = true; // Đánh dấu quân cờ đã di chuyển

            // Trả về true nếu quân cờ bị bắt hoặc quân cờ là quân tốt (Pawn)
            return capture || piece.Type == PieceType.Pawn;
        }
    }
}
