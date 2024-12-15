using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ChessUI
{
    public static class ChessCursors
    {
        // Định nghĩa con trỏ chuột cho quân cờ trắng (WhiteCursor)
        // Khi quân cờ trắng được chọn, con trỏ này sẽ được sử dụng.
        public static readonly Cursor WhiteCursor = LoadCursor("Assets/CursorW.cur");

        // Định nghĩa con trỏ chuột cho quân cờ đen (BlackCursor)
        // Khi quân cờ đen được chọn, con trỏ này sẽ được sử dụng.
        public static readonly Cursor BlackCursor = LoadCursor("Assets/CursorB.cur");

        // Phương thức LoadCursor dùng để tải một con trỏ từ một file .cur
        // Phương thức này nhận tham số filePath là đường dẫn đến tệp con trỏ.
        private static Cursor LoadCursor(string filePath)
        {
            // Lấy stream của tài nguyên con trỏ từ tệp được chỉ định.
            // Đường dẫn tài nguyên được cung cấp dưới dạng một URI (Uniform Resource Identifier) tương đối.
            // UriKind.Relative cho biết rằng đường dẫn này là tương đối.
            Stream stream = Application.GetResourceStream(new Uri(filePath, UriKind.Relative)).Stream;

            // Tạo một đối tượng Cursor mới từ stream và trả về nó.
            return new Cursor(stream, true);
        }
    }

}
