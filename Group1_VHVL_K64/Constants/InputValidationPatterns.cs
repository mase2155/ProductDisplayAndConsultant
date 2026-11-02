namespace Group1_VHVL_K64.Constants
{
    /// <summary>
    /// Mẫu RegularExpression dùng chung cho form (server + jQuery unobtrusive).
    /// </summary>
    public static class InputValidationPatterns
    {
        public const string Email =
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        public const string EmailErrorMessage =
            "Email không đúng định dạng (ví dụ: ten@example.com).";

        /// <summary>
        /// Ít nhất hai từ, chữ cái Latin / tiếng Việt (tương thích validate phía trình duyệt).
        /// </summary>
        public const string FullName =
            @"^[a-zA-ZÀ-ỹ]+(?:\s+[a-zA-ZÀ-ỹ]+)+$";

        public const string FullNameErrorMessage =
            "Họ và tên phải gồm ít nhất họ và tên (ví dụ: Nguyễn Văn A).";

        /// <summary>
        /// Từ 8 ký tự trở lên và có ít nhất một ký tự đặc biệt.
        /// </summary>
        public const string PasswordStrong =
            @"^(?=.*[!@#$%^&*(),.?""':{}|<>_\-+=\[\]\\;/`~]).{8,100}$";

        public const string PasswordStrongErrorMessage =
            "Mật khẩu phải từ 8 ký tự trở lên và có ít nhất một ký tự đặc biệt.";

        /// <summary>
        /// Số điện thoại Việt Nam; để trống vẫn hợp lệ.
        /// </summary>
        public const string PhoneOptional =
            @"^$|^(0|\+84)[0-9]{8,10}$";

        public const string PhoneOptionalErrorMessage =
            "Số điện thoại không hợp lệ (ví dụ: 0912345678 hoặc +84912345678).";

        /// <summary>
        /// Số điện thoại Việt Nam bắt buộc (0 + 9 số hoặc +84 + 9 số).
        /// </summary>
        public const string PhoneRequired =
            @"^(0[0-9]{9}|\+84[0-9]{9})$";

        public const string PhoneRequiredErrorMessage =
            "Số điện thoại không hợp lệ (bắt đầu bằng 0 hoặc +84, đủ 10 chữ số).";

        /// <summary>
        /// Nội dung liên hệ: 10–2000 ký tự, không chỉ khoảng trắng.
        /// </summary>
        public const string ContactMessage =
            @"^(?!\s*$)[\s\S]{10,2000}$";

        public const string ContactMessageErrorMessage =
            "Nội dung phải từ 10 đến 2000 ký tự và không được để trống.";

        /// <summary>
        /// Email hoặc số điện thoại Việt Nam (form liên hệ nhanh trang chủ).
        /// </summary>
        public const string EmailOrPhone =
            @"^(?:[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}|0[0-9]{9}|\+84[0-9]{9})$";

        public const string EmailOrPhoneErrorMessage =
            "Nhập email hợp lệ hoặc số điện thoại (0 + 9 số hoặc +84 + 9 số).";
    }
}
