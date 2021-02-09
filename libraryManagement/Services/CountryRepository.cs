using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public class CountryRepository : ICountryRepository
    {
        private LibraryDbContext _countryContext;
        public CountryRepository(LibraryDbContext countryContext)
        {
            _countryContext = countryContext;
        }
        public bool CountryExist(int countryId)
        {
            return _countryContext.Countries.Any(c => c.Id == countryId);
        }

        public bool CreateCountry(Country country)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCountry(Country country)
        {
            throw new NotImplementedException();
        }

        public ICollection<Author> GetAuthorsFromACountry(int countryId)
        {
            return _countryContext.Authors.Where(c => c.Country.Id == countryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _countryContext.Countries.OrderBy(c => c.Name).ToList();            
            
        }

        public Country GetCountry(int countryId)
        {
            return _countryContext.Countries.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryOfAnAuthor(int authorId)
        {
            return _countryContext.Authors.Where(a => a.Id == authorId).Select(c => c.Country).FirstOrDefault();
        }

        public bool IsDuplicateCountryName(int countryId, string countryName)
        {
            var country =_countryContext.Countries.Where(c => c.Name.Trim().ToUpper() == countryName.Trim().ToUpper() && c.Id == countryId).FirstOrDefault();
            return country == null ? false : true;
        }

        public bool Save()
        {
      //to check the CRUD Operation successeded we use SaveChanges() method. It returns 0, 1 or -ve number. if it 
      //is zero mean nothing but not error. If it is 1 mean changed occur successfully, -ve mean an error
      //we use this method for all our crud operation.
            var saved = _countryContext.SaveChanges();
            return saved >=0 ? true : false;
        }
        public bool UpdateCountry(Country country)
        {
            _countryContext.Update(country);
            return Save();
        }
    }
}
