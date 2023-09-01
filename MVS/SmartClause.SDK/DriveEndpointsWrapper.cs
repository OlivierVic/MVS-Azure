using SmartClause.SDK.DTO;
using SmartClause.SDK.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<DriveItemDTO>> QueryDrive(string tenantId, string lang, string folderId, bool negotiation, bool signature, bool signed, bool validated, string sortField, bool isDesc, int pageNumber, int pageSize)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Drive/QueryDrive?lang={lang}&folderId={folderId}&negotiation={negotiation}&signature={signature}&signed={signed}&sortField={sortField}&validated={validated}&isDesc={isDesc}&pageNumber={pageNumber}&pageSize={pageSize}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<DriveItemDTO>>(request);
        }
        public async Task<List<DriveItemDTO>> QueryTrash(string tenantId, string lang, string sortField, bool isDesc, int pageNumber, int pageSize)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Drive/QueryTrash?lang={lang}&sortField={sortField}&isDesc={isDesc}&pageNumber={pageNumber}&pageSize={pageSize}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<DriveItemDTO>>(request);
        }
        public async Task<List<DriveItemDTO>> QuerySearch(string tenantId, string lang, string search, bool negotiation, bool signature, bool signed, string sortField, bool isDesc, int pageNumber, int pageSize)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Drive/QuerySearch?lang={lang}&search={HttpUtility.UrlEncode(search)}&negotiation={negotiation}&signature={signature}&signed={signed}&sortField={sortField}&isDesc={isDesc}&pageNumber={pageNumber}&pageSize={pageSize}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<DriveItemDTO>>(request);
        }
        public async Task<List<SearchUserDTO>> QueryAdvancedSearchOwners(string tenantId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Drive/QueryAdvancedSearchOwners", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<SearchUserDTO>>(request);
        }
        public async Task<List<SearchUserDTO>> QueryAdvancedSearchContributors(string tenantId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Drive/QueryAdvancedSearchContributors", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<SearchUserDTO>>(request);
        }
        public async Task<List<TermSheetDTO>> QueryAdvancedSearchTermSheets(string tenantId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Drive/QueryAdvancedSearchTermSheets", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<TermSheetDTO>>(request);
        }
        public async Task<List<DriveItemDTO>> QueryAdvancedSearch(SearchRequest searchRequest, string tenantId = null)
        {
            DriveSearchRequest driveSearchRequest = new()
            {
                Search = searchRequest.Query,
                None = false,
                Negotiation = false,
                Signature = false,
                Signed = false,
                Validated = false,
                SignedComparator = 1,
                CreationComparator = 1,
                Language = searchRequest.Criteria.FirstOrDefault(c => c.Type == SearchCriterionRequest.TypeEnum.Language)?.Data?.Language,
                SheetElements = new List<SearchSheetElement>(),
                Contributors = new string[] { },
                SortField = "name",
                IsDescending = false,
                PageNumber = Convert.ToInt32(searchRequest.SkipItems.Value / searchRequest.NumberItems.Value) + 1,
                PageSize = searchRequest.NumberItems.Value
            };

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.Status))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.Status);
                if ((criterion.Data?.Status?.Length ?? 0) != 0)
                {
                    driveSearchRequest.None = criterion.Data?.Status?.Contains("None") ?? false;
                    driveSearchRequest.Negotiation = criterion.Data?.Status?.Contains("InNegotiation") ?? false;
                    driveSearchRequest.Signature = criterion.Data?.Status?.Contains("InSignature") ?? false;
                    driveSearchRequest.Signed = criterion.Data?.Status?.Contains("Signed") ?? false;
                    driveSearchRequest.Validated = criterion.Data?.Status?.Contains("Validated") ?? false;
                }
            }
            else
            {
                driveSearchRequest.None = true;
                driveSearchRequest.Negotiation = true;
                driveSearchRequest.Signature = true;
                driveSearchRequest.Signed = true;
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.DateContractStart))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.DateContractStart);
                if (criterion.Data.StartDateTime.HasValue && criterion.Data.EndDateTime.HasValue)
                {
                    driveSearchRequest.CreationComparator = 6;
                    driveSearchRequest.CreationDate = criterion.Data.StartDateTime.Value.StartOfDay();
                    driveSearchRequest.CreationBetween = criterion.Data.EndDateTime.Value.EndOfDay();
                }
                else if (!criterion.Data.StartDateTime.HasValue && criterion.Data.EndDateTime.HasValue)
                {
                    driveSearchRequest.CreationComparator = 2;
                    driveSearchRequest.CreationDate = criterion.Data.EndDateTime.Value.EndOfDay();
                }
                else if (criterion.Data.StartDateTime.HasValue && !criterion.Data.EndDateTime.HasValue)
                {
                    driveSearchRequest.CreationComparator = 1;
                    driveSearchRequest.CreationDate = criterion.Data.StartDateTime.Value.StartOfDay();
                }
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.DateSignature))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.DateSignature);
                if (criterion.Data.StartDateTime.HasValue && criterion.Data.EndDateTime.HasValue)
                {
                    driveSearchRequest.SignedComparator = 6;
                    driveSearchRequest.SignedDate = criterion.Data.StartDateTime.Value.StartOfDay();
                    driveSearchRequest.SignedBetween = criterion.Data.EndDateTime.Value.EndOfDay();
                }
                else if (!criterion.Data.StartDateTime.HasValue && criterion.Data.EndDateTime.HasValue)
                {
                    driveSearchRequest.SignedComparator = 2;
                    driveSearchRequest.SignedDate = criterion.Data.EndDateTime.Value.EndOfDay();
                }
                else if (criterion.Data.StartDateTime.HasValue && !criterion.Data.EndDateTime.HasValue)
                {
                    driveSearchRequest.SignedComparator = 1;
                    driveSearchRequest.SignedDate = criterion.Data.StartDateTime.Value.StartOfDay();
                }
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.ContractType))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.ContractType);
                driveSearchRequest.Templates = criterion.Data.ContractTypes;
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.Country))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.Country);
                driveSearchRequest.Country = criterion.Data.Country;
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.Project))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.Project);
                driveSearchRequest.Project = criterion.Data.Project;
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.Owner))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.Owner);
                driveSearchRequest.Owner = criterion.Data.ReferenceElementId;
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.Contributors))
            {
                SearchCriterionRequest criterion = searchRequest.Criteria.First(c => c.Type == SearchCriterionRequest.TypeEnum.Contributors);
                driveSearchRequest.Contributors = criterion.Data.ContributorsEmails.ToArray();
            }

            if (searchRequest.Criteria.Any(c => c.Type == SearchCriterionRequest.TypeEnum.TermSheets))
            {
                foreach (SearchCriterionRequest termSheet in searchRequest.Criteria.Where(c => c.Type == SearchCriterionRequest.TypeEnum.TermSheets))
                {
                    switch (termSheet.Data.Type)
                    {
                        case SearchCriterionDataRequest.TermSheetTypeEnum.DateTime:
                            if (termSheet.Data.StartDateTime.HasValue && termSheet.Data.EndDateTime.HasValue)
                            {
                                driveSearchRequest.SheetElements.Add(
                                    new SearchSheetElement()
                                    {
                                        Name = termSheet.Data.Title,
                                        Value = termSheet.Data.StartDateTime.Value.StartOfDay().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        Between = termSheet.Data.EndDateTime.Value.StartOfDay().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        Comparator = 6
                                    });
                            }
                            else if (!termSheet.Data.StartDateTime.HasValue && termSheet.Data.EndDateTime.HasValue)
                            {
                                driveSearchRequest.SheetElements.Add(
                                    new SearchSheetElement()
                                    {
                                        Name = termSheet.Data.Title,
                                        Value = termSheet.Data.EndDateTime.Value.StartOfDay().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        Comparator = 2
                                    });
                            }
                            else if (termSheet.Data.StartDateTime.HasValue && !termSheet.Data.EndDateTime.HasValue)
                            {
                                driveSearchRequest.SheetElements.Add(
                                    new SearchSheetElement()
                                    {
                                        Name = termSheet.Data.Title,
                                        Value = termSheet.Data.StartDateTime.Value.StartOfDay().ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        Comparator = 1
                                    });
                            }
                            break;
                        case SearchCriterionDataRequest.TermSheetTypeEnum.Number:
                            driveSearchRequest.SheetElements.Add(
                                new SearchSheetElement() { Name = termSheet.Data.Title, Value = termSheet.Data.Number.ToString(), Comparator = (int)termSheet.Data.Comparator });
                            break;
                        default:
                            driveSearchRequest.SheetElements.Add(
                                new SearchSheetElement() { Name = termSheet.Data.Title, Value = termSheet.Data.Text, Comparator = 5 });
                            break;
                    }

                }
            }

            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Drive/QueryAdvancedSearch", "POST", bodyObject: driveSearchRequest);

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await this.GetResponseAsObject<List<DriveItemDTO>>(request);
        }
    }
}
