using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<Guid> Post([FromBody] CongTy value) {
            var result = await _getCongTyService.Insert(value);
            return result;
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public void Put([FromBody] CongTy value) {
            var result = _getCongTyService.Update(value);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id) {
            _getCongTyService.Delete(id);
            return Ok(new { code = 200, message = "Xóa công ty thành công!" });
        }
    }
}
