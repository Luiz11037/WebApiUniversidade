using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Mvc;

namespace apiUniversidade.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CursoController : Controller
    {
       private reandonly ILogger<CursoController> _logger;
       private reandonly apiUniversidade _context;

       public CursoController(ILogger<CursoController> logger , apiUniversidade context)
       {
        _logger = logger;
        _context = context;
       }
       
        [HttpGet]

        public ActionResult<IEnumerable<Cursos>> Get()
        {
            var cursos = context.Cursos.ToList();
            if(cursos is null)
                return NotFound();
            
            return cursos;
        }
    }
}