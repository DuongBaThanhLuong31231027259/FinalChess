namespace ChessLogic
{
    // Định nghĩa class Result trong namespace ChessLogic, dùng để lưu trữ kết quả của ván cờ.
    public class Result
    {
        // Thuộc tính Winner lưu trữ người chơi chiến thắng (nếu có).
        public Player Winner { get; }

        // Thuộc tính Reason lưu trữ lý do kết thúc ván cờ (ví dụ: Chiếu hết, hòa, hết quân...).
        public EndReason Reason { get; }

        // Constructor của class Result, nhận vào người thắng (winner) và lý do kết thúc (reason).
        public Result(Player winner, EndReason reason)
        {
            Winner = winner; // Gán giá trị người thắng cho thuộc tính Winner.
            Reason = reason; // Gán giá trị lý do kết thúc cho thuộc tính Reason.
        }

        // Phương thức tĩnh (static) Win: tạo và trả về một kết quả với người thắng và lý do là Chiếu hết.
        public static Result Win(Player winner)
        {
            return new Result(winner, EndReason.Checkmate); // Trả về kết quả thắng với lý do là Checkmate (Chiếu hết).
        }

        // Phương thức tĩnh (static) Draw: tạo và trả về một kết quả hòa với lý do kết thúc đã cho.
        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason); // Trả về kết quả hòa, không có người thắng (Player.None).
        }
    }
}
