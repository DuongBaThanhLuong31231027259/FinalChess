namespace ChessLogic
{
    public enum EndReason
    {
        // Khi ván cờ kết thúc do một bên bị chiếu hết (checkmate)
        Checkmate,

        // Khi ván cờ kết thúc do hòa, không có bên nào có thể thực hiện nước đi hợp lệ (stalemate)
        Stalemate,

        // Khi ván cờ kết thúc theo quy tắc 50 nước, tức là không có
        // quân cờ bị bắt hoặc không có nước đi tốt trong 50 nước liên tiếp (fifty-move rule)
        FiftyMoveRule,

        // Khi ván cờ kết thúc vì không còn đủ quân cờ để thắng (insufficient material)
        InsufficientMaterial,

        // Khi ván cờ kết thúc do một bên lập lại một vị trí cờ ba lần (threefold repetition)
        ThreefoldRepetition
    }

}
