using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KafkaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KafkaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> logger;
        private readonly IProdutoProducer produtoProducer;

        public ProdutoController(ILogger<WeatherForecastController> logger, IProdutoProducer produtoProducer)
        {
            this.logger = logger;
            this.produtoProducer = produtoProducer;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            var message = JsonConvert.SerializeObject(produto); 
            var retorno = await produtoProducer.EnviarMensagem("produtos", message);
            return Ok(retorno);
        }
    }
}