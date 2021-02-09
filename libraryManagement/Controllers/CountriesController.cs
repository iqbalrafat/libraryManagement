﻿using libraryManagement.DTos;
using libraryManagement.models;
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
    public class CountriesController : Controller

    {
        private ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //the countries list will return both country name and null value for author
            //it is therefore better to use Dto to get the specific field in this case Id and country name
            var countriesDto = new List<CountryDto>();
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }
            return Ok(countriesDto);
        }
        //api/countries/countryId
        [HttpGet("{countryId}", Name ="GetCountry")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
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
        //api/countries/authorId
        [HttpGet("authors/{authorId}")]
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
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200,Type =typeof(IEnumerable<AuthorDto>))]
        public IActionResult GetAuthorsFromACountry(int countryId)
           
        {
            if (!_countryRepository.CountryExist(countryId))
                return NotFound();
                var authors = _countryRepository.GetAuthorsFromACountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            
            var authorDto = new List<AuthorDto>();
            foreach(var author in authors)
            {
                authorDto.Add(new AuthorDto()
                {
                    id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

           return Ok(authorDto);
        }
        //api/countries
        [HttpPost]
        [ProducesResponseType(200,Type =typeof(Country))]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]


        public IActionResult CreateCountry([FromBody]Country countryToCreate)
        {
            if (countryToCreate == null)
                return BadRequest(ModelState);
            var country = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper()).FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", $"Country {countryToCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_countryRepository.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {countryToCreate.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCountry", new { countryId = countryToCreate.Id },countryToCreate);
        }
    }
}
