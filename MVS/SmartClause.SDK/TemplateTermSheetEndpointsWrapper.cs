using SmartClause.SDK.DTO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    /// <summary>
    /// Endpoints related to TemplateTermSheet
    /// </summary>
    public partial class Client
    {
        /// <summary>
        /// Returns the term sheet elements associated to the template
        /// </summary>
        /// <param name="templateId">the template id</param>
        /// <returns>the template's term sheet elements</returns>
        public async Task<List<TermSheetElement>> GetTemplateTermSheetElements(string templateId, string tenantId = null)
        {
            var request = await CreateHttpWebRequest($"/api/Template/TermSheet/Get/{templateId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            var termSheetResult = await GetResponseAsObject<TermSheetResult>(request);
            return termSheetResult.Elements;
        }

        public async Task<List<TermSheetElement>> AddTemplateTermSheetElements(string templateId,
            List<TermSheetElement> elements, string tenantId = null)
        {
            // Request object
            dynamic bodyObject = new ExpandoObject();
            bodyObject.TemplateId = templateId;
            bodyObject.Elements = elements;

            var request = await CreateHttpWebRequest("/api/Template/TermSheet/Element/BulkAdd", "POST",
                bodyObject: (object)bodyObject);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            try
            {
                var result = await GetResponseAsObject<TermSheetResult>(request);
                return result?.Elements;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update the template term sheet elements
        /// </summary>
        /// <param name="elements">the elements to update</param>
        /// <returns>the updated elements</returns>
        public async Task<List<TermSheetElement>> UpdateTemplateTermSheetElements(List<TermSheetElement> elements, string tenantId = null)
        {
            // Request object 
            dynamic bodyObject = new ExpandoObject();

            List<dynamic> bodyElements = new List<dynamic>();

            foreach (var element in elements)
            {
                dynamic bodyElement = new ExpandoObject();
                bodyElement.ElementId = element.Id;
                bodyElement.Element = element;
                bodyElements.Add(bodyElement);
            }

            bodyObject.Elements = bodyElements;

            try
            {
                var request =
                    await CreateHttpWebRequest("/api/Template/TermSheet/Element/BulkUpdate", "POST",
                        bodyObject: (object)bodyObject);
                if (!string.IsNullOrWhiteSpace(tenantId))
                {
                    request.Headers.Add("TenantId", tenantId);
                }
                var result = await GetResponseAsObject<TermSheetResult>(request);
                return result?.Elements;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<HttpWebResponse> DeleteTemplateTermSheetElement(string templateId, int elementId, string tenantId = null)
        {
            var request = await CreateHttpWebRequest($"/api/Template/TermSheet/Element/{templateId}/Delete/{elementId}",
                "DELETE");
            try
            {
                if (!string.IsNullOrWhiteSpace(tenantId))
                {
                    request.Headers.Add("TenantId", tenantId);
                }
                return (HttpWebResponse)await request.GetResponseAsync();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }
    }
}