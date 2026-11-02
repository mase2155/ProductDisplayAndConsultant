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
    }
}
