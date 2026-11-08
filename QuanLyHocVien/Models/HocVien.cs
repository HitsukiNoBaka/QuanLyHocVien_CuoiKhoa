namespace QuanLyHocVien.Models
{
    public class DiemRenLuyen
    {
        public float chaphanh { get; set; } = 0;
        public float thaiDo { get; set; } = 0;
        public float hangHai { get; set; } = 0;
        public float yThuc { get; set; } = 0;
        public float congTacCD { get; set; } = 0;
        public float total => chaphanh + thaiDo + hangHai + yThuc + congTacCD;
    }
    public class HocVien
    {
        public string ID { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateOnly NgaySinh{ get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string GioiTinh { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public string Lop { get; set; } = string.Empty;
        public DiemRenLuyen DiemRL { get; set; } = new();
    }
}
