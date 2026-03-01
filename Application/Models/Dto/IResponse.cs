using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Dto {
    public class ApiResponse<T> {
        public string message { get; set; }
        public int code { get; set; }
        public T data { get; set; }

        public static ApiResponse<T> Success(T data, string message = "Thành công", int code = 200) {
            return new ApiResponse<T> { data = data, message = message, code = code };
        }
    }

    public class ApiResponse {
        public string message { get; set; }
        public int code { get; set; }
        public static ApiResponse Failure(string message = "Thất bại", int code = 400) {
            return new ApiResponse { message = message, code = code };
        }
    }
}
