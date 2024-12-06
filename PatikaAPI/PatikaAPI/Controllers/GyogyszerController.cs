using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatikaAPI.Models;
using PatikaAPI.DTOs;

namespace PatikaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GyogyszerController : ControllerBase
    {
        [HttpGet]

        public IActionResult Get()
        {
            using(var context = new PatikaContext())
            {
                try
                {
                    List<Gyogyszer> result = context.Gyogyszers.ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Gyogyszer> result = new List<Gyogyszer>();
                    Gyogyszer hiba = new Gyogyszer()
                    {
                        Id = -1,
                        Nev = ex.Message
                    };
                    result.Add(hiba);
                    return BadRequest(result);
                }
            }
            
        }

        [HttpGet("GogyszerDTO")]

        public IActionResult GetGyogyszerDTO()
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<GyogyszerNevHatoanyagDTO> result = context.Gyogyszers.Select(gy => new GyogyszerNevHatoanyagDTO()
                    {
                        Id=gy.Id,
                        Nev = gy.Nev,
                        Hatoanyag = gy.Hatoanyag,
                    }).ToList();
                    
                    return StatusCode(200, result);
                }
                catch (Exception ex)
                {
                    List<GyogyszerNevHatoanyagDTO> hiba = new();
                    GyogyszerNevHatoanyagDTO hibaDTO = new GyogyszerNevHatoanyagDTO()
                    {
                        Id = -1,
                        Nev = ex.Message
                    };
                    return BadRequest(hibaDTO);
                }
            }

        }

        [HttpGet("ById")]
        public IActionResult Get(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    Gyogyszer result = context.Gyogyszers.FirstOrDefault (x => x.Id == id);
                    return Ok(result);
                }
                catch (Exception ex)
                {
            
                    Gyogyszer hiba = new Gyogyszer()
                    {
                        Id = -1,
                        Nev = ex.Message
                    };
                    
                    return BadRequest(hiba);
                }
            }   
        }
        [HttpGet("ToBetegsegName")]
        public IActionResult Get(string bname)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Gyogyszer> result = context.Kezels.Include(k => k.Gyogyszer).Include(k => k.Betegseg).Where(k => k.Betegseg.Megnevezes == bname).Select(k => k.Gyogyszer).ToList();
                    return Ok(result);
                }
                catch (Exception ex)    
                {
                    List<Gyogyszer> result = new List<Gyogyszer>();
                    Gyogyszer hiba = new()
                    {
                        Id = -1,
                        Nev = ex.Message
                    };
                    result.Add(hiba);
                    return BadRequest(result);

                }
            }
        }

        [HttpGet("ToBetegsegId")]
        public IActionResult GetById(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Gyogyszer> result = context.Kezels.Include(k => k.Gyogyszer).Include(k => k.Betegseg).Where(k => k.Betegseg.Id  == id).Select(k => k.Gyogyszer).ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Gyogyszer> result = new List<Gyogyszer>();
                    Gyogyszer hiba = new()
                    {
                        Id = -1,
                        Nev = ex.Message
                    };
                    result.Add(hiba);
                    return BadRequest(result);

                }
            }
        }
        [HttpPost("UjGyogyszer")]
        public IActionResult Post(Gyogyszer ujGogyszer)
        {
            using(var context = new PatikaContext())
            {
                try
                {
                    context.Gyogyszers.Add(ujGogyszer);
                    context.SaveChanges();
                    return StatusCode(200, "Sikeres rögzítés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("TorolGyogyszer")]
        public IActionResult Delete(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    Gyogyszer torlendo = new Gyogyszer()
                    {
                        Id = id
                    };
                    context.Gyogyszers.Remove(torlendo);
                    context.SaveChanges();
                    return StatusCode(200, "Sikeres törlés!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ModositGyogyszer")]
        public IActionResult Put(Gyogyszer ujGogyszer)
        {
            using (var context = new PatikaContext())
            {
                try
                {

                    if (context.Gyogyszers.FirstOrDefault(gy => gy.Id == ujGogyszer.Id) is not null)
                    {
                        context.Gyogyszers.Update(ujGogyszer);
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

    }
}
