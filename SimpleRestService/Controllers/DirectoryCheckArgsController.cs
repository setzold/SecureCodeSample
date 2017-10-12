using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimpleRestService.Controllers
{
    [Route("api/[controller]")]
    public class DirectoryCheckArgsController : Controller
    {
        private readonly ILogger _logger;

        public DirectoryCheckArgsController(ILogger<DirectoryController> logger)
        {
            _logger = logger;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Bitte geben Sie in die URL den Rechnernamen ein, dessen freigegebenen Ordner Sie auflisten möchten" };
        }

        // GET api/directory/machine
        [HttpGet("{machine}/{share}")]
        public string Get(string machine, string share)
        {
            string targetShare = string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(machine)) throw new ArgumentNullException(nameof(machine), "Machine ressource was null or empty. Please insert valid machine name.");
                if (string.IsNullOrWhiteSpace(share)) throw new ArgumentNullException(nameof(machine), "Share ressource was null or empty. Please insert valid share name.");

                //string decodeShareressource = HttpUtility.UrlDecode(share);
                //if (decodeShareressource.Contains("$"))
                //    throw new InvalidOperationException("@The share ressource contains an invalid character '$'");

                targetShare = $"\\\\{machine}\\{share}";

                var dirs = System.IO.Directory.GetDirectories(targetShare);
                var files = System.IO.Directory.GetFiles(targetShare);
                return $"{string.Join(Environment.NewLine, dirs)}" +
                    $"{string.Join(Environment.NewLine, files)}";
            }
            //catch (System.IO.IOException ex)
            //{
            //    return $"Share {targetShare} was not found {ex.Message}";
            //}
            catch (Exception ex) //Not only default exceptions
            {
                _logger.LogError($"Error on listing share for machine {machine}.", ex.Message);
                return ex.Message;
            }
        }
    }
}