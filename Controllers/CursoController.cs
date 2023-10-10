using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Mvc;
using apiUniversidade.Context;

namespace apiUniversidade.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursoController : Controller
    {
       private readonly ILogger<CursoController> _logger;
       private readonly apiUniversidadeContext _context;

       public CursoController(ILogger<CursoController> logger , apiUniversidadeContext context)
       {
        _logger = logger;
        _context = context;
       }
       
        [HttpGet]

        public ActionResult<IEnumerable<Curso>> Get()
        {
            var cursos = _context.Cursos.ToList();
            if(cursos is null)
                return NotFound();
            
            return cursos;
        }
    }
}