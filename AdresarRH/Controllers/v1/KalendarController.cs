using AspNetCoreRateLimit;
using CroHoliCityAPI.Model;
using CroHoliCityAPI.Repository.IRepository;
using CroHoliCityAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace CroHoliCityAPI.Controllers.v1
{
    [ApiController]
    [Route(template: "api/v{version:ApiVersion}/kalendar")]
    //versioning
    [ApiVersion("1.0")]
    /// kontroler koji sluzi za dohvat neradnih dana u godini
    public class KalendarController : ControllerBase
    {
        private readonly IKalendarRepo _kalendarRepo;
        private readonly ILogger<KalendarController> _logger;
        private readonly IMemoryCache _memoryCacher;
        private readonly IIpPolicyStore _policyStore;
        private ApiResponse _response;

        public KalendarController(IKalendarRepo kalendarRepo,ILogger<KalendarController> logger,IMemoryCache memoryCacher,IIpPolicyStore policyStore)
        {
            _kalendarRepo = kalendarRepo;
            _logger = logger;
            _memoryCacher = memoryCacher;
            _policyStore = policyStore;
            _response=new ApiResponse();
            _logger.LogInformation("KalendarController created");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        /// <summary>
        /// Dohvaća sve neradne dane u tekućoj godini
        /// 
        public async Task<ApiResponse> DohvatiNeradneDane()
        {
            List<Dan> neradniDani = new List<Dan>();
            //check if available in cache
            if (_memoryCacher.TryGetValue("neradniDani", out IEnumerable<Dan> cacheEntry)) {
                _response.Data = cacheEntry;
                _response.StatusCode = HttpStatusCode.OK;
                return _response;
            }
            try {
                neradniDani = await _kalendarRepo.GetAllAsync(neradniDan=>neradniDan.NeradniDan==true,asNoTracking:true);
                _response.Data = neradniDani;
                _response.StatusCode = HttpStatusCode.OK;
                //set cache for 1 minute
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                _memoryCacher.Set("neradniDani", neradniDani, cacheEntryOptions);
            }
            catch (Exception e) {
                ErrorHandler(e.Message,nameof(DohvatiNeradneDane), HttpStatusCode.InternalServerError);
                
            }
            return _response;
        }
        [HttpGet(template:"uskrs/{godina:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApiResponse> DohvatiDatumUskrsa(int godina)
        {
            DateTime uskrs=Helpers.CalculateEasterDate(godina);
            return ResponseOk(_response,uskrs);
        }

        [HttpGet(template: "tjelovo/{godina:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApiResponse> DohvatiDatumTjelovo(int godina)
        {
            DateTime uskrs = Helpers.CalculateTjelovoDate(godina);
            return ResponseOk(_response, uskrs);
        }
        private void ErrorHandler(string errorMsg, string apiCalled, HttpStatusCode code)
        {
            _response.StatusCode = code;
            _response.Success = false;
            _response.Errors.Add(errorMsg);
            using (_logger.BeginScope("Error at: " + apiCalled)) {
                _logger.LogError(errorMsg);
            }
        }
        private ApiResponse ResponseOk(ApiResponse resp,object respData)
        {
            resp.StatusCode = HttpStatusCode.OK;
            resp.Success = true;
            resp.Data = respData;
            return resp;
        }

    }
}
