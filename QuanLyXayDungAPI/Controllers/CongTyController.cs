using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuanLyXayDungAPI.Controllers {
    [Route("api/cong-ty")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]
    public class CongTyController : ControllerBase {
        private readonly CongTyService _getCongTyService;

        public CongTyController(CongTyService getCongTyService) {
            _getCongTyService = getCongTyService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<List<CongTy>> Get() {
            return await _getCongTyService.GetList();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<CongTy> Get(Guid id) {
            return await _getCongTyService.GetByID(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CongTy value) {
            var error = validate(value);
            if (!string.IsNullOrEmpty(error)) {
                return BadRequest(new { code = 400, message = error });
            }
            try {
                var result = await _getCongTyService.Insert(value);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest(new { code = 400, message = ex.Message });
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CongTy value) {
            var error = validate(value);
            if (!string.IsNullOrEmpty(error)) {
                return BadRequest(new { code = 400, message = error });
            }
            try {
                await _getCongTyService.Update(value);
                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { code = 400, message = ex.Message });
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id) {
            Thread.Sleep(1000);
            _getCongTyService.Delete(id);
            return Ok(new { code = 200, message = "Xóa công ty thành công!" });
        }

        private string validate(CongTy value) {
            if (value == null)
                return "Dữ liệu rỗng";
            if(string.IsNullOrEmpty(value.Ten))
                return "Tên công ty không được để trống";
            if(value.Ten.Length > 255)
                return "Tên công ty không được vượt quá 255 ký tự";
            if (string.IsNullOrEmpty(value.DiaChi))
                return "Địa chỉ công ty không được để trống";
            if (value.DiaChi.Length > 500)
                return "Địa chỉ công ty không được vượt quá 500 ký tự";
            if (string.IsNullOrEmpty(value.SDT))
                return "Số điện thoại công ty không được để trống";
            if (value.SDT.Length > 15)
                return "Số điện thoại công ty không được vượt quá 15 ký tự";
            if (value.SDT.Any(c => !Char.IsNumber(c))) {
                return "Số điện thoại công ty không hợp lệ";
            }
            if (string.IsNullOrEmpty(value.Email))
                return "Email công ty không được để trống";
            if (value.Email.Length > 150)
                return "Email công ty không được vượt quá 150 ký tự";
            if (string.IsNullOrEmpty(value.MaSoThue))
                return "Mã số thuế không được để trống";
            if (value.MaSoThue.Length > 15)
                return "Mã số thuế không được vượt quá 15 ký tự";
            return string.Empty;
        }
    }
}
