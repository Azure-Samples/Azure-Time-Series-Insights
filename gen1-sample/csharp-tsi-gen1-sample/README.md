---
title: Query data from Azure Time Series Insights Gen1 environments using C#
description: This sample covers how to query data from Azure Time Series Insights Gen1 environments using C#.
---

# Query data from Azure Time Series Insights Gen1 environments using C#

This C# example demonstrates how to use the [Gen1 Query APIs](https://docs.microsoft.com/rest/api/time-series-insights/ga-query) to query data from Azure Time Series Insights Gen1 environments as described in [Query data from the Azure Time Series Insights Gen1 environment using C#](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-query-data-csharp).

Set up steps:

1. [Provision a Gen1 Azure Time Series Insights](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-get-started) environment.
1. Configure your Azure Time Series Insights environment for Azure Active Directory as described in [Authentication and authorization](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-authentication-and-authorization). 
1. Install the required project dependencies.
1. Edit the sample code below by replacing each **#DUMMY#** with the appropriate environment identifier.
1. Execute the code inside Visual Studio.

The sample shows several basic examples of Query API usage:

* How to acquire an access token through Azure Active Directory using [Microsoft.IdentityModel.Clients.ActiveDirectory](https://www.nuget.org/packages/Microsoft.IdentityModel.Clients.ActiveDirectory/).

* How to pass that acquired access token in the `Authorization` header of subsequent Query API requests. 

* The sample calls each of the Gen1 Query APIs demonstrating how HTTP requests are made to the:
    * [Get Environments API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environments-api) to return the environments the user has access to
    * [Get Environment Availability API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environment-availability-api)
    * [Get Environment Metadata API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environment-metadata-api) to retrieve environment metadata
    * [Get Environments Events API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environment-events-api)
    * [Get Environment Aggregates API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environment-aggregates-api)
    
* How to interact with the Gen1 Query APIs using WSS to message the:

   * [Get Environment Events Streamed API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environment-events-streamed-api)
   * [Get Environment Aggregates Streamed API](https://docs.microsoft.com/rest/api/time-series-insights/ga-query-api#get-environment-aggregates-streamed-api)

## See also

* The [Azure Time Series Insights API reference](https://docs.microsoft.com/rest/api/time-series-insights/ga) documentation for all Gen1 REST APIs.

* Follow the [Authentication and authorization](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-authentication-and-authorization#summary-and-best-practices) documentation to register your application in Azure Active Directory.

* The [TSI JS client SDK](https://github.com/microsoft/tsiclient/blob/master/docs/API.md).
