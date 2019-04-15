// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator 1.0.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.TimeSeriesInsights.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Partial list of time series types returned in a single request.
    /// </summary>
    public partial class GetTypesPage : PagedResponse
    {
        /// <summary>
        /// Initializes a new instance of the GetTypesPage class.
        /// </summary>
        public GetTypesPage()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GetTypesPage class.
        /// </summary>
        /// <param name="continuationToken">If returned, this means that
        /// current results represent a partial result. Continuation token
        /// allows to get the next page of results. To get the next page of
        /// query results, send the same request with continuation token
        /// parameter in "x-ms-continuation" HTTP header.</param>
        /// <param name="types">Partial list of time series types returned in a
        /// single request. Can be empty if server was unable to fill the page
        /// with more types in this request, or there is no more types when
        /// continuation token is null.</param>
        public GetTypesPage(string continuationToken = default(string), IList<TimeSeriesType> types = default(IList<TimeSeriesType>))
            : base(continuationToken)
        {
            Types = types;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets partial list of time series types returned in a single
        /// request. Can be empty if server was unable to fill the page with
        /// more types in this request, or there is no more types when
        /// continuation token is null.
        /// </summary>
        [JsonProperty(PropertyName = "types")]
        public IList<TimeSeriesType> Types { get; private set; }

    }
}