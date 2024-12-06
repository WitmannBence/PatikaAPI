using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatikaAPI.DTOs;
using PatikaAPI.Models;

namespace PatikaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetegsegController : ControllerBase
    {
        #region Szinkron végpontok
        [HttpGet]
        public IActionResult Get()
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = context.Betegsegs.ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result = new List<Betegseg>();
                    Betegseg hiba = new Betegseg()
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    };
                    result.Add(hiba);
                    return BadRequest(result);
                }
            }

        }
        [HttpGet("BetegsegById")]
        public IActionResult Get(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    Betegseg result = context.Betegsegs.FirstOrDefault(x => x.Id == id);
                    if (result == null)
                        return NotFound("Nincs ilyen azonosítójú betegség");
                    else
                        return Ok(result);
                }
                catch (Exception ex)
                {

                    Betegseg hiba = new Betegseg()
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    };

                    return BadRequest(hiba);
                }
            }
        }

        [HttpGet("ToGyogyszerName")]
        public IActionResult Get(string gyname)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = context.Kezels.Include(k => k.Betegseg).Include(k => k.Gyogyszer).Where(k => k.Gyogyszer.Nev == gyname).Select(k => k.Betegseg).ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result = new List<Betegseg>();
                    Betegseg hiba = new()
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    };
                    result.Add(hiba);
                    return BadRequest(result);

                }
            }
        }

        [HttpGet("ToGyogyszerId")]
        public IActionResult GetById(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = context.Kezels.Include(k => k.Betegseg).Include(k => k.Gyogyszer).Where(k => k.Gyogyszer.Id == id).Select(k => k.Betegseg).ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result = new List<Betegseg>();
                    Betegseg hiba = new()
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    };
                    result.Add(hiba);
                    return BadRequest(result);

                }
            }
        }

        [HttpGet("BetegsegDTO")]

        public IActionResult GetBetegsegDTO()
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<IdMegnevezesDTO> result = context.Betegsegs.Select(gy => new IdMegnevezesDTO()
                    {
                        Id = gy.Id,
                        Megnevezes = gy.Megnevezes
                    }).ToList();

                    return StatusCode(200, result);
                }
                catch (Exception ex)
                {
                    List<IdMegnevezesDTO> hiba = new();
                    IdMegnevezesDTO hibaDTO = new IdMegnevezesDTO()
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    };
                    return BadRequest(hibaDTO);
                }
            }

        }

        [HttpPost("UjBetegseg")]
        public IActionResult Post(Betegseg ujGogyszer)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    context.Betegsegs.Add(ujGogyszer);
                    context.SaveChanges();
                    return StatusCode(200, "Sikeres rögzítés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("TorolBetegseg")]
        public IActionResult Delete(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    Betegseg torlendo = new Betegseg()
                    {
                        Id = id
                    };
                    context.Betegsegs.Remove(torlendo);
                    context.SaveChanges();
                    return StatusCode(200, "Sikeres törlés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ModositBetegseg")]
        public IActionResult Put(Betegseg modositBetegseg)
        {
            using (var context = new PatikaContext())
            {
                try
                {

                    if (context.Betegsegs.Contains(modositBetegseg))
                    {
                        context.Betegsegs.Update(modositBetegseg);
                        context.SaveChanges();
                        return StatusCode(200, "Sikeres módosítás!");
                    }
                    else
                    {
                        return NotFound("Nincs ilyen azonosítójú gyógyszer!");
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region Aszinkron 

        [HttpGet("GetAllAsynx")]
        public async Task<IActionResult> GetAllAsync()
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = await context.Betegsegs.ToListAsync();
                    return Ok(result);
                }
                catch (Exception ex)
                {

                    List<Betegseg> result = new List<Betegseg>()
                    {
                        new Betegseg() {
                        Id = -1,
                        Megnevezes = ex.Message
                        }

                    };
                    return BadRequest(result);
                }



                #endregion
            }
        }
    }
}
