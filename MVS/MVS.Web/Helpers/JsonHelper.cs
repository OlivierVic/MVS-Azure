// <copyright file="JsonHelper.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MVS.Web.Helpers;

public static class JsonHelper
{
    private static JsonSerializerSettings JsonSerializerSettings => new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Include,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static HtmlString GetJson(object vm)
    {
        string s = JsonConvert.SerializeObject(vm, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include });
        return new HtmlString(s);
    }

    public static string GetJsonString(object vm) => JsonConvert.SerializeObject(vm, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include });

    public static T ParseJson<T>(string obj) => JsonConvert.DeserializeObject<T>(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Include, MissingMemberHandling = MissingMemberHandling.Ignore });
}
