using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GPLibrary.DataObjects
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class VerifiedLinkTemplateFieldValue
    {
        public string FieldValue { get; set; }
        public bool ShareField { get; set; } = false;
    }

    public class VerifiedLinkTemplateFieldValueList : Dictionary<Guid, VerifiedLinkTemplateFieldValue> { }
}