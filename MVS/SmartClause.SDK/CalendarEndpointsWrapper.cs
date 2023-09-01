using SmartClause.SDK.DTO;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<GetCalendarItemsResponseDto> GetCalendarItems(GetCalendarItemsRequestDto requestDto)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest("/api/Calendar/Items", "POST", bodyObject: requestDto);
            return await this.GetResponseAsObject<GetCalendarItemsResponseDto>(request);
        }

        public async Task<GetCalendarYearBreakdownResponseDto> GetCalendarYearBreakdown(
            GetCalendarYearBreakdownRequestDto requestDto)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest("/api/Calendar/Year/Breakdown", "POST", bodyObject: requestDto);
            return await this.GetResponseAsObject<GetCalendarYearBreakdownResponseDto>(request);
        }
    }
}