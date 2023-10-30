using GiornateMondiali.Core.Models;

namespace GiornateMondiali.Core
{
    public interface IGmService
    {
        SpecialDayResponse GetSpecialDays(DateTime date);
    }
}
