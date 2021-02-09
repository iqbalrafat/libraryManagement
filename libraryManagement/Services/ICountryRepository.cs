using libraryManagement.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libraryManagement.Services
{
    public interface ICountryRepository

    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountryOfAnAuthor(int authorId);
        ICollection<Author> GetAuthorsFromACountry(int countryId);
        bool CountryExist(int countryId);
        bool IsDuplicateCountryName(int countryId, string countryName);
        //CRUD Operation Method (Create, update and Delete)

        // To create/Delete/Update country we have to provide an object of Country. Then save method will let us know
        //if it is created or not
        bool CreateCountry(Country country);
              
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
        
    }
}
