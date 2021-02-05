using libraryManagement.DTos;
using libraryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController: Controller
        
    {
        private ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200,Type=typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //the countries list will return both country name and null value for author
            //it is therefore better to use Dto to get the specific field in this case Id and country name
            var countriesDto = new List<CountryDto>();
            foreach(var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
             return Ok (countriesDto);
        }
        //api/countries/countryId
        [HttpGet("{countryId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type=typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            var country = _countryRepository.GetCountry(countryId);
            var countryDto = new CountryDto()
            {
                Name = country.Name,
                Id = country.Id
            };
            return Ok(countryDto);
        }
        //api/countries/authryId
        [HttpGet("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            var country = _countryRepository.GetCountryOfAnAuthor(authorId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };
            return Ok(countryDto);
           
        }

    }
}
