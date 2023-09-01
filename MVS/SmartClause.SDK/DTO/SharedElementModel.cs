using Smartclause.SDK.DTO;
using System;
using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class SharedElementInfos
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }

    public enum SharedElementEnum
    {
        NoAccess = 0,
        Access = 1,
        RequestAccess = 2,
        RequestCanShared = 3,
    }

    public class SharedElementDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ElementId { get; set; }
        public int AccessRequestStatus { get; set; }
        public Nullable<System.DateTime> AccessRequestDate { get; set; }
        public string AccessRequestMsg { get; set; }
        public Nullable<bool> CanShared { get; set; }
    }

    public class UpdateSharedElement
    {
        public string EndUserId { get; set; }
        public bool Value { get; set; }
        public string EndUserEmail { get; set; }
    }

    public class UpdateElementsRequest
    {
        public List<UpdateSharedElement> usersToChangeAcces { get; set; }
        public List<UpdateSharedElement> usersToChangeCanShared { get; set; }
    }

    public class UpdateElementsResponse
    {
        public class User
        {
            public string EndUserId { get; set; }
            public string EndUserEmail { get; set; }
        }

        public List<User> UsersThatGainedAccess { get; set; }
    }
}