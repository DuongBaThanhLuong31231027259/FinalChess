namespace ChessLogic
{
    public class PawnPromotion : Move
    {
        // Thuộc tính ghi đè (override) để xác định loại nước đi là phong cấp (PawnPromotion).
        public override MoveType Type => MoveType.PawnPromotion;

        // Vị trí ban đầu của quân tốt trước khi thực hiện nước đi phong cấp.
        public override Position FromPos { get; }

        // Vị trí mới mà quân tốt được di chuyển đến và phong cấp.
        public override Position ToPos { get; }

        private readonly PieceType newType;


        // Constructor nhận vào vị trí bắt đầu (from), vị trí kết thúc (to), 
        // và loại quân cờ mà quân tốt sẽ phong cấp.
        public PawnPromotion(Position from, Position to, PieceType newType)
        {
            FromPos = from;
            ToPos = to;
            this.newType = newType;
        }

        // Phương thức tạo quân cờ mới (Knight, Bishop, Rook hoặc Queen) dựa trên loại quân cờ được chọn.
        private Piece CreatePromotionPiece(Player color)
        {
            return newType switch
            {
                PieceType.Knight => new Knight(color), // Nếu chọn phong cấp thành Mã.
                PieceType.Bishop => new Bishop(color), // Nếu chọn phong cấp thành Tượng.
                PieceType.Rook => new Rook(color), // Nếu chọn phong cấp thành Xe.
                _ => new Queen(color) // Mặc định phong cấp thành Hậu
            };
        }

        // Thực hiện nước đi phong cấp trên bàn cờ.
        public override bool Execute(Board board)
        {
            
            Piece pawn = board[FromPos]; // Lấy quân tốt tại vị trí ban đầu.
            board[FromPos] = null; // Xóa quân tốt khỏi vị trí ban đầu.

            // Tạo quân cờ mới sau khi phong cấp với màu sắc của quân tốt ban đầu.
            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true; // Đánh dấu quân cờ mới là đã di chuyển (HasMoved = true).
            board[ToPos] = promotionPiece; // // Đặt quân cờ mới tại vị trí đích trên bàn cờ.

            return true; // Trả về true để cho biết nước đi đã được thực hiện thành công.
        }
    }
}
