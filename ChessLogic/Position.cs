namespace ChessLogic
{
    public class Position
    { // Thuộc tính chỉ đọc đại diện cho hàng của vị trí trên bàn cờ
        public int Row { get; }
        // Thuộc tính chỉ đọc đại diện cho cột của vị trí trên bàn cờ
        public int Column { get; }

        public Position(int row, int column)
        {
            // Gán giá trị tham số row cho thuộc tính Row
            Row = row;
            // Gán giá trị tham số column cho thuộc tính Column
            Column = column;
        }

        // Phương thức xác định màu của ô vuông dựa trên vị trí
        public Player SquareColor()
        {
            if ((Row + Column) % 2 == 0)
            {
                return Player.White;
            }

            return Player.Black;
        }

        // Ghi đè phương thức Equals để kiểm tra hai đối tượng Position có bằng nhau không
        public override bool Equals(object obj)
        {
            // Kiểm tra obj có phải là một đối tượng Position và so sánh Row, Column
            return obj is Position position &&
                   Row == position.Row &&
                   Column == position.Column;
        }
        // Ghi đè phương thức GetHashCode để cung cấp giá trị băm duy nhất cho đối tượng
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
        // Định nghĩa toán tử == để kiểm tra hai đối tượng Position có bằng nhau không
        public static bool operator ==(Position left, Position right)
        {
            // Sử dụng EqualityComparer để so sánh hai đối tượng
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        // Định nghĩa toán tử != để kiểm tra hai đối tượng Position có khác nhau không
        public static bool operator !=(Position left, Position right)
        {
            // Trả về kết quả ngược lại của toán tử ==
            return !(left == right);
        }


        //Cộng một vị trí với một hướng di chuyển
        public static Position operator +(Position pos, Direction dir)
        {
            // Tạo và trả về một vị trí mới bằng cách cộng giá trị RowDelta và ColumnDelta từ Direction
            // vào giá trị Row và Column của Position.
            return new Position(
                pos.Row + dir.RowDelta,      // Tính hàng mới: Cộng hàng hiện tại với thay đổi hàng từ hướng
                pos.Column + dir.ColumnDelta // Tính cột mới: Cộng cột hiện tại với thay đổi cột từ hướng
            );
        }
    }
}
