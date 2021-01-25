/* Sample Codes:
using APEXAContracting.Common;
using System;
using System.Collections.Generic;
using System.Text;
using DTO = APEXAContracting.Model.DTO;

namespace APEXAContracting.Business.Interface
{
    public interface IRegistrationService
    {
        List<DTO.RegistrationOutTime> GetRegistrationOutTime(Guid regId);

        List<DTO.RegistrationHistoryLocation> GetRegistrationOutDetail(Guid regId, DateTime fromDate, DateTime toDate);

        BusinessResult<string> Save(DTO.Registration model);

        BusinessResult<string> UploadRealData(DTO.BeaconRealData model);

        BusinessResult<string> EndIsolate(string beaconNumber);
    }
}
*/