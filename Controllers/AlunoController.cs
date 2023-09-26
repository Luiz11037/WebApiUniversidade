using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Mvc;

namespace apiUniversidade.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly ILogger<AlunoController> _logger;

        public AlunoController(ILogger<AlunoController> logger)
        {
            _logger = logger;
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