using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities {
    public class CongTy {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string MaSoThue { get; set; }
        public int Plan { get; set; } // 1: Basic, 2: Pro
        public DateTime NgayTao { get; set; }
    }
}
