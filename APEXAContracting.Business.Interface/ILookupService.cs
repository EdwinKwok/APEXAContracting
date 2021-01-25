using APEXAContracting.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using DTO = APEXAContracting.Model.DTO;

namespace APEXAContracting.Business.Interface
{
    public interface ILookupService
    {     
        List<LookupNum> GetLanguageLookup(bool includeEmptyRow);

        List<LookupNum> GetHealthStatusLookup(bool includeEmptyRow);
    }
}
