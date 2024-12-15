namespace ChessLogic
{
    public class Direction
    {
        // Định nghĩa các hướng cơ bản bằng cách tạo các đối tượng Direction với giá trị thay đổi hàng và cột
        public readonly static Direction North = new Direction(-1, 0); // Hướng Bắc: Giảm hàng, không đổi cột
        public readonly static Direction South = new Direction(1, 0);  // Hướng Nam: Tăng hàng, không đổi cột
        public readonly static Direction East = new Direction(0, 1);   // Hướng Đông: Không đổi hàng, tăng cột
        public readonly static Direction West = new Direction(0, -1);  // Hướng Tây: Không đổi hàng, giảm cột

        // Định nghĩa các hướng chéo bằng cách cộng các hướng cơ bản
        public readonly static Direction NorthEast = North + East; // Hướng Đông Bắc
        public readonly static Direction NorthWest = North + West; // Hướng Tây Bắc
        public readonly static Direction SouthEast = South + East; // Hướng Đông Nam
        public readonly static Direction SouthWest = South + West; // Hướng Tây Nam

        // Thuộc tính lưu trữ thay đổi về hàng
        public int RowDelta { get; }
        // Thuộc tính lưu trữ thay đổi về cột
        public int ColumnDelta { get; }

        // Constructor để khởi tạo một hướng với thay đổi hàng và cột cụ thể
        public Direction(int rowDelta, int columnDelta)
        {
            RowDelta = rowDelta;   // Gán giá trị thay đổi hàng
            ColumnDelta = columnDelta; // Gán giá trị thay đổi cột
        }

        // Định nghĩa toán tử cộng để cộng hai hướng, trả về hướng mới
        public static Direction operator +(Direction dir1, Direction dir2)
        {
            // Tạo hướng mới bằng cách cộng giá trị thay đổi hàng và cột của hai hướng
            return new Direction(dir1.RowDelta + dir2.RowDelta, dir1.ColumnDelta + dir2.ColumnDelta);
        }

        // Định nghĩa toán tử nhân để nhân một hướng với một số nguyên (scalar), trả về hướng mới
        public static Direction operator *(int scalar, Direction dir)
        {
            // Tạo hướng mới bằng cách nhân giá trị thay đổi hàng và cột với scalar
            return new Direction(scalar * dir.RowDelta, scalar * dir.ColumnDelta);
        }
    }
}
