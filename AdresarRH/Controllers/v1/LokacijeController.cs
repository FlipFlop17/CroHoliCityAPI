using AspNetCoreRateLimit;
using CroHoliCityAPI.Model;
using CroHoliCityAPI.Repository.IRepository;
using CroHoliCityAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CroHoliCityAPI.Controllers.v1
{

    [ApiController]
    [Route(template: "api/v{version:ApiVersion}/lokacije")]
    //versioning
    [ApiVersion("1.0")]
    //dohvaća lokacije o gradovima,županijama,poštanskim brojevima
    public class LokacijeController : ControllerBase
    {
        private readonly ILogger<LokacijeController> _logger;
        private readonly IlokacijeRepo _lokacijeDb;
        private readonly IIpPolicyStore _ipStore;
        private ApiResponse _response;
        private readonly IMemoryCache _cache;
        public LokacijeController(ILogger<LokacijeController> logger,IlokacijeRepo lokacijeDb,IIpPolicyStore ipStore,IMemoryCache cache)
        {
            _logger = logger;
            _lokacijeDb = lokacijeDb;
            _ipStore = ipStore;
            _cache = cache;
            _response = new ApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Dohvaća podatke o gradovima/županijama/PB", Description = "Dohvaća podatke o gradovima/županijama/PB")]
        public async Task<ApiResponse> GetLokacije()
        {
            //check if available in cache
            if (_cache.TryGetValue("lokacije", out IEnumerable<Lokacija> cacheEntry)) {
                _response.Data = cacheEntry;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }

            try {
                IEnumerable<Lokacija> result = await _lokacijeDb.GetAllAsync();
                _response.Data = result;
                _response.StatusCode = HttpStatusCode.OK;
                //set cache for 1 minute
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                _cache.Set("lokacije", result, cacheEntryOptions);
            }
            catch (Exception e) {
                ErrorHandler(e.Message,nameof(GetLokacije), HttpStatusCode.InternalServerError);
            }
            return _response;
        }

        [HttpGet("poste/{postanskiBroj}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> GetLokacijeByPostanski(int postanskiBroj)
        {
            //check if available in cache
            if (_cache.TryGetValue(postanskiBroj, out IEnumerable<Lokacija> cacheEntry)) {
                _response.Data = cacheEntry;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            try {
                IEnumerable<Lokacija> result = await _lokacijeDb.GetAllAsync(Lokacija => Lokacija.PostanskiBroj == postanskiBroj, asNoTracking: true);
                if (result.Count() == 0) {
                    ErrorHandler("Nema rezultata za odabrani poštanski broj", nameof(GetLokacijeByPostanski), HttpStatusCode.NotFound);
                } else {
                    //set cache for 1 minute
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    _cache.Set(postanskiBroj, result, cacheEntryOptions);
                    _response.Data = result;
                    _response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception e) {
                ErrorHandler(e.Message, nameof(GetLokacijeByPostanski), HttpStatusCode.InternalServerError);
            }
            return _response;
        }

        [HttpGet("gradovi/{grad}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> GetGradovi(string grad)
        {
            //check if available in cache
            if (_cache.TryGetValue(grad, out IEnumerable<Lokacija> cacheEntry)) {
                _response.Data = cacheEntry;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            try {
                IEnumerable<Lokacija> result= await _lokacijeDb.GetAllAsync(lok => lok.Naziv.ToUpper() == grad.ToUpper(), asNoTracking: true);
                if (result.Count() == 0) {
                    ErrorHandler("Nema rezultata za odabrani grad", nameof(GetGradovi), HttpStatusCode.NotFound);
                } else {
                    //set cache for 1 minute
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    _cache.Set(grad, result, cacheEntryOptions);
                    _response.Data = result;
                    _response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception e) {
                ErrorHandler(e.Message, nameof(GetGradovi), HttpStatusCode.InternalServerError);
            }

            return _response;
        }

        [HttpGet("zupanije/{zupanija}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ApiResponse> GetZupanije(string zupanija)
        {
            //check if available in cache
            if (_cache.TryGetValue(zupanija, out IEnumerable<Lokacija> cacheEntry)) {
                _response.Data = cacheEntry;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            try {
                IEnumerable<Lokacija> result = await _lokacijeDb.GetAllAsync(lok => lok.Zupanija.ToUpper() == zupanija.ToUpper(), asNoTracking: true);
                if (result.Count()==0) {
                    ErrorHandler("Nema rezultata za odabranu županiju", nameof(GetZupanije), HttpStatusCode.NotFound);
                } else {
                    //set cache for 1 minute
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    _cache.Set(zupanija, result, cacheEntryOptions);
                    _response.Data = result;
                    _response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception e) {
                ErrorHandler(e.Message,nameof(GetZupanije), HttpStatusCode.InternalServerError); 
            }

            return _response;
        }

        [HttpGet("naselja/{naselje}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> GetNaselje(string naselje)
        {
            //check if available in cache
            if (_cache.TryGetValue(naselje, out Lokacija cacheEntry)) {
                _response.Data = cacheEntry;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }

            try {
                Lokacija result = await _lokacijeDb.GetAsync(lok => lok.Naselje.ToUpper() == naselje.ToUpper(), asNoTracking: true);
                if (result==null) {
                    ErrorHandler("Nema rezultata za odabrano naselje", nameof(GetNaselje), HttpStatusCode.NotFound);
                } else {
                    //set cache for 1 minute
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    _cache.Set(naselje, result, cacheEntryOptions);
                    _response.Data = result;
                    _response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch ( Exception e) {
                ErrorHandler(e.Message,nameof(GetNaselje),HttpStatusCode.InternalServerError);
            }

            return _response;
        }

        private void ErrorHandler(string errorMsg,string apiCalled,HttpStatusCode code)
        {
            _response.StatusCode = code;
            _response.Success = false;
            _response.Errors.Add(errorMsg);
            using (_logger.BeginScope("Error at: " + apiCalled)) {
                _logger.LogError(errorMsg);
            }
        }
    }
}
