---
title: Query data from the by Azure Time Series Insights environment using C#
description: This sample covers how to query data from the Time Series Insights environment using C#.
---

# Query data from the Azure Time Series Insights environment using C#

This C# example demonstrates how to query data from the Azure Time Series Insights environment.

Set up steps:
1. Follow steps in [Authentication and authorization](https://docs.microsoft.com/en-us/azure/time-series-insights/time-series-insights-authentication-and-authorization) to create an application in your tenant. Record tenant ID, application ID, and application key.
2. Set all the constants defined at the beginning of the sample.

The sample shows several basic examples of Query API usage:
1. As a preparation step, the access token is acquired through the Azure Active Directory API. This token is required in the `Authorization` header of every Query API request.
2. The list of environments that the user has access to is obtained. One of the environments is picked up as the environment of interest, and further data is queried for this environment.
3. As an example of HTTPS request, availability data is requested for the environment of interest.
4. As an example of web socket request, event aggregates data is requested for the environment of interest. Data is requested for the whole availability time range.

## Next steps

For the full Query API reference, see the [Query API](/rest/api/time-series-insights/time-series-insights-reference-queryapi) document.
