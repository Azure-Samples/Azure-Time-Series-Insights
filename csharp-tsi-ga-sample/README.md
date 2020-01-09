---
title: Query data from Azure Time Series Insights GA environments using C#
description: This sample covers how to query data from Azure Time Series Insights GA environments using C#.
---

# Query data from Azure Time Series Insights GA environments using C#

This C# example demonstrates how to query data from Azure Time Series Insights GA environments.

Set up steps:
1. Follow steps in [Authentication and authorization](https://docs.microsoft.com/en-us/azure/time-series-insights/time-series-insights-authentication-and-authorization) to create an application in your tenant. Record tenant ID, application ID, and application key.
1. Set all the constants defined at the beginning of the sample.

The sample shows several basic examples of Query API usage:
1. As a preparation step, the access token is acquired through the Azure Active Directory API. This token is required in the `Authorization` header of every Query API request.
1. The list of environments that the user has access to is obtained. One of the environments is picked up as the environment of interest, and further data is queried for this environment.
1. As an example of HTTPS request, availability data is requested for the environment of interest.
1. As an example of web socket request, event aggregates data is requested for the environment of interest. Data is requested for the whole availability time range.

## See also

* The [Azure Time Series Insights API reference](https://docs.microsoft.com/rest/api/time-series-insights/ga) documentation for all General Availability REST APIs.

* Follow the [Authentication and authorization](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-authentication-and-authorization#summary-and-best-practices) documentation to register your application in Azure Active Directory.

* The [TSI JS client SDK](https://github.com/microsoft/tsiclient/blob/master/docs/API.md).
