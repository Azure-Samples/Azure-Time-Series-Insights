---
title: Query the Reference Data Management API from Azure Time Series Insights Gen1 environments using C# and MSAL.NET
description: This sample covers how to query the Reference Data Management API from Azure Time Series Insights Gen1 environments using C# and MSAL.NET.
---

# Query the Gen1 Reference Data Management API from Azure Time Series Insights using C# and MSAL.NET

This C# example demonstrates how to query the Reference Data Management API from Azure Time Series Insights Gen1 environments using [MSAL.NET](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) as described in the [Manage Gen1 reference data for an Azure Time Series Insights environment by using C#](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-manage-reference-data-csharp) article.

## Recommended development environment

1. [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) - Version 16.4.2+
1. [NETCore.app](https://www.nuget.org/packages/Microsoft.NETCore.App/2.2.8) - Version 2.2.8
1. [MSAL.NET](https://www.nuget.org/packages/Microsoft.Identity.Client/) - Version 4.7.1

## Setup and configuration

1. Execute the command: `dotnet restore` in this root directory.
1. Follow steps in [Authentication and authorization](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-authentication-and-authorization) to create an application in your tenant. Record the **Application ID**, **Redirect URIs**, and Time Series Insights environment information. Use `http://localhost:8080/` as the **Redirect URI**.
1. Replace all **#PLACEHOLDER#** values with the appropriate information from the preceding step in [Program.cs](./Program.cs).
1. Execute the command `dotnet run`in this root directory.
1. When prompted, use your user profile to login Azure.

## Description

This example demonstrates a few important Azure Active Directory and Azure Time Series Insights features:

1. Acquiring an access token using MSAL.NET **PublicClientApplication**.
1. Sequential CREATE, READ, UPDATE, and DELETE operations against the [Gen1 Reference Data Management API](https://docs.microsoft.com/rest/api/time-series-insights/ga-reference-data-api).
1. Common response codes including [common error codes](https://docs.microsoft.com/rest/api/time-series-insights/ga-reference-data-api#validation-and-error-handling).

    The Reference Data Management API processes each item individually and an error with one item does not prevent the others from successfully completing. For example, if your request has 100 items and one item has an error, then 99 items are written and one is rejected. 

The sample assumes a Reference Data set with the following simple schema:

| Key Name | Type |
| --- | --- |
| uuid | String |

Read [Create a reference data set for your Time Series Insights environment using the Azure portal](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-add-reference-data-set) to learn more.

## See also

* The [Azure Time Series Insights API reference](https://docs.microsoft.com/rest/api/time-series-insights/ga) documentation for all Gen1 REST APIs.

* Follow the [Authentication and authorization](https://docs.microsoft.com/azure/time-series-insights/time-series-insights-authentication-and-authorization#summary-and-best-practices) documentation to register your application in Azure Active Directory.

* The [TSI JS client SDK](https://github.com/microsoft/tsiclient/blob/master/docs/API.md).
